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
                    using (var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
                    {
                        try
                        {
                            var challengerPlayers = await _throttledRequestHelper.SendThrottledRequest(async () => await _riotApi.League.GetChallengerLeagueAsync(Region.euw, LeagueQueue.RankedSolo));
                            var mastersPlayers = await _throttledRequestHelper.SendThrottledRequest(async () => await _riotApi.League.GetMasterLeagueAsync(Region.euw, LeagueQueue.RankedSolo));

                            var highEloPlayerEntires = challengerPlayers.Entries.Concat(mastersPlayers.Entries);

                            int totalPlayersFound = highEloPlayerEntires.Count();
                            int currentPlayerCount = 0;

                            _logging.LogEvent("Found " + totalPlayersFound + " summoners.");

                            foreach (RiotSharp.Endpoints.LeagueEndpoint.LeaguePosition highEloPlayer in highEloPlayerEntires)
                            {
                                try
                                {
                                    _logging.LogEvent(++currentPlayerCount + "/" + totalPlayersFound + ": " + highEloPlayer.PlayerOrTeamName);

                                    var summoner = await _throttledRequestHelper.SendThrottledRequest(async () => await _riotApi.Summoner.GetSummonerBySummonerIdAsync(Region.euw, long.Parse(highEloPlayer.PlayerOrTeamId)));
                                    var dbSummoner = await dbContext.Summoners.FirstOrDefaultAsync(x => x.AccountId == summoner.AccountId);

                                    if (dbSummoner == null)
                                    {
                                        var newDbSummoner = new Models.DbSummoner.Summoner
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

                                        _logging.LogEvent(dbSummoner.SummonerName + " added to our database.");
                                    }
                                    else
                                    {
                                        _logging.LogEvent(dbSummoner.SummonerName + " already exists in our database.");
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

                                        var matchList = await _throttledRequestHelper.SendThrottledRequest(
                                            async () =>
                                            await _riotApi.Match.GetMatchListAsync(
                                                Region.euw, summoner.AccountId,
                                                null,
                                                null,
                                                null,
                                                collectionFromDate,         //No filter if null on these dates
                                                collectionToDate,
                                                0,                          //starting index
                                                25));                       //ending index

                                        if (matchList != null && matchList?.Matches != null)
                                        {
                                            _logging.LogEvent(" Found " + matchList?.Matches.Count() + " matches for the summoner " + dbSummoner.SummonerName);

                                            foreach (RiotSharp.Endpoints.MatchEndpoint.MatchReference match in matchList?.Matches)
                                            {
                                                if(!dbContext.Matches.Any(x => x.GameId == match.GameId))
                                                {
                                                    var newDbMatch = await ConvertRiotMatchReferenceToDbMatch(match);
                                                    if (newDbMatch != null)
                                                    {
                                                        dbContext.Matches.Add(newDbMatch);
                                                        await dbContext.SaveChangesAsync();

                                                        _logging.LogEvent(" Added new match " + newDbMatch.GameId);
                                                    }
                                                }
                                                else
                                                {
                                                    _logging.LogEvent(" The game " + match.GameId + " already exists in our database.");
                                                }
                                            }
                                        }
                                    }
                                }
                                //This catch block avoids one unhandled exception causing the service to restart
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
                        //This catch block is to stop the app crashing if something goes really tits up somewhere I haven't thought of
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
            }
        }

        private async Task<Models.DbMatch.Match> ConvertRiotMatchReferenceToDbMatch(RiotSharp.Endpoints.MatchEndpoint.MatchReference riotMatchReference)
        {
            try
            {
                var newDbMatch = new Models.DbMatch.Match();

                var riotMatch = await _throttledRequestHelper.SendThrottledRequest( async () => await _riotApi.Match.GetMatchAsync(Region.euw, riotMatchReference.GameId));

                if (riotMatch != null)
                {
                    newDbMatch.GameId = riotMatch.GameId;
                    newDbMatch.GameDuration = riotMatch.GameDuration;
                    newDbMatch.GameDate = riotMatch.GameCreation;
                    newDbMatch.GamePatch = riotMatch.GameVersion;
                    newDbMatch.WinningTeamId = riotMatch.Participants.FirstOrDefault(x => x.Stats.Win).TeamId;

                    var winningTeamParticipants = riotMatch.Participants.Where(x => x.TeamId == newDbMatch.WinningTeamId);
                    var winningTeamstats = riotMatch.Teams.FirstOrDefault(x => x.TeamId == newDbMatch.WinningTeamId);

                    newDbMatch.Teams.Add(ConvertRiotParticipantsToDbTeam(winningTeamParticipants, winningTeamstats));

                    var losingTeamParticipants = riotMatch.Participants.Where(x => x.TeamId != newDbMatch.WinningTeamId);
                    var losingTeamStats = riotMatch.Teams.FirstOrDefault(x => x.TeamId != newDbMatch.WinningTeamId);

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
            var team = new Models.DbMatch.MatchTeam
            {
                BaronKills = teamStats.BaronKills,
                DragonKills = teamStats.DragonKills,
                InhibitorKills = teamStats.InhibitorKills,
                RiftHeraldKills = teamStats.RiftHeraldKills
            };

            foreach (var participant in participants)
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
                            ChampionLevel = participant.Stats.ChampLevel,
                            Items = GetItemsForParticipant(participant),
                            TrinketId = participant.Stats.Item6,
                            Runes = GetRunesForParticipant(participant),
                            SummonerSpells = GetSummonerSpellsForParticipant(participant),

                            //Gold
                            GoldEarned = participant.Stats.GoldEarned,
                            GoldSpent = participant.Stats.GoldSpent,

                            //Vision
                            VisionScore = participant.Stats.VisionScore,
                            WardsPlaced = participant.Stats.WardsPlaced,
                            VisionWardsBoughtInGame = participant.Stats.VisionWardsBoughtInGame,
                            SightWardsBoughtInGame = participant.Stats.SightWardsBoughtInGame,

                            //Damage dealt/taken
                            TotalDamageTaken = participant.Stats.TotalDamageTaken,
                            TotalDamageDealt = participant.Stats.TotalDamageDealt,
                            TotalDamageDealtToChampions = participant.Stats.TotalDamageDealtToChampions,

                            TrueDamageTaken = participant.Stats.TrueDamageTaken,
                            TrueDamageDealt = participant.Stats.TrueDamageDealt,
                            TrueDamageDealtToChampions = participant.Stats.TrueDamageDealtToChampions,

                            MagicalDamageTaken = participant.Stats.MagicalDamageTaken,
                            MagicDamageDealt = participant.Stats.MagicDamageDealt,
                            MagicDamageDealtToChampions = participant.Stats.MagicDamageDealtToChampions,

                            PhysicalDamageTaken = participant.Stats.PhysicalDamageTaken,
                            PhysicalDamageDealt = participant.Stats.PhysicalDamageDealt,
                            PhysicalDamageDealtToChampions = participant.Stats.PhysicalDamageDealtToChampions,

                            LargestCriticalStrike = participant.Stats.LargestCriticalStrike,

                            //objectives
                            FirstTowerAssist = participant.Stats.FirstTowerAssist,
                            FirstTowerKill = participant.Stats.FirstTowerKill,
                            TurretKills = participant.Stats.TurretKills,
                            DamageDealtToTurrets = participant.Stats.DamageDealtToTurrets,
                            
                            FirstInhibitorAssist = participant.Stats.FirstInhibitorAssist,
                            FirstInhibitorKill = participant.Stats.FirstInhibitorKill,
                            InhibitorKills = participant.Stats.InhibitorKills,
                            
                            DamageDealtToObjectives = participant.Stats.DamageDealtToObjectives,
                            ObjectivePlayerScore = participant.Stats.ObjectivePlayerScore,

                            //Kills
                            FirstBloodAssist = participant.Stats.FirstBloodAssist,
                            FirstBloodKill = participant.Stats.FirstBloodKill,

                            LargestMultiKill = participant.Stats.LargestMultiKill,
                            LargestKillingSpree = participant.Stats.LargestKillingSpree,
                            PentaKills = participant.Stats.PentaKills,
                            QuadraKills = participant.Stats.QuadraKills,
                            KillingSprees = participant.Stats.KillingSprees,
                            DoubleKills = participant.Stats.DoubleKills,

                            //Farming
                            NeutralMinionsKilled = participant.Stats.NeutralMinionsKilled,
                            NeutralMinionsKilledEnemyJungle = participant.Stats.NeutralMinionsKilledEnemyJungle,
                            NeutralMinionsKilledTeamJungle = participant.Stats.NeutralMinionsKilledTeamJungle,

                            //Misc info
                            TimeCCingOthers = participant.Stats.TimeCCingOthers,
                            TotalTimeCrowdControlDealt = participant.Stats.TotalTimeCrowdControlDealt,

                            TotalHeal = participant.Stats.TotalHeal,
                            TotalUnitsHealed = participant.Stats.TotalUnitsHealed,
                            
                            TotalScoreRank = participant.Stats.TotalScoreRank
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
            var items = new List<Models.DbMatch.PlayerItem>();

            try
            {
                if (participant?.Stats?.Item0 != 0)
                {
                    items.Add(new Models.DbMatch.PlayerItem() { ItemId = participant.Stats.Item0, ItemSlot = 0});
                }

                if (participant?.Stats.Item1 != 0)
                {
                    items.Add(new Models.DbMatch.PlayerItem() { ItemId = participant.Stats.Item1, ItemSlot = 1 });
                }

                if (participant?.Stats?.Item2 != 0)
                {
                    items.Add(new Models.DbMatch.PlayerItem() { ItemId = participant.Stats.Item2, ItemSlot = 2 });
                }

                if (participant?.Stats?.Item3 != 0)
                {
                    items.Add(new Models.DbMatch.PlayerItem() { ItemId = participant.Stats.Item3, ItemSlot = 3 });
                }

                if (participant?.Stats?.Item4 != 0)
                {
                    items.Add(new Models.DbMatch.PlayerItem() { ItemId = participant.Stats.Item4, ItemSlot = 4 });
                }

                if (participant?.Stats?.Item5 != 0)
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
            var runes = new List<Models.DbMatch.PlayerRune>();

            try
            {
                //Primary Style
                if(participant?.Stats?.PerkPrimaryStyle != 0)
                {
                    runes.Add(new Models.DbMatch.PlayerRune() { RuneId = participant.Stats.PerkPrimaryStyle, RuneSlot = 0 });
                }

                //Primary sub style row one
                if (participant?.Stats?.Perk0 != 0)
                {
                    runes.Add(new Models.DbMatch.PlayerRune() { RuneId = participant.Stats.Perk0, RuneSlot = 1 });
                }

                //Primary sub style row two
                if (participant?.Stats?.Perk1 != 0)
                {
                    runes.Add(new Models.DbMatch.PlayerRune() { RuneId = participant.Stats.Perk1, RuneSlot = 2 });
                }

                //Primary sub style row three
                if (participant?.Stats?.Perk2 != 0)
                {
                    runes.Add(new Models.DbMatch.PlayerRune() { RuneId = participant.Stats.Perk2, RuneSlot = 3 });
                }

                //Primary sub style row four
                if (participant?.Stats?.Perk3 != 0)
                {
                    runes.Add(new Models.DbMatch.PlayerRune() { RuneId = participant.Stats.Perk3, RuneSlot = 4 });
                }

                //secondary Style
                if (participant?.Stats?.PerkSubStyle != 0)
                {
                    runes.Add(new Models.DbMatch.PlayerRune() { RuneId = participant.Stats.PerkSubStyle, RuneSlot = 5 });
                }

                //secondary sub style row one
                if (participant?.Stats?.Perk4 != 0)
                {
                    runes.Add(new Models.DbMatch.PlayerRune() { RuneId = participant.Stats.Perk4, RuneSlot = 6 });
                }

                //secondary sub style row two
                if (participant?.Stats?.Perk5 != 0)
                {
                    runes.Add(new Models.DbMatch.PlayerRune() { RuneId = participant.Stats.Perk5, RuneSlot = 7 });
                }
            }
            catch (Exception ex)
            {
                _logging.LogEvent(" Exception hit when getting runes for participant : " + ex.Message);
            }

            return runes;
        }

        private ICollection<Models.DbMatch.PlayerSummonerSpell> GetSummonerSpellsForParticipant(RiotSharp.Endpoints.MatchEndpoint.Participant participant)
        {
            var summonerSpells = new List<Models.DbMatch.PlayerSummonerSpell>();

            try
            {
                if(participant?.Spell1Id != 0)
                {
                    summonerSpells.Add(new Models.DbMatch.PlayerSummonerSpell() { SummonerSpellId = participant.Spell1Id, SummonerSpellSlot = 0 });
                }

                if (participant?.Spell2Id != 0)
                {
                    summonerSpells.Add(new Models.DbMatch.PlayerSummonerSpell() { SummonerSpellId = participant.Spell2Id, SummonerSpellSlot = 0 });
                }
            }
            catch(Exception ex)
            {
                _logging.LogEvent(" Exception hit when getting summoner spells for participant : " + ex.Message);
            }

            return summonerSpells;
        }
    }
}
