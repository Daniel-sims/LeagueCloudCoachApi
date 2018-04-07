using LccWebAPI.Database.Context;
using LccWebAPI.Models.ApiMatch;
using LccWebAPI.Models.ApiStaticData;
using LccWebAPI.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LccWebAPI.Controllers.Utils.Match
{
    public class MatchProvider : IMatchProvider
    {
        private readonly ILogging _logging;

        private readonly DatabaseContext _dbContext;

        public MatchProvider(ILogging logging, DatabaseContext databaseContext)
        {
            _logging = logging;
            _dbContext = databaseContext;
        }

        public IEnumerable<Models.ApiMatch.Match> GetMatchesForListOfTeamIds(long usersChampionId, IEnumerable<int> teamOne, IEnumerable<int> teamTwo, int matchCount)
        {
            var matchList = new List<Models.ApiMatch.Match>();

            try
            {
                //Find matches in the database matching the users query
                var allMatchesContainingUsersChampion = _dbContext.Matches
                    .Include(x => x.Teams).ThenInclude(y => y.Players).ThenInclude(x => x.Runes)
                    .Include(x => x.Teams).ThenInclude(y => y.Players).ThenInclude(x => x.Items)
                    .Include(x => x.Teams).ThenInclude(y => y.Players).ThenInclude(x => x.SummonerSpells).ToList(); 
                //To list on this avoids some errors to do with ignored includes

                // This Linq query could be added to the one above?
                var fullMatchupMatches = allMatchesContainingUsersChampion
                    .Where(x => x.Teams.Any(y => y.Players.Any(v => v.ChampionId == usersChampionId)))
                    .Where(q => q.Teams
                    .All(t =>
                        teamOne.All(f => t.Players.Select(p => p.ChampionId).Contains(f)) ||
                        teamTwo.All(f => t.Players.Select(p => p.ChampionId).Contains(f)))).ToList();
                //To list on this avoids resharper warning of possible multiple enumerations

                if (fullMatchupMatches.Any())
                {
                    foreach (var match in fullMatchupMatches)
                    {
                        if (matchList.Count == matchCount)
                            break;

                        matchList.Add(ConvertDbMatchToApiMatch(match));
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.LogEvent("Exception caught when getting matches for ids : " + ex.Message);
            }
            
            return matchList;
        }
        
        private Models.ApiMatch.Match ConvertDbMatchToApiMatch(Models.DbMatch.Match dbMatch)
        {
            var match = new Models.ApiMatch.Match
            {
                GameId = dbMatch.GameId,
                GameDate = dbMatch.GameDate,
                GameDuration = dbMatch.GameDuration,
                GamePatch = dbMatch.GamePatch,
                WinningTeamId = dbMatch.WinningTeamId,
                Teams = ConvertDbTeamsToApiTeams(dbMatch.Teams, dbMatch.WinningTeamId)
            };
            
            return match;
        }

        private IEnumerable<MatchTeam> ConvertDbTeamsToApiTeams(ICollection<Models.DbMatch.MatchTeam> dbTeams, int? winningTeamId)
        {
            var matchTeams = new List<MatchTeam>();

            try
            {
                var dbWinningTeam = dbTeams.FirstOrDefault(x => x.TeamId == winningTeamId);
                var dbLosingTeam = dbTeams.FirstOrDefault(x => x.TeamId != winningTeamId);

                if (dbWinningTeam != null && dbLosingTeam != null)
                {
                    var winningTeam = new MatchTeam
                    {
                        BaronKills = dbWinningTeam.BaronKills,
                        DragonKills = dbWinningTeam.DragonKills,
                        InhibitorKills = dbWinningTeam.InhibitorKills,
                        RiftHeraldKills = dbWinningTeam.RiftHeraldKills,
                        TeamId = dbWinningTeam.TeamId,
                        Players = ConvertDbPlayersToApiPlayers(dbWinningTeam.Players)
                    };

                    var losingTeam = new MatchTeam()
                    {
                        BaronKills = dbLosingTeam.BaronKills,
                        DragonKills = dbLosingTeam.DragonKills,
                        InhibitorKills = dbLosingTeam.InhibitorKills,
                        RiftHeraldKills = dbLosingTeam.RiftHeraldKills,
                        TeamId = dbLosingTeam.TeamId,
                        Players = ConvertDbPlayersToApiPlayers(dbLosingTeam.Players)
                    };

                    matchTeams.Add(winningTeam);
                    matchTeams.Add(losingTeam);
                }
            }
            catch (Exception ex)
            {
                _logging.LogEvent("Exception hit converting DbTeam to ApiTeam : " + ex.Message);
            }
            
            return matchTeams;
        }

        private IEnumerable<MatchPlayer> ConvertDbPlayersToApiPlayers(IEnumerable<Models.DbMatch.MatchPlayer> dbPlayers)
        {
            var matchPlayers = new List<MatchPlayer>();

            try
            {
                foreach (var player in dbPlayers)
                {
                    matchPlayers.Add(new MatchPlayer()
                    {
                        // General player data
                        // Lookup actual data?
                        PlayerId = player.PlayerId,

                        // Game Specific data
                        TeamId = player.TeamId,
                        ParticipantId =  player.ParticipantId,
                        
                        Kills = player.Kills,
                        Deaths = player.Deaths,
                        Assists = player.Assists,
                        TotalMinionsKilled = player.TotalMinionsKilled,

                        Champion = GetChampionInformationForChampionId(player.ChampionId),
                        ChampionLevel = player.ChampionLevel,

                        //Items
                        ItemOne = GetItemInformationForItemId(player.Items?.FirstOrDefault(x => x.ItemSlot == 0)?.ItemId),
                        ItemTwo = GetItemInformationForItemId(player.Items?.FirstOrDefault(x => x.ItemSlot == 1)?.ItemId),
                        ItemThree = GetItemInformationForItemId(player.Items?.FirstOrDefault(x => x.ItemSlot == 2)?.ItemId),
                        ItemFour = GetItemInformationForItemId(player.Items?.FirstOrDefault(x => x.ItemSlot == 3)?.ItemId),
                        ItemFive = GetItemInformationForItemId(player.Items?.FirstOrDefault(x => x.ItemSlot == 4)?.ItemId),
                        ItemSix = GetItemInformationForItemId(player.Items?.FirstOrDefault(x => x.ItemSlot == 5)?.ItemId),

                        Trinket = GetItemInformationForItemId(player.TrinketId),

                        //Runes
                        PrimaryRuneStyle = GetParentRuneForRuneId(player.Runes?.FirstOrDefault(x => x.RuneSlot == 0)?.RuneId),
                        PrimaryRuneSubStyleOne = GetRuneInformationForRuneId(player.Runes?.FirstOrDefault(x => x.RuneSlot == 1)?.RuneId),
                        PrimaryRuneSubStyleTwo = GetRuneInformationForRuneId(player.Runes?.FirstOrDefault(x => x.RuneSlot == 2)?.RuneId),
                        PrimaryRuneSubStyleThree = GetRuneInformationForRuneId(player.Runes?.FirstOrDefault(x => x.RuneSlot == 3)?.RuneId),
                        PrimaryRuneSubStyleFour = GetRuneInformationForRuneId(player.Runes?.FirstOrDefault(x => x.RuneSlot == 4)?.RuneId),

                        SecondaryRuneStyle = GetParentRuneForRuneId(player.Runes?.FirstOrDefault(x => x.RuneSlot == 5)?.RuneId),
                        SecondaryRuneStyleOne = GetRuneInformationForRuneId(player.Runes?.FirstOrDefault(x => x.RuneSlot == 6)?.RuneId),
                        SecondaryRuneStyleTwo = GetRuneInformationForRuneId(player.Runes?.FirstOrDefault(x => x.RuneSlot == 7)?.RuneId),

                        //Summoners
                        SummonerSpellOne = GetSummonerSpellInformationForSummonerSpellId(player.SummonerSpells?.FirstOrDefault(x => x.SummonerSpellSlot == 0)?.SummonerSpellId),
                        SummonerSpellTwo = GetSummonerSpellInformationForSummonerSpellId(player.SummonerSpells?.FirstOrDefault(x => x.SummonerSpellSlot == 1)?.SummonerSpellId),

                        //Gold
                        GoldEarned = player.GoldEarned,
                        GoldSpent = player.GoldSpent,
                        
                        //Vision
                        VisionScore = player.VisionScore,
                        WardsPlaced = player.WardsPlaced,
                        VisionWardsBoughtInGame = player.VisionWardsBoughtInGame,
                        SightWardsBoughtInGame = player.SightWardsBoughtInGame,

                        //Damage dealt/taken
                        TotalDamageTaken = player.TotalDamageTaken,
                        TotalDamageDealt = player.TotalDamageDealt,
                        TotalDamageDealtToChampions = player.TotalDamageDealtToChampions,

                        TrueDamageTaken = player.TrueDamageTaken,
                        TrueDamageDealt = player.TrueDamageDealt,
                        TrueDamageDealtToChampions = player.TrueDamageDealtToChampions,

                        MagicDamageTaken = player.MagicDamageTaken,
                        MagicDamageDealt = player.MagicDamageDealt,
                        MagicDamageDealtToChampions = player.MagicDamageDealtToChampions,

                        PhysicalDamageTaken = player.PhysicalDamageTaken,
                        PhysicalDamageDealt = player.PhysicalDamageDealt,
                        PhysicalDamageDealtToChampions = player.PhysicalDamageDealtToChampions,

                        LargestCriticalStrike = player.LargestCriticalStrike,

                        //Objectives
                        FirstTowerAssist = player.FirstTowerAssist,
                        FirstTowerKill = player.FirstTowerKill,
                        TurretKills = player.TurretKills,
                        DamageDealtToTurrets = player.DamageDealtToTurrets,

                        FirstInhibitorAssist = player.FirstInhibitorAssist,
                        FirstInhibitorKill = player.FirstInhibitorKill,
                        InhibitorKills = player.InhibitorKills,

                        DamageDealtToObjectives = player.DamageDealtToObjectives,
                        ObjectivePlayerScore = player.ObjectivePlayerScore,

                        //Kills
                        FirstBloodAssist = player.FirstBloodAssist,
                        FirstBloodKill = player.FirstBloodKill,

                        LargestMultiKill = player.LargestMultiKill,
                        LargestKillingSpree = player.LargestKillingSpree,
                        PentaKills = player.PentaKills,
                        QuadraKills = player.QuadraKills,
                        KillingSprees = player.KillingSprees,
                        DoubleKills = player.DoubleKills,

                        //Farming
                        NeutralMinionsKilled = player.NeutralMinionsKilled,
                        NeutralMinionsKilledEnemyJungle = player.NeutralMinionsKilledEnemyJungle,
                        NeutralMinionsKilledTeamJungle = player.NeutralMinionsKilledTeamJungle,

                        //Misc
                        TimeCCingOthers = player.TimeCCingOthers,
                        TotalTimeCrowdControlDealt = player.TotalTimeCrowdControlDealt,

                        TotalHeal = player.TotalHeal,
                        TotalUnitsHealed = player.TotalUnitsHealed,

                        TotalScoreRank = player.TotalScoreRank
                    });
                }
            }
            catch (Exception ex)
            {
                _logging.LogEvent("Exception hit converting DbPlayer to ApiPlayer : " + ex.Message);
            }

            return matchPlayers;
        }

        private Item GetItemInformationForItemId(long? itemId)
        {
            var item = new Item();

            try
            {
                var itemForItemId = _dbContext.Items.FirstOrDefault(x => x.ItemId == itemId);
                if (itemForItemId != null)
                {
                    item.ItemId = itemForItemId.ItemId;
                    item.ItemName = itemForItemId.ItemName;
                    item.ImageFull = itemForItemId.ImageFull;

                    item.PlainText = itemForItemId.PlainText;

                    item.Description = itemForItemId.Description;
                    item.SanitizedDescription = itemForItemId.SanitizedDescription;
                }
            }
            catch (Exception ex)
            {
                _logging.LogEvent(" Exception hit getting item information for id : " + ex.Message);
            }

            return item;
        }

        private Rune GetRuneInformationForRuneId(long? runeId)
        {
            var rune = new Rune();

            try
            {
                var runeForRuneId = _dbContext.Runes.FirstOrDefault(x => x.RuneId == runeId);
                if (runeForRuneId != null)
                {
                    rune.RuneId = runeForRuneId.RuneId;
                    rune.RuneName = runeForRuneId.RuneName;

                    rune.Key = runeForRuneId.Key;
                    rune.LongDesc = runeForRuneId.LongDesc;
                    rune.ShortDesc = runeForRuneId.ShortDesc;
                }
            }
            catch (Exception ex)
            {
                _logging.LogEvent(" Exception hit getting rune information for id : " + ex.Message);
            }
            
            return rune;
        }

        private Rune GetParentRuneForRuneId(long? runeId)
        {
            var rune = new Rune();

            try
            {
                var runeForRuneId = _dbContext.Runes.FirstOrDefault(x => x.RunePathId == runeId);
                if (runeForRuneId != null)
                {
                    rune.RuneId = runeForRuneId.RunePathId;
                    rune.RuneName = runeForRuneId.RunePathName;
                    rune.Key = runeForRuneId.RunePathName;

                }
            }
            catch (Exception ex)
            {
                _logging.LogEvent(" Exception hit getting rune information for id : " + ex.Message);
            }

            return rune;
        }

        private SummonerSpell GetSummonerSpellInformationForSummonerSpellId(long? summonerSpellId)
        {
            var summonerSpell = new SummonerSpell();

            try
            {
                var summonerSpellForId = _dbContext.SummonerSpells.FirstOrDefault(x => x.SummonerSpellId == summonerSpellId);
                if (summonerSpellForId != null)
                {
                    summonerSpell.SummonerSpellId = summonerSpellForId.SummonerSpellId;
                    summonerSpell.SummonerSpellName = summonerSpellForId.SummonerSpellName;

                    summonerSpell.Description = summonerSpellForId.Description;

                    summonerSpell.ImageFull = summonerSpellForId.ImageFull;
                }
            }
            catch (Exception ex)
            {
                _logging.LogEvent(" Exception hit getting summoner spell information for id : " + ex.Message);
            }
            
            return summonerSpell;
        }

        private Champion GetChampionInformationForChampionId(long? championId)
        {
            var champion = new Champion();

            try
            {
                var championForId = _dbContext.Champions.FirstOrDefault(x => x.ChampionId == championId);
                if (championForId != null)
                {
                    champion.ChampionId = championForId.ChampionId;
                    champion.ChampionName = championForId.ChampionName;
                    champion.ImageFull = championForId.ImageFull;
                }
            }
            catch (Exception ex)
            {
                _logging.LogEvent(" Exception hit getting summoner spell information for id : " + ex.Message);
            }

            return champion;
        }


    }
}
   
