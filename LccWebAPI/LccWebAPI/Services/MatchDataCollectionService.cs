using LccWebAPI.Constants;
using LccWebAPI.DatabaseContexts;
using LccWebAPI.Models;
using LccWebAPI.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RiotSharp;
using RiotSharp.Interfaces;
using RiotSharp.LeagueEndpoint;
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
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogging _logging;
        private readonly IThrottledRequestHelper _trottledRequestHelper;

        public MatchDataCollectionService(IRiotApi riotApi, IServiceScopeFactory serviceScopeFactory,ILogging logging
            , IThrottledRequestHelper throttledRequestHelper)
        {
            _riotApi = riotApi;
            _serviceScopeFactory = serviceScopeFactory;
            _logging = logging;
            _trottledRequestHelper = throttledRequestHelper;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while(!cancellationToken.IsCancellationRequested)
            {
                _logging.LogEvent("MatchDataCollectionService started.");

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    try
                    {
                        var summonerDbContext = scope.ServiceProvider.GetRequiredService<SummonerDtoContext>();

                        var challengerPlayers = await _trottledRequestHelper.SendThrottledRequest<League>(async () => await _riotApi.GetChallengerLeagueAsync(RiotSharp.Misc.Region.euw, LeagueQueue.RankedSolo));

                        _logging.LogEvent("Retrieved challenger players.");

                        foreach (var challengerPlayer in challengerPlayers.Entries)
                        {
                            var summoner = await _trottledRequestHelper.SendThrottledRequest<Summoner>(async () => await _riotApi.GetSummonerBySummonerIdAsync(RiotSharp.Misc.Region.euw, Convert.ToInt64(challengerPlayer.PlayerOrTeamId)));
                            _logging.LogEvent("Retrieved summoner - " + summoner.Name + ".");

                            if(summoner != null && summonerDbContext.Summoners.FirstOrDefault(x => x.Summoner.AccountId.Equals(summoner.AccountId)) == null)
                            {
                                summonerDbContext.Summoners.Add(new SummonerDto(summoner));
                                summonerDbContext.SaveChanges();
                            }
                        }
                    }
                    catch(RiotSharpException e)
                    {
                        _logging.LogEvent("RiotSharpException encountered - " + e.Message + ".");
                        if(e.HttpStatusCode == (HttpStatusCode)429)
                        {
                            _logging.LogEvent("Sleeping for 50 seconds.");
                            await Task.Run(() => Thread.Sleep(50 * 1000));
                        }
                    }
                    catch(Exception e)
                    {
                        _logging.LogEvent("Exception encountered - " + e.Message + ".");
                    }
                }
                _logging.LogEvent("MatchDataCollectionService finished, will wait 10 minutes and start again.");
                await Task.Run(() => Thread.Sleep(600000));
            }
            
        }
    }
}
