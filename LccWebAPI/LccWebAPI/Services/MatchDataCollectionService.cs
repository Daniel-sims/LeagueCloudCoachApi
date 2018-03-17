using LccWebAPI.Constants;
using LccWebAPI.DatabaseContexts;
using LccWebAPI.Models;
using LccWebAPI.Repository;
using LccWebAPI.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RiotSharp;
using RiotSharp.Interfaces;
using RiotSharp.LeagueEndpoint;
using RiotSharp.MatchEndpoint;
using RiotSharp.SummonerEndpoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace LccWebAPI.Services
{
    public class MatchDataCollectionService : HostedService
    {
        private readonly IRiotApi _riotApi;
        private readonly ILogging _logging;
        private readonly IThrottledRequestHelper _trottledRequestHelper;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public MatchDataCollectionService(IRiotApi riotApi, ILogging logging
            , IThrottledRequestHelper throttledRequestHelper, IServiceScopeFactory serviceScopeFactory)
        {
            _riotApi = riotApi;
            _serviceScopeFactory = serviceScopeFactory;
            _logging = logging;
            _trottledRequestHelper = throttledRequestHelper;
        }

        int newSummonersAddedToDatabaseTotal = 0;
        int newSummonersAddedThisSession = 0;

        int matchesUpdatedTotal = 0;
        int matchesUpdatedThisSession = 0;
        
        private void PrintSummary()
        {
            Console.WriteLine("###########SUMMARY###########");
            Console.WriteLine("----------Total----------");
            Console.WriteLine(newSummonersAddedToDatabaseTotal + " new summoners added to the database this session.");
            Console.WriteLine(matchesUpdatedTotal + " Matches have been added this session.");
            Console.WriteLine("---------This Run--------");
            Console.WriteLine(newSummonersAddedThisSession + " new summoners added to the database this run.");
            Console.WriteLine(matchesUpdatedThisSession + " Matches updated this run");
            Console.WriteLine("#############################");
        }

        // This task is not optmised in any way and has lots of code duplication
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while(!cancellationToken.IsCancellationRequested)
            {
                _logging.LogEvent("MatchDataCollectionService started.");
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    using (var matchRepository = scope.ServiceProvider.GetRequiredService<IMatchupInformationRepository>())
                    using (var matchReferenceRepository = scope.ServiceProvider.GetRequiredService<IMatchReferenceRepository>())
                    using (var summonerRepository = scope.ServiceProvider.GetRequiredService<ISummonerRepository>())
                    {
                        try
                        {
                            var challengerPlayers = await _trottledRequestHelper.SendThrottledRequest<League>(async () => await _riotApi.GetChallengerLeagueAsync(RiotSharp.Misc.Region.euw, LeagueQueue.RankedSolo));

                            int totalPlayers = challengerPlayers.Entries.Count;
                            int currentCount = 0;

                            foreach (var challengerPlayer in challengerPlayers.Entries)
                            {
                                Console.WriteLine(++currentCount + "/" + totalPlayers + " - " + challengerPlayer.PlayerOrTeamName);

                                var summoner = await _trottledRequestHelper.SendThrottledRequest<Summoner>(async () => await _riotApi.GetSummonerBySummonerIdAsync(RiotSharp.Misc.Region.euw, Convert.ToInt64(challengerPlayer.PlayerOrTeamId)));

                                var summonerInDatabase = summonerRepository.GetSummonerByAccountId(summoner.AccountId);
                                if (summoner != null && summonerInDatabase == null)
                                {

                                    summonerRepository.InsertSummoner(new LccSummoner(summoner));
                                    newSummonersAddedToDatabaseTotal++;
                                    newSummonersAddedThisSession++;

                                    var matchList = await _trottledRequestHelper.SendThrottledRequest<MatchList>(async () => await _riotApi.GetMatchListAsync(RiotSharp.Misc.Region.euw, summoner.AccountId, null, null, null, null, null, 0, 50));

                                    if (matchList?.Matches != null && matchList?.Matches?.Count > 0)
                                    {
                                        foreach (var match in matchList?.Matches)
                                        {
                                            if (matchReferenceRepository.GetMatchReferenceByGameId(match.GameId) == null)
                                            {
                                                matchReferenceRepository.InsertMatchReference(new LccMatchReference(match));
                                                matchesUpdatedTotal++;
                                                matchesUpdatedThisSession++;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    DateTime lastUpdatedDate = summonerInDatabase.LastUpdated;
                                    DateTime lastRevisionDateFromRiot = summoner.RevisionDate;

                                    if (lastRevisionDateFromRiot > lastUpdatedDate)
                                    {
                                        if (summonerInDatabase.Level != summoner.Level)
                                        {
                                            Console.WriteLine("Revision included a level change");
                                        }
                                        summonerInDatabase.LastUpdated = summoner.RevisionDate;

                                        summonerRepository.UpdateSummoner(summonerInDatabase);

                                        var newMatchereferences = await _trottledRequestHelper.SendThrottledRequest<MatchList>(async () => await _riotApi.GetMatchListAsync(RiotSharp.Misc.Region.euw, summoner.AccountId, null, null, null, lastUpdatedDate, DateTime.Now, 0, 25));

                                        if (newMatchereferences?.Matches != null && newMatchereferences?.Matches?.Count > 0)
                                        {
                                            foreach (var matchReference in newMatchereferences?.Matches)
                                            {
                                                if (matchReferenceRepository.GetMatchReferenceByGameId(matchReference.GameId) == null)
                                                {
                                                    matchReferenceRepository.InsertMatchReference(new LccMatchReference(matchReference));
                                                    Console.WriteLine("Added match reference to our database.");

                                                    var match = await _trottledRequestHelper.SendThrottledRequest<Match>(async () => await _riotApi.GetMatchAsync(RiotSharp.Misc.Region.euw, matchReference.GameId));

                                                    var lccMatch = new LccMatchupInformation
                                                    {
                                                        GameId = match.GameId,
                                                        ChampionIds = match.Participants.Select(x => Convert.ToInt64(x.ChampionId)).ToList()
                                                    };

                                                    matchesUpdatedTotal++;
                                                    matchesUpdatedThisSession++;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Match already exists in our database.");
                                                }
                                            }
                                        }
                                    }
                                }

                                summonerRepository.Save();
                                matchReferenceRepository.Save();
                            }
                        }
                        catch (RiotSharpException e)
                        {
                            _logging.LogEvent("RiotSharpException encountered - " + e.Message + ".");
                            if (e.HttpStatusCode == (HttpStatusCode)429)
                            {
                                _logging.LogEvent("RateLimitExceeded exception - Sleeping for 50 seconds.");
                                await Task.Run(() => Thread.Sleep(50 * 1000));
                            }
                        }
                        catch (Exception e)
                        {
                            _logging.LogEvent("Exception encountered - " + e.Message + ".");
                        }
                    }
                }

                PrintSummary();

                matchesUpdatedThisSession = 0;
                newSummonersAddedThisSession = 0;

                _logging.LogEvent("MatchDataCollectionService finished, will wait 5 minutes and start again.");
                await Task.Run(() => Thread.Sleep(300000));
            }
        }
    }
}
