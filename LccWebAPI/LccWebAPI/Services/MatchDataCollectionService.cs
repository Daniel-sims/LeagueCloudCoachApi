﻿using LccWebAPI.Database.Context;
using LccWebAPI.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RiotSharp.Endpoints.LeagueEndpoint;
using RiotSharp.Endpoints.MatchEndpoint;
using RiotSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LccWebAPI.Services
{
    public class MatchDataCollectionService : HostedService
    {
        private readonly IRiotApi _riotApi;
        private readonly ILogging _logging;
        private readonly IThrottledRequestHelper _throttledRequestHelper;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IServiceProvider _serviceProvider;

        public MatchDataCollectionService(IRiotApi riotApi, 
            ILogging logging, 
            IThrottledRequestHelper throttledRequestHelper, 
            IServiceScopeFactory serviceScopeFactory,
            IServiceProvider serviceProvider)
        {
            _riotApi = riotApi;
            _serviceScopeFactory = serviceScopeFactory;
            _logging = logging;
            _throttledRequestHelper = throttledRequestHelper;
            _serviceProvider = serviceProvider;
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

        private Models.Db.Match CreateMatch(long testMatchId)
        {
            return new Models.Db.Match
            {
                GameId = testMatchId,
                Teams = new List<Models.Db.MatchTeam>
                {
                    new Models.Db.MatchTeam()
                    {
                        Players = new List<Models.Db.MatchPlayer>()
                        {
                            new Models.Db.MatchPlayer()
                            {
                                TeamId = 100,
                                ParticipantId = 1,
                                Kills = 3,
                                Deaths = 0,
                                Assists = 2,
                                ChampionId = 44,
                                Items = new List<Models.Db.PlayerItem>()
                                {
                                    new Models.Db.PlayerItem()
                                    {
                                        ItemId = 412,
                                        ItemSlot = 1
                                    },
                                    new Models.Db.PlayerItem()
                                    {
                                        ItemId = 651,
                                        ItemSlot = 2
                                    },
                                    new Models.Db.PlayerItem()
                                    {
                                        ItemId = 22,
                                        ItemSlot = 3
                                    }
                                },
                                Runes = new List<Models.Db.PlayerRune>()
                                {
                                    new Models.Db.PlayerRune()
                                    {
                                        RuneId = 662,
                                        RuneSlot = 1
                                    },
                                     new Models.Db.PlayerRune()
                                    {
                                        RuneId = 41,
                                        RuneSlot = 2
                                    }
                                },
                                SummonerSpells = new List<Models.Db.PlayerSummonerSpell>()
                                {
                                    new Models.Db.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 22,
                                        SummonerSpellSlot = 1
                                    },
                                    new Models.Db.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 14,
                                        SummonerSpellSlot = 2
                                    }
                                }
                            },
                            new Models.Db.MatchPlayer()
                            {
                                TeamId = 100,
                                ParticipantId = 2,
                                Kills = 3,
                                Deaths = 0,
                                Assists = 2,
                                ChampionId = 44,
                                Items = new List<Models.Db.PlayerItem>()
                                {
                                    new Models.Db.PlayerItem()
                                    {
                                        ItemId = 412,
                                        ItemSlot = 1
                                    },
                                    new Models.Db.PlayerItem()
                                    {
                                        ItemId = 651,
                                        ItemSlot = 2
                                    },
                                    new Models.Db.PlayerItem()
                                    {
                                        ItemId = 22,
                                        ItemSlot = 3
                                    }
                                },
                                Runes = new List<Models.Db.PlayerRune>()
                                {
                                    new Models.Db.PlayerRune()
                                    {
                                        RuneId = 662,
                                        RuneSlot = 1
                                    },
                                     new Models.Db.PlayerRune()
                                    {
                                        RuneId = 41,
                                        RuneSlot = 2
                                    }
                                },
                                SummonerSpells = new List<Models.Db.PlayerSummonerSpell>()
                                {
                                    new Models.Db.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 22,
                                        SummonerSpellSlot = 1
                                    },
                                    new Models.Db.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 14,
                                        SummonerSpellSlot = 2
                                    }
                                }
                            },
                            new Models.Db.MatchPlayer()
                            {
                                TeamId = 100,
                                ParticipantId = 3,
                                Kills = 3,
                                Deaths = 0,
                                Assists = 2,
                                ChampionId = 44,
                                Items = new List<Models.Db.PlayerItem>()
                                {
                                    new Models.Db.PlayerItem()
                                    {
                                        ItemId = 412,
                                        ItemSlot = 1
                                    },
                                    new Models.Db.PlayerItem()
                                    {
                                        ItemId = 651,
                                        ItemSlot = 2
                                    },
                                    new Models.Db.PlayerItem()
                                    {
                                        ItemId = 22,
                                        ItemSlot = 3
                                    }
                                },
                                Runes = new List<Models.Db.PlayerRune>()
                                {
                                    new Models.Db.PlayerRune()
                                    {
                                        RuneId = 662,
                                        RuneSlot = 1
                                    },
                                     new Models.Db.PlayerRune()
                                    {
                                        RuneId = 41,
                                        RuneSlot = 2
                                    }
                                },
                                SummonerSpells = new List<Models.Db.PlayerSummonerSpell>()
                                {
                                    new Models.Db.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 22,
                                        SummonerSpellSlot = 1
                                    },
                                    new Models.Db.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 14,
                                        SummonerSpellSlot = 2
                                    }
                                }
                            },
                            new Models.Db.MatchPlayer()
                            {
                                TeamId = 100,
                                ParticipantId = 4,
                                Kills = 3,
                                Deaths = 0,
                                Assists = 2,
                                ChampionId = 44,
                                Items = new List<Models.Db.PlayerItem>()
                                {
                                    new Models.Db.PlayerItem()
                                    {
                                        ItemId = 412,
                                        ItemSlot = 1
                                    },
                                    new Models.Db.PlayerItem()
                                    {
                                        ItemId = 651,
                                        ItemSlot = 2
                                    },
                                    new Models.Db.PlayerItem()
                                    {
                                        ItemId = 22,
                                        ItemSlot = 3
                                    }
                                },
                                Runes = new List<Models.Db.PlayerRune>()
                                {
                                    new Models.Db.PlayerRune()
                                    {
                                        RuneId = 662,
                                        RuneSlot = 1
                                    },
                                     new Models.Db.PlayerRune()
                                    {
                                        RuneId = 41,
                                        RuneSlot = 2
                                    }
                                },
                                SummonerSpells = new List<Models.Db.PlayerSummonerSpell>()
                                {
                                    new Models.Db.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 22,
                                        SummonerSpellSlot = 1
                                    },
                                    new Models.Db.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 14,
                                        SummonerSpellSlot = 2
                                    }
                                }
                            },
                            new Models.Db.MatchPlayer()
                            {
                                TeamId = 100,
                                ParticipantId = 5,
                                Kills = 3,
                                Deaths = 0,
                                Assists = 2,
                                ChampionId = 44,
                                Items = new List<Models.Db.PlayerItem>()
                                {
                                    new Models.Db.PlayerItem()
                                    {
                                        ItemId = 412,
                                        ItemSlot = 1
                                    },
                                    new Models.Db.PlayerItem()
                                    {
                                        ItemId = 651,
                                        ItemSlot = 2
                                    },
                                    new Models.Db.PlayerItem()
                                    {
                                        ItemId = 22,
                                        ItemSlot = 3
                                    }
                                },
                                Runes = new List<Models.Db.PlayerRune>()
                                {
                                    new Models.Db.PlayerRune()
                                    {
                                        RuneId = 662,
                                        RuneSlot = 1
                                    },
                                     new Models.Db.PlayerRune()
                                    {
                                        RuneId = 41,
                                        RuneSlot = 2
                                    }
                                },
                                SummonerSpells = new List<Models.Db.PlayerSummonerSpell>()
                                {
                                    new Models.Db.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 22,
                                        SummonerSpellSlot = 1
                                    },
                                    new Models.Db.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 14,
                                        SummonerSpellSlot = 2
                                    }
                                }
                            },
                        }
                    },
                    new Models.Db.MatchTeam()
                    {
                        Players = new List<Models.Db.MatchPlayer>()
                        {
                            new Models.Db.MatchPlayer()
                            {
                                TeamId = 200,
                                ParticipantId = 6,
                                Kills = 3,
                                Deaths = 0,
                                Assists = 2,
                                ChampionId = 44,
                                Items = new List<Models.Db.PlayerItem>()
                                {
                                    new Models.Db.PlayerItem()
                                    {
                                        ItemId = 412,
                                        ItemSlot = 1
                                    },
                                    new Models.Db.PlayerItem()
                                    {
                                        ItemId = 651,
                                        ItemSlot = 2
                                    },
                                    new Models.Db.PlayerItem()
                                    {
                                        ItemId = 22,
                                        ItemSlot = 3
                                    }
                                },
                                Runes = new List<Models.Db.PlayerRune>()
                                {
                                    new Models.Db.PlayerRune()
                                    {
                                        RuneId = 662,
                                        RuneSlot = 1
                                    },
                                     new Models.Db.PlayerRune()
                                    {
                                        RuneId = 41,
                                        RuneSlot = 2
                                    }
                                },
                                SummonerSpells = new List<Models.Db.PlayerSummonerSpell>()
                                {
                                    new Models.Db.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 22,
                                        SummonerSpellSlot = 1
                                    },
                                    new Models.Db.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 14,
                                        SummonerSpellSlot = 2
                                    }
                                }
                            },
                            new Models.Db.MatchPlayer()
                            {
                                TeamId = 200,
                                ParticipantId = 7,
                                Kills = 3,
                                Deaths = 0,
                                Assists = 2,
                                ChampionId = 44,
                                Items = new List<Models.Db.PlayerItem>()
                                {
                                    new Models.Db.PlayerItem()
                                    {
                                        ItemId = 412,
                                        ItemSlot = 1
                                    },
                                    new Models.Db.PlayerItem()
                                    {
                                        ItemId = 651,
                                        ItemSlot = 2
                                    },
                                    new Models.Db.PlayerItem()
                                    {
                                        ItemId = 22,
                                        ItemSlot = 3
                                    }
                                },
                                Runes = new List<Models.Db.PlayerRune>()
                                {
                                    new Models.Db.PlayerRune()
                                    {
                                        RuneId = 662,
                                        RuneSlot = 1
                                    },
                                     new Models.Db.PlayerRune()
                                    {
                                        RuneId = 41,
                                        RuneSlot = 2
                                    }
                                },
                                SummonerSpells = new List<Models.Db.PlayerSummonerSpell>()
                                {
                                    new Models.Db.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 22,
                                        SummonerSpellSlot = 1
                                    },
                                    new Models.Db.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 14,
                                        SummonerSpellSlot = 2
                                    }
                                }
                            },
                            new Models.Db.MatchPlayer()
                            {
                                TeamId = 200,
                                ParticipantId = 8,
                                Kills = 3,
                                Deaths = 0,
                                Assists = 2,
                                ChampionId = 44,
                                Items = new List<Models.Db.PlayerItem>()
                                {
                                    new Models.Db.PlayerItem()
                                    {
                                        ItemId = 412,
                                        ItemSlot = 1
                                    },
                                    new Models.Db.PlayerItem()
                                    {
                                        ItemId = 651,
                                        ItemSlot = 2
                                    },
                                    new Models.Db.PlayerItem()
                                    {
                                        ItemId = 22,
                                        ItemSlot = 3
                                    }
                                },
                                Runes = new List<Models.Db.PlayerRune>()
                                {
                                    new Models.Db.PlayerRune()
                                    {
                                        RuneId = 662,
                                        RuneSlot = 1
                                    },
                                     new Models.Db.PlayerRune()
                                    {
                                        RuneId = 41,
                                        RuneSlot = 2
                                    }
                                },
                                SummonerSpells = new List<Models.Db.PlayerSummonerSpell>()
                                {
                                    new Models.Db.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 22,
                                        SummonerSpellSlot = 1
                                    },
                                    new Models.Db.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 14,
                                        SummonerSpellSlot = 2
                                    }
                                }
                            },
                            new Models.Db.MatchPlayer()
                            {
                                TeamId = 200,
                                ParticipantId = 9,
                                Kills = 3,
                                Deaths = 0,
                                Assists = 2,
                                ChampionId = 44,
                                Items = new List<Models.Db.PlayerItem>()
                                {
                                    new Models.Db.PlayerItem()
                                    {
                                        ItemId = 412,
                                        ItemSlot = 1
                                    },
                                    new Models.Db.PlayerItem()
                                    {
                                        ItemId = 651,
                                        ItemSlot = 2
                                    },
                                    new Models.Db.PlayerItem()
                                    {
                                        ItemId = 22,
                                        ItemSlot = 3
                                    }
                                },
                                Runes = new List<Models.Db.PlayerRune>()
                                {
                                    new Models.Db.PlayerRune()
                                    {
                                        RuneId = 662,
                                        RuneSlot = 1
                                    },
                                     new Models.Db.PlayerRune()
                                    {
                                        RuneId = 41,
                                        RuneSlot = 2
                                    }
                                },
                                SummonerSpells = new List<Models.Db.PlayerSummonerSpell>()
                                {
                                    new Models.Db.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 22,
                                        SummonerSpellSlot = 1
                                    },
                                    new Models.Db.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 14,
                                        SummonerSpellSlot = 2
                                    }
                                }
                            },
                            new Models.Db.MatchPlayer()
                            {
                                TeamId = 200,
                                ParticipantId = 10,
                                Kills = 3,
                                Deaths = 0,
                                Assists = 2,
                                ChampionId = 44,
                                Items = new List<Models.Db.PlayerItem>()
                                {
                                    new Models.Db.PlayerItem()
                                    {
                                        ItemId = 412,
                                        ItemSlot = 1
                                    },
                                    new Models.Db.PlayerItem()
                                    {
                                        ItemId = 651,
                                        ItemSlot = 2
                                    },
                                    new Models.Db.PlayerItem()
                                    {
                                        ItemId = 22,
                                        ItemSlot = 3
                                    }
                                },
                                Runes = new List<Models.Db.PlayerRune>()
                                {
                                    new Models.Db.PlayerRune()
                                    {
                                        RuneId = 662,
                                        RuneSlot = 1
                                    },
                                     new Models.Db.PlayerRune()
                                    {
                                        RuneId = 41,
                                        RuneSlot = 2
                                    }
                                },
                                SummonerSpells = new List<Models.Db.PlayerSummonerSpell>()
                                {
                                    new Models.Db.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 22,
                                        SummonerSpellSlot = 1
                                    },
                                    new Models.Db.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 14,
                                        SummonerSpellSlot = 2
                                    }
                                }
                            },
                        }
                    }
                }
            };
        }
        // This task is not optmised in any way and has lots of code duplication
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _logging.LogEvent("MatchDataCollectionService started.");

                using (var scope = _serviceProvider.CreateScope())
                {
                    using (var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
                    {
                        long testMatchId = 5512222;

                        dbContext.Matches.Add(CreateMatch(testMatchId));
                        dbContext.SaveChanges();

                        var match = dbContext.Matches
                            .Include(x => x.Teams).ThenInclude(y => y.Players).ThenInclude(x => x.Runes)
                            .Include(x => x.Teams).ThenInclude(y => y.Players).ThenInclude(x => x.Items)
                            .Include(x => x.Teams).ThenInclude(y => y.Players).ThenInclude(x => x.SummonerSpells)
                            .FirstOrDefault(x => x.GameId == 51122);
                        await Task.Run(() => Thread.Sleep(100000));
                    }
                }

            //    _logging.LogEvent("MatchDataCollectionService started.");

                    //using (var scope = _serviceScopeFactory.CreateScope())
                  //  {
                //        using (var basicMatchupInformationRepository = scope.ServiceProvider.GetRequiredService<IBasicMatchupInformationRepository>())
                //        using (var summonerRepository = scope.ServiceProvider.GetRequiredService<ISummonerRepository>())
                //        {
                //            try
                //            {
                //                //_logging.LogEvent("Current matches count - " + matchupInformationRepository.GetAllMatchups().Count());

                //                League challengerPlayers = await _throttledRequestHelper.SendThrottledRequest(async () => await _riotApi.League.GetChallengerLeagueAsync(Region.euw, LeagueQueue.RankedSolo));
                //                League mastersPlayers = await _throttledRequestHelper.SendThrottledRequest(async () => await _riotApi.League.GetMasterLeagueAsync(Region.euw, LeagueQueue.RankedSolo));

                //                IEnumerable<LeaguePosition> highEloPlayerEntires = challengerPlayers.Entries.Concat(mastersPlayers.Entries);

                //                int totalPlayers = highEloPlayerEntires.Count();
                //                int currentCount = 0;

                //                foreach (var highEloPlayer in highEloPlayerEntires)
                //                {
                //                    _logging.LogEvent(++currentCount + "/" + totalPlayers + " - " + highEloPlayer.PlayerOrTeamName);

                //                    Summoner summoner = await _throttledRequestHelper.SendThrottledRequest(async () => await _riotApi.Summoner.GetSummonerBySummonerIdAsync(RiotSharp.Misc.Region.euw, Convert.ToInt64(highEloPlayer.PlayerOrTeamId)));

                //                    Db_LccSummoner summonerInDatabase = summonerRepository.GetSummonerByAccountId(summoner.AccountId);
                //                    if (summoner != null && summonerInDatabase == null)
                //                    {
                //                        summonerRepository.InsertSummoner(
                //                            new Db_LccSummoner()
                //                            {
                //                                AccountId = summoner.AccountId,
                //                                SummonerName = summoner.Name,
                //                                LastUpdatedTime = DateTime.Now

                //                            });

                //                        newSummonersAddedToDatabaseTotal++;
                //                        newSummonersAddedThisSession++;

                //                        MatchList matchList = await _throttledRequestHelper.SendThrottledRequest(async () => await _riotApi.Match.GetMatchListAsync(Region.euw, summoner.AccountId, null, null, null, null, null, 0, 75));
                //                        if (matchList != null && matchList?.Matches != null)
                //                        {
                //                            await GetRiotMatchupInformationAndAddIfNotExisting(basicMatchupInformationRepository, matchList, highEloPlayerEntires);
                //                        }
                //                    }
                //                    else
                //                    {
                //                        DateTime lastUpdatedDate = summonerInDatabase.LastUpdatedTime;
                //                        DateTime lastRevisionDateFromRiot = summoner.RevisionDate;

                //                        if (lastRevisionDateFromRiot > lastUpdatedDate)
                //                        {
                //                            summonerInDatabase.LastUpdatedTime = summoner.RevisionDate;
                //                            summonerRepository.UpdateSummoner(summonerInDatabase);

                //                            MatchList newMatches = await _throttledRequestHelper.SendThrottledRequest(async () => await _riotApi.Match.GetMatchListAsync(RiotSharp.Misc.Region.euw, summoner.AccountId, null, null, null, lastUpdatedDate, DateTime.Now, 0, 25));
                //                            if (newMatches != null && newMatches?.Matches != null)
                //                            {
                //                                await GetRiotMatchupInformationAndAddIfNotExisting(basicMatchupInformationRepository, newMatches, highEloPlayerEntires);
                //                            }
                //                        }
                //                    }

                //                    summonerRepository.Save();
                //                }
                //            }
                //            catch (RiotSharpException e)
                //            {
                //                _logging.LogEvent("RiotSharpException encountered - " + e.Message + ".");
                //                if (e.HttpStatusCode == (HttpStatusCode)429)
                //                {
                //                    _logging.LogEvent("RateLimitExceeded exception - Sleeping for 50 seconds.");
                //                    await Task.Run(() => Thread.Sleep(50 * 1000));
                //                }
                //            }
                //            catch (Exception e)
                //            {
                //                _logging.LogEvent("Exception encountered - " + e.Message + ".");
                //            }
                //        }
                //    }

                //    PrintSummary();

                //    matchesUpdatedThisSession = 0;
                //    newSummonersAddedThisSession = 0;

                //    _logging.LogEvent("MatchDataCollectionService finished, will wait 10 minutes and start again.");
                //    await Task.Run(() => Thread.Sleep(600000));
            }
        }
        
        //private async Task GetRiotMatchupInformationAndAddIfNotExisting(IBasicMatchupInformationRepository matchupInformationRepository, MatchList matchlist, IEnumerable<LeaguePosition> highEloPlayerEntires)
        //{
            //foreach (var match in matchlist?.Matches)
            //{
            //    var m = matchupInformationRepository.GetMatchupByGameId(match.GameId);
            //    if(m != null)
            //    {
            //        int i = 0;
            //        i++;
            //    }

            //    if (match.Queue == LeagueQueue.RankedSoloId && m == null)
            //    {
            //        Match riotMatchInformation = await _throttledRequestHelper.SendThrottledRequest<Match>(async () => await _riotApi.Match.GetMatchAsync(RiotSharp.Misc.Region.euw, match.GameId));
                    
            //        if (riotMatchInformation != null & riotMatchInformation?.Participants != null)
            //        {
            //            var winningTeamId = riotMatchInformation.Teams.Find(x => x.Win == MatchOutcome.Win).TeamId;

            //            List<Db_LccBasicMatchInfoPlayer> winningTeam = new List<Db_LccBasicMatchInfoPlayer>();
            //            List<Db_LccBasicMatchInfoPlayer> losingTeam = new List<Db_LccBasicMatchInfoPlayer>();

            //            foreach (Participant player in riotMatchInformation.Participants)
            //            {
            //                Player matchPlayer = riotMatchInformation.ParticipantIdentities.FirstOrDefault(x => x.ParticipantId == player.ParticipantId).Player;

            //                Db_LccBasicMatchInfoPlayer lccMatchupInformationPlayer = 
            //                    new Db_LccBasicMatchInfoPlayer()
            //                    {
            //                        ChampionId = player.ChampionId,
            //                        Lane = player.Timeline.Lane,
            //                        PlayerAccountId = matchPlayer.AccountId,
            //                        SummonerName = matchPlayer.SummonerName
            //                    };

            //                if (player.TeamId == winningTeamId)
            //                {
            //                    winningTeam.Add(lccMatchupInformationPlayer);
            //                }
            //                else
            //                {
            //                    losingTeam.Add(lccMatchupInformationPlayer);
            //                }
            //            }

            //            matchupInformationRepository.InsertMatchup(
            //                new Db_LccBasicMatchInfo()
            //                {
            //                    GameId = match.GameId,
            //                    MatchDate = match.Timestamp,
            //                    WinningTeamChampions = winningTeam,
            //                    LosingTeamChampions = losingTeam
            //                });

            //            matchesUpdatedTotal++;
            //            matchesUpdatedThisSession++;

            //            _logging.LogEvent("Added new matchup No:" + matchesUpdatedTotal);
            //        }
            //    }
            //}
       // }
    }
}
