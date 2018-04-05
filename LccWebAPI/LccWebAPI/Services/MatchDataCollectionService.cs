using LccWebAPI.Constants;
using LccWebAPI.Database.Context;
using LccWebAPI.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RiotSharp;
using RiotSharp.Interfaces;
using RiotSharp.Misc;
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
        private readonly IServiceProvider _serviceProvider;

        public MatchDataCollectionService(IRiotApi riotApi, 
            ILogging logging, 
            IThrottledRequestHelper throttledRequestHelper,
            IServiceProvider serviceProvider)
        {
            _riotApi = riotApi;
            _logging = logging;
            _throttledRequestHelper = throttledRequestHelper;
            _serviceProvider = serviceProvider;
        }
       
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _logging.LogEvent("MatchDataCollectionService started.");

                using (var scope = _serviceProvider.CreateScope())
                {
                    try
                    {
                        RiotSharp.Endpoints.LeagueEndpoint.League challengerPlayers = await _throttledRequestHelper.SendThrottledRequest(async () => await _riotApi.League.GetChallengerLeagueAsync(Region.euw, LeagueQueue.RankedSolo));
                        RiotSharp.Endpoints.LeagueEndpoint.League mastersPlayers = await _throttledRequestHelper.SendThrottledRequest(async () => await _riotApi.League.GetMasterLeagueAsync(Region.euw, LeagueQueue.RankedSolo));

                        IEnumerable<RiotSharp.Endpoints.LeagueEndpoint.LeaguePosition> highEloPlayerEntires = challengerPlayers.Entries.Concat(mastersPlayers.Entries);
                            
                        int totalPlayersFound = highEloPlayerEntires.Count();
                        int currentPlayerCount = 0;

                        _logging.LogEvent("Found " + totalPlayersFound + " summoners.");

                        foreach (RiotSharp.Endpoints.LeagueEndpoint.LeaguePosition highEloPlayer in highEloPlayerEntires)
                        {
                            try
                            {
                                using (var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
                                {
                                    _logging.LogEvent(++currentPlayerCount + "/" + totalPlayersFound + ": " + highEloPlayer.PlayerOrTeamName);

                                    RiotSharp.Endpoints.SummonerEndpoint.Summoner summoner = await _throttledRequestHelper.SendThrottledRequest(async () => await _riotApi.Summoner.GetSummonerBySummonerIdAsync(Region.euw, Convert.ToInt64(highEloPlayer.PlayerOrTeamId)));

                                    Models.DbSummoner.Summoner dbSummoner = await dbContext.Summoners.FirstOrDefaultAsync(x => x.AccountId == summoner.AccountId);

                                    if (dbSummoner == null)
                                    {
                                        Models.DbSummoner.Summoner newDbSummoner = new Models.DbSummoner.Summoner
                                        {
                                            SummonerId = summoner.Id,
                                            AccountId = summoner.AccountId,
                                            ProfileIconId = summoner.ProfileIconId,
                                            Level = summoner.Level,
                                            RevisionDate = summoner.RevisionDate,
                                            SummonerName = summoner.Name,
                                            LastUpdatedDate = new DateTime()
                                        };

                                        dbContext.Summoners.Add(newDbSummoner);
                                        await dbContext.SaveChangesAsync();

                                        dbSummoner = newDbSummoner;
                                    }

                                    // If the summoner has had updates post the date what we have on our records
                                    // RevisionDate can be anything from a level up to new games played
                                    if (summoner.RevisionDate > dbSummoner.LastUpdatedDate)
                                    {
                                        /*
                                            * TODO: Update Summoner information
                                            *  I.E Level, name...etc
                                            */

                                        DateTime? collectionFromDate;
                                        DateTime? collectionToDate;

                                        if (dbSummoner.LastUpdatedDate == DateTime.MinValue)
                                        {
                                            collectionFromDate = null;
                                            collectionToDate = null;
                                        }
                                        else
                                        {
                                            collectionFromDate = dbSummoner.LastUpdatedDate;
                                            collectionToDate = DateTime.Now;
                                        }

                                        RiotSharp.Endpoints.MatchEndpoint.MatchList matchList = await _throttledRequestHelper.SendThrottledRequest(
                                            async () =>
                                            await _riotApi.Match.GetMatchListAsync(
                                                Region.euw, summoner.AccountId,
                                                null,
                                                null,
                                                null,
                                                collectionFromDate,
                                                collectionToDate,
                                                0,                          //starting index
                                                75));                       //ending index

                                        if (matchList != null && matchList?.Matches != null)
                                        {
                                            foreach (RiotSharp.Endpoints.MatchEndpoint.MatchReference match in matchList?.Matches)
                                            {
                                                Models.DbMatch.Match newDbMatch = await ConvertRiotMatchReferenceToDbMatch(match);
                                                if (newDbMatch != null)
                                                {
                                                    dbContext.Matches.Add(newDbMatch);
                                                    await dbContext.SaveChangesAsync();
                                                }
                                            }
                                        }
                                    }
                                }
                                }
                            catch (RiotSharp.RiotSharpException ex)
                            {
                                _logging.LogEvent(" RiotSharpException: " + ex.Message);
                            }
                            catch (Exception ex)
                            {
                                _logging.LogEvent(" Exception: " + ex.Message);
                            }
                        }
                    }
                    catch (RiotSharp.RiotSharpException ex)
                    {
                        _logging.LogEvent(" RiotSharpException: " + ex.Message);
                    }
                    catch(Exception ex)
                    {
                        _logging.LogEvent(" Exception: " + ex.Message);
                    } 
                }
            }
        }

        private async Task<Models.DbMatch.Match> ConvertRiotMatchReferenceToDbMatch(RiotSharp.Endpoints.MatchEndpoint.MatchReference riotMatchReference)
        {
            try
            {
                Models.DbMatch.Match newDbMatch = new Models.DbMatch.Match();

                RiotSharp.Endpoints.MatchEndpoint.Match riotMatch = await _riotApi.Match.GetMatchAsync(Region.euw, riotMatchReference.GameId);

                if (riotMatch != null)
                {
                    newDbMatch.GameId = riotMatch.GameId;
                    newDbMatch.GameDuration = riotMatch.GameDuration;
                    newDbMatch.GameDate = riotMatch.GameCreation;
                    newDbMatch.GamePatch = riotMatch.GameVersion;
                    newDbMatch.WinningTeamId = riotMatch.Participants.FirstOrDefault(x => x.Stats.Win).TeamId;

                    IEnumerable<RiotSharp.Endpoints.MatchEndpoint.Participant> winningTeamParticipants = riotMatch.Participants.Where(x => x.TeamId == newDbMatch.WinningTeamId);
                    RiotSharp.Endpoints.MatchEndpoint.TeamStats winningTeamstats = riotMatch.Teams.FirstOrDefault(x => x.TeamId == newDbMatch.WinningTeamId);

                    newDbMatch.Teams.Add(ConvertRiotParticipantsToDbTeam(winningTeamParticipants, winningTeamstats));

                    IEnumerable<RiotSharp.Endpoints.MatchEndpoint.Participant> losingTeamParticipants = riotMatch.Participants.Where(x => x.TeamId != newDbMatch.WinningTeamId);
                    RiotSharp.Endpoints.MatchEndpoint.TeamStats losingTeamStats = riotMatch.Teams.FirstOrDefault(x => x.TeamId != newDbMatch.WinningTeamId);

                    newDbMatch.Teams.Add(ConvertRiotParticipantsToDbTeam(losingTeamParticipants, losingTeamStats));
                    
                    return newDbMatch;
                }
            }
            catch(Exception e)
            {
                _logging.LogEvent(" Exception hit when converting Riot Match Reference to DbMatch. Reason: " + e.Message);
            }
            
            return null;
        }

        private Models.DbMatch.MatchTeam ConvertRiotParticipantsToDbTeam(IEnumerable<RiotSharp.Endpoints.MatchEndpoint.Participant> participants, RiotSharp.Endpoints.MatchEndpoint.TeamStats teamStats)
        {
            Models.DbMatch.MatchTeam team = new Models.DbMatch.MatchTeam
            {
                BaronKills = teamStats.BaronKills,
                DragonKills = teamStats.DragonKills,
                InhibitorKills = teamStats.InhibitorKills,
                RiftHeraldKills = teamStats.RiftHeraldKills
            };

            foreach (RiotSharp.Endpoints.MatchEndpoint.Participant participant in participants)
            {
                try
                {
                    team.Players.Add
                    (
                        new Models.DbMatch.MatchPlayer()
                        {
                            TeamId = participant.TeamId,
                            ParticipantId = participant.ParticipantId,
                            Kills = participant.Stats.Kills,
                            Deaths = participant.Stats.Deaths,
                            Assists = participant.Stats.Assists,
                            ChampionId = participant.ChampionId,
                            SummonerSpells = GetSummonerSpellsForParticipant(participant),
                            Items = GetItemsForParticipant(participant),
                            TrinketId = participant.Stats.Item6,
                            Runes = GetRunesForParticipant(participant)
                        }
                    );
                }
                catch(Exception e)
                {
                    _logging.LogEvent(" Exception encountered when converting Riot Participants to DbTeam. Reason: " + e.Message);
                }
            }

            return team;
        }
        
        private ICollection<Models.DbMatch.PlayerItem> GetItemsForParticipant(RiotSharp.Endpoints.MatchEndpoint.Participant participant)
        {
            ICollection<Models.DbMatch.PlayerItem> items = new List<Models.DbMatch.PlayerItem>();

            try
            {
                if (participant.Stats.Item0 != 0)
                {
                    items.Add(new Models.DbMatch.PlayerItem() { ItemId = participant.Stats.Item0, ItemSlot = 0});
                }

                if (participant.Stats.Item1 != 0)
                {
                    items.Add(new Models.DbMatch.PlayerItem() { ItemId = participant.Stats.Item1, ItemSlot = 1 });
                }

                if (participant.Stats.Item2 != 0)
                {
                    items.Add(new Models.DbMatch.PlayerItem() { ItemId = participant.Stats.Item2, ItemSlot = 2 });
                }

                if (participant.Stats.Item3 != 0)
                {
                    items.Add(new Models.DbMatch.PlayerItem() { ItemId = participant.Stats.Item3, ItemSlot = 3 });
                }

                if (participant.Stats.Item4 != 0)
                {
                    items.Add(new Models.DbMatch.PlayerItem() { ItemId = participant.Stats.Item4, ItemSlot = 4 });
                }

                if (participant.Stats.Item5 != 0)
                {
                    items.Add(new Models.DbMatch.PlayerItem() { ItemId = participant.Stats.Item5, ItemSlot = 5 });
                }
            }
            catch(Exception ex)
            {
                _logging.LogEvent(" Exception hit when getting items for participant : " + ex.Message);
            }
            
            return items;
        }
        
        private ICollection<Models.DbMatch.PlayerRune> GetRunesForParticipant(RiotSharp.Endpoints.MatchEndpoint.Participant participant)
        {
            ICollection<Models.DbMatch.PlayerRune> runes = new List<Models.DbMatch.PlayerRune>();

            return runes;
        }

        private ICollection<Models.DbMatch.PlayerSummonerSpell> GetSummonerSpellsForParticipant(RiotSharp.Endpoints.MatchEndpoint.Participant participant)
        {
            ICollection<Models.DbMatch.PlayerSummonerSpell> summonerSpells = new List<Models.DbMatch.PlayerSummonerSpell>();

            return summonerSpells;
        }


        private Models.DbMatch.Match CreateMatch(long testMatchId)
        {
            return new Models.DbMatch.Match
            {
                GameId = testMatchId,
                Teams = new List<Models.DbMatch.MatchTeam>
                {
                    new Models.DbMatch.MatchTeam()
                    {
                        Players = new List<Models.DbMatch.MatchPlayer>()
                        {
                            new Models.DbMatch.MatchPlayer()
                            {
                                TeamId = 100,
                                ParticipantId = 1,
                                Kills = 3,
                                Deaths = 0,
                                Assists = 2,
                                ChampionId = 44,
                                Items = new List<Models.DbMatch.PlayerItem>()
                                {
                                    new Models.DbMatch.PlayerItem()
                                    {
                                        ItemId = 412,
                                        ItemSlot = 1
                                    },
                                    new Models.DbMatch.PlayerItem()
                                    {
                                        ItemId = 651,
                                        ItemSlot = 2
                                    },
                                    new Models.DbMatch.PlayerItem()
                                    {
                                        ItemId = 22,
                                        ItemSlot = 3
                                    }
                                },
                                Runes = new List<Models.DbMatch.PlayerRune>()
                                {
                                    new Models.DbMatch.PlayerRune()
                                    {
                                        RuneId = 662,
                                        RuneSlot = 1
                                    },
                                     new Models.DbMatch.PlayerRune()
                                    {
                                        RuneId = 41,
                                        RuneSlot = 2
                                    }
                                },
                                SummonerSpells = new List<Models.DbMatch.PlayerSummonerSpell>()
                                {
                                    new Models.DbMatch.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 22,
                                        SummonerSpellSlot = 1
                                    },
                                    new Models.DbMatch.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 14,
                                        SummonerSpellSlot = 2
                                    }
                                }
                            },
                            new Models.DbMatch.MatchPlayer()
                            {
                                TeamId = 100,
                                ParticipantId = 2,
                                Kills = 3,
                                Deaths = 0,
                                Assists = 2,
                                ChampionId = 44,
                                Items = new List<Models.DbMatch.PlayerItem>()
                                {
                                    new Models.DbMatch.PlayerItem()
                                    {
                                        ItemId = 412,
                                        ItemSlot = 1
                                    },
                                    new Models.DbMatch.PlayerItem()
                                    {
                                        ItemId = 651,
                                        ItemSlot = 2
                                    },
                                    new Models.DbMatch.PlayerItem()
                                    {
                                        ItemId = 22,
                                        ItemSlot = 3
                                    }
                                },
                                Runes = new List<Models.DbMatch.PlayerRune>()
                                {
                                    new Models.DbMatch.PlayerRune()
                                    {
                                        RuneId = 662,
                                        RuneSlot = 1
                                    },
                                     new Models.DbMatch.PlayerRune()
                                    {
                                        RuneId = 41,
                                        RuneSlot = 2
                                    }
                                },
                                SummonerSpells = new List<Models.DbMatch.PlayerSummonerSpell>()
                                {
                                    new Models.DbMatch.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 22,
                                        SummonerSpellSlot = 1
                                    },
                                    new Models.DbMatch.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 14,
                                        SummonerSpellSlot = 2
                                    }
                                }
                            },
                            new Models.DbMatch.MatchPlayer()
                            {
                                TeamId = 100,
                                ParticipantId = 3,
                                Kills = 3,
                                Deaths = 0,
                                Assists = 2,
                                ChampionId = 44,
                                Items = new List<Models.DbMatch.PlayerItem>()
                                {
                                    new Models.DbMatch.PlayerItem()
                                    {
                                        ItemId = 412,
                                        ItemSlot = 1
                                    },
                                    new Models.DbMatch.PlayerItem()
                                    {
                                        ItemId = 651,
                                        ItemSlot = 2
                                    },
                                    new Models.DbMatch.PlayerItem()
                                    {
                                        ItemId = 22,
                                        ItemSlot = 3
                                    }
                                },
                                Runes = new List<Models.DbMatch.PlayerRune>()
                                {
                                    new Models.DbMatch.PlayerRune()
                                    {
                                        RuneId = 662,
                                        RuneSlot = 1
                                    },
                                     new Models.DbMatch.PlayerRune()
                                    {
                                        RuneId = 41,
                                        RuneSlot = 2
                                    }
                                },
                                SummonerSpells = new List<Models.DbMatch.PlayerSummonerSpell>()
                                {
                                    new Models.DbMatch.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 22,
                                        SummonerSpellSlot = 1
                                    },
                                    new Models.DbMatch.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 14,
                                        SummonerSpellSlot = 2
                                    }
                                }
                            },
                            new Models.DbMatch.MatchPlayer()
                            {
                                TeamId = 100,
                                ParticipantId = 4,
                                Kills = 3,
                                Deaths = 0,
                                Assists = 2,
                                ChampionId = 44,
                                Items = new List<Models.DbMatch.PlayerItem>()
                                {
                                    new Models.DbMatch.PlayerItem()
                                    {
                                        ItemId = 412,
                                        ItemSlot = 1
                                    },
                                    new Models.DbMatch.PlayerItem()
                                    {
                                        ItemId = 651,
                                        ItemSlot = 2
                                    },
                                    new Models.DbMatch.PlayerItem()
                                    {
                                        ItemId = 22,
                                        ItemSlot = 3
                                    }
                                },
                                Runes = new List<Models.DbMatch.PlayerRune>()
                                {
                                    new Models.DbMatch.PlayerRune()
                                    {
                                        RuneId = 662,
                                        RuneSlot = 1
                                    },
                                     new Models.DbMatch.PlayerRune()
                                    {
                                        RuneId = 41,
                                        RuneSlot = 2
                                    }
                                },
                                SummonerSpells = new List<Models.DbMatch.PlayerSummonerSpell>()
                                {
                                    new Models.DbMatch.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 22,
                                        SummonerSpellSlot = 1
                                    },
                                    new Models.DbMatch.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 14,
                                        SummonerSpellSlot = 2
                                    }
                                }
                            },
                            new Models.DbMatch.MatchPlayer()
                            {
                                TeamId = 100,
                                ParticipantId = 5,
                                Kills = 3,
                                Deaths = 0,
                                Assists = 2,
                                ChampionId = 44,
                                Items = new List<Models.DbMatch.PlayerItem>()
                                {
                                    new Models.DbMatch.PlayerItem()
                                    {
                                        ItemId = 412,
                                        ItemSlot = 1
                                    },
                                    new Models.DbMatch.PlayerItem()
                                    {
                                        ItemId = 651,
                                        ItemSlot = 2
                                    },
                                    new Models.DbMatch.PlayerItem()
                                    {
                                        ItemId = 22,
                                        ItemSlot = 3
                                    }
                                },
                                Runes = new List<Models.DbMatch.PlayerRune>()
                                {
                                    new Models.DbMatch.PlayerRune()
                                    {
                                        RuneId = 662,
                                        RuneSlot = 1
                                    },
                                     new Models.DbMatch.PlayerRune()
                                    {
                                        RuneId = 41,
                                        RuneSlot = 2
                                    }
                                },
                                SummonerSpells = new List<Models.DbMatch.PlayerSummonerSpell>()
                                {
                                    new Models.DbMatch.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 22,
                                        SummonerSpellSlot = 1
                                    },
                                    new Models.DbMatch.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 14,
                                        SummonerSpellSlot = 2
                                    }
                                }
                            },
                        }
                    },
                    new Models.DbMatch.MatchTeam()
                    {
                        Players = new List<Models.DbMatch.MatchPlayer>()
                        {
                            new Models.DbMatch.MatchPlayer()
                            {
                                TeamId = 200,
                                ParticipantId = 6,
                                Kills = 3,
                                Deaths = 0,
                                Assists = 2,
                                ChampionId = 44,
                                Items = new List<Models.DbMatch.PlayerItem>()
                                {
                                    new Models.DbMatch.PlayerItem()
                                    {
                                        ItemId = 412,
                                        ItemSlot = 1
                                    },
                                    new Models.DbMatch.PlayerItem()
                                    {
                                        ItemId = 651,
                                        ItemSlot = 2
                                    },
                                    new Models.DbMatch.PlayerItem()
                                    {
                                        ItemId = 22,
                                        ItemSlot = 3
                                    }
                                },
                                Runes = new List<Models.DbMatch.PlayerRune>()
                                {
                                    new Models.DbMatch.PlayerRune()
                                    {
                                        RuneId = 662,
                                        RuneSlot = 1
                                    },
                                     new Models.DbMatch.PlayerRune()
                                    {
                                        RuneId = 41,
                                        RuneSlot = 2
                                    }
                                },
                                SummonerSpells = new List<Models.DbMatch.PlayerSummonerSpell>()
                                {
                                    new Models.DbMatch.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 22,
                                        SummonerSpellSlot = 1
                                    },
                                    new Models.DbMatch.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 14,
                                        SummonerSpellSlot = 2
                                    }
                                }
                            },
                            new Models.DbMatch.MatchPlayer()
                            {
                                TeamId = 200,
                                ParticipantId = 7,
                                Kills = 3,
                                Deaths = 0,
                                Assists = 2,
                                ChampionId = 44,
                                Items = new List<Models.DbMatch.PlayerItem>()
                                {
                                    new Models.DbMatch.PlayerItem()
                                    {
                                        ItemId = 412,
                                        ItemSlot = 1
                                    },
                                    new Models.DbMatch.PlayerItem()
                                    {
                                        ItemId = 651,
                                        ItemSlot = 2
                                    },
                                    new Models.DbMatch.PlayerItem()
                                    {
                                        ItemId = 22,
                                        ItemSlot = 3
                                    }
                                },
                                Runes = new List<Models.DbMatch.PlayerRune>()
                                {
                                    new Models.DbMatch.PlayerRune()
                                    {
                                        RuneId = 662,
                                        RuneSlot = 1
                                    },
                                     new Models.DbMatch.PlayerRune()
                                    {
                                        RuneId = 41,
                                        RuneSlot = 2
                                    }
                                },
                                SummonerSpells = new List<Models.DbMatch.PlayerSummonerSpell>()
                                {
                                    new Models.DbMatch.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 22,
                                        SummonerSpellSlot = 1
                                    },
                                    new Models.DbMatch.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 14,
                                        SummonerSpellSlot = 2
                                    }
                                }
                            },
                            new Models.DbMatch.MatchPlayer()
                            {
                                TeamId = 200,
                                ParticipantId = 8,
                                Kills = 3,
                                Deaths = 0,
                                Assists = 2,
                                ChampionId = 44,
                                Items = new List<Models.DbMatch.PlayerItem>()
                                {
                                    new Models.DbMatch.PlayerItem()
                                    {
                                        ItemId = 412,
                                        ItemSlot = 1
                                    },
                                    new Models.DbMatch.PlayerItem()
                                    {
                                        ItemId = 651,
                                        ItemSlot = 2
                                    },
                                    new Models.DbMatch.PlayerItem()
                                    {
                                        ItemId = 22,
                                        ItemSlot = 3
                                    }
                                },
                                Runes = new List<Models.DbMatch.PlayerRune>()
                                {
                                    new Models.DbMatch.PlayerRune()
                                    {
                                        RuneId = 662,
                                        RuneSlot = 1
                                    },
                                     new Models.DbMatch.PlayerRune()
                                    {
                                        RuneId = 41,
                                        RuneSlot = 2
                                    }
                                },
                                SummonerSpells = new List<Models.DbMatch.PlayerSummonerSpell>()
                                {
                                    new Models.DbMatch.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 22,
                                        SummonerSpellSlot = 1
                                    },
                                    new Models.DbMatch.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 14,
                                        SummonerSpellSlot = 2
                                    }
                                }
                            },
                            new Models.DbMatch.MatchPlayer()
                            {
                                TeamId = 200,
                                ParticipantId = 9,
                                Kills = 3,
                                Deaths = 0,
                                Assists = 2,
                                ChampionId = 44,
                                Items = new List<Models.DbMatch.PlayerItem>()
                                {
                                    new Models.DbMatch.PlayerItem()
                                    {
                                        ItemId = 412,
                                        ItemSlot = 1
                                    },
                                    new Models.DbMatch.PlayerItem()
                                    {
                                        ItemId = 651,
                                        ItemSlot = 2
                                    },
                                    new Models.DbMatch.PlayerItem()
                                    {
                                        ItemId = 22,
                                        ItemSlot = 3
                                    }
                                },
                                Runes = new List<Models.DbMatch.PlayerRune>()
                                {
                                    new Models.DbMatch.PlayerRune()
                                    {
                                        RuneId = 662,
                                        RuneSlot = 1
                                    },
                                     new Models.DbMatch.PlayerRune()
                                    {
                                        RuneId = 41,
                                        RuneSlot = 2
                                    }
                                },
                                SummonerSpells = new List<Models.DbMatch.PlayerSummonerSpell>()
                                {
                                    new Models.DbMatch.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 22,
                                        SummonerSpellSlot = 1
                                    },
                                    new Models.DbMatch.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 14,
                                        SummonerSpellSlot = 2
                                    }
                                }
                            },
                            new Models.DbMatch.MatchPlayer()
                            {
                                TeamId = 200,
                                ParticipantId = 10,
                                Kills = 3,
                                Deaths = 0,
                                Assists = 2,
                                ChampionId = 44,
                                Items = new List<Models.DbMatch.PlayerItem>()
                                {
                                    new Models.DbMatch.PlayerItem()
                                    {
                                        ItemId = 412,
                                        ItemSlot = 1
                                    },
                                    new Models.DbMatch.PlayerItem()
                                    {
                                        ItemId = 651,
                                        ItemSlot = 2
                                    },
                                    new Models.DbMatch.PlayerItem()
                                    {
                                        ItemId = 22,
                                        ItemSlot = 3
                                    }
                                },
                                Runes = new List<Models.DbMatch.PlayerRune>()
                                {
                                    new Models.DbMatch.PlayerRune()
                                    {
                                        RuneId = 662,
                                        RuneSlot = 1
                                    },
                                     new Models.DbMatch.PlayerRune()
                                    {
                                        RuneId = 41,
                                        RuneSlot = 2
                                    }
                                },
                                SummonerSpells = new List<Models.DbMatch.PlayerSummonerSpell>()
                                {
                                    new Models.DbMatch.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 22,
                                        SummonerSpellSlot = 1
                                    },
                                    new Models.DbMatch.PlayerSummonerSpell()
                                    {
                                        SummonerSpellId = 14,
                                        SummonerSpellSlot = 2
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

    }
}
