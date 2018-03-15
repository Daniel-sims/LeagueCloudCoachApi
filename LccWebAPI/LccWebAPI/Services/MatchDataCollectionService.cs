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

        int matchesUpdateTotal = 0;
        int matchesUpdatedThisSession = 0;
        
        private void PrintSummary()
        {
            Console.WriteLine("###########SUMMARY###########");
            Console.WriteLine("----------Summoners----------");
            Console.WriteLine(newSummonersAddedToDatabaseTotal + " new summoners added to the database this session.");
            Console.WriteLine(newSummonersAddedThisSession + " new summoners added to the database this run.");
            Console.WriteLine("-----------Matches-----------");
            Console.WriteLine(matchesUpdateTotal + " Matches have been added this session.");
            Console.WriteLine(matchesUpdatedThisSession + " Matches updated this run");

        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while(!cancellationToken.IsCancellationRequested)
            {
                _logging.LogEvent("MatchDataCollectionService started.");
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var summonerRepository = scope.ServiceProvider.GetRequiredService<ISummonerRepository>();

                    try
                    {
                        var challengerPlayers = await _trottledRequestHelper.SendThrottledRequest<League>(async () => await _riotApi.GetChallengerLeagueAsync(RiotSharp.Misc.Region.euw, LeagueQueue.RankedSolo));
                        _logging.LogEvent("Retrieved challenger players.");

                        foreach (var challengerPlayer in challengerPlayers.Entries)
                        {
                            var summoner = await _trottledRequestHelper.SendThrottledRequest<Summoner>(async () => await _riotApi.GetSummonerBySummonerIdAsync(RiotSharp.Misc.Region.euw, Convert.ToInt64(challengerPlayer.PlayerOrTeamId)));
                            _logging.LogEvent("Retrieved summoner - " + summoner.Name + ".");
                            
                            var summonerInDatabase = summonerRepository.GetSummonerByAccountId(summoner.AccountId);
                            if (summoner != null && summonerInDatabase == null)
                            {
                                var newSummoner = new SummonerDto(summoner)
                                {
                                    DateLastUpdated = summoner.RevisionDate
                                };

                                summonerRepository.InsertSummoner(newSummoner);
                                
                                _logging.LogEvent("Summoner " + summoner.Name + " added.");
                                ++newSummonersAddedToDatabaseTotal;
                                ++newSummonersAddedThisSession;

                                //So because this is a new summoner we want to get just their 20 most recent matches
                                var matchList = await _trottledRequestHelper.SendThrottledRequest<MatchList>(async () => await _riotApi.GetMatchListAsync(RiotSharp.Misc.Region.euw, summoner.AccountId, null, null, null, null, null, 0, 50));
                                
                                if(matchList?.Matches?.Count > 0)
                                {
                                    _logging.LogEvent("Retrieved " + matchList?.Matches?.Count + " new matches to add to the database.");
                                    matchesUpdatedThisSession += matchList.Matches.Count;
                                    matchesUpdateTotal += matchList.Matches.Count;
                                }
                                else
                                {
                                    _logging.LogEvent("No Matches found for this summoner.");
                                }
                            }
                            else
                            {
                                _logging.LogEvent("Summoner " + summoner.Name + " already exists in database.");

                                DateTime lastUpdatedDate = summonerInDatabase.DateLastUpdated;
                                DateTime lastRevisionDateFromRiot = summoner.RevisionDate;

                                //If there's been a revision previously!
                                if(lastRevisionDateFromRiot > lastUpdatedDate)
                                {
                                    //Get the new matches between the last revision date and the current date.
                                    var newMatches = await _trottledRequestHelper.SendThrottledRequest<MatchList>(async () => await _riotApi.GetMatchListAsync(RiotSharp.Misc.Region.euw, summoner.AccountId, null, null, null, lastUpdatedDate, DateTime.Now, 0, 25));
                                    if(newMatches?.Matches?.Count > 0)
                                    {
                                        _logging.LogEvent("Summoner " + summoner.Name + " has matches we didn't previously have adding " + newMatches.Matches.Count + " to the database");
                                        matchesUpdatedThisSession += newMatches.Matches.Count;
                                        matchesUpdateTotal += newMatches.Matches.Count;
                                    }
                                    else
                                    {
                                        _logging.LogEvent("Updated the summoner but no new matches!");
                                    }
                                }
                                else
                                {
                                    _logging.LogEvent("Everything is all up to date!");
                                }
                            }
                        }
                    }
                    catch (RiotSharpException e)
                    {
                        _logging.LogEvent("RiotSharpException encountered - " + e.Message + ".");
                        if (e.HttpStatusCode == (HttpStatusCode)429)
                        {
                            _logging.LogEvent("Sleeping for 50 seconds.");
                            await Task.Run(() => Thread.Sleep(50 * 1000));
                        }
                    }
                    catch (Exception e)
                    {
                        _logging.LogEvent("Exception encountered - " + e.Message + ".");
                    }
                    finally
                    {
                        summonerRepository.Save();
                        _logging.LogEvent("Saving changes.");
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
