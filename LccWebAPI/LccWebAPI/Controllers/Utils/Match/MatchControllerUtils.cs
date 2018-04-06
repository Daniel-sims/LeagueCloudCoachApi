using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LccWebAPI.Models.ApiMatch;
using LccWebAPI.Utils;

namespace LccWebAPI.Controllers.Utils.Match
{
    public class MatchControllerUtils : IMatchControllerUtils
    {
        private readonly ILogging _logging;

        public MatchControllerUtils(ILogging logging)
        {
            _logging = logging;
        }

        public Models.ApiMatch.Match ConvertDbMatchToApiMatch(Models.DbMatch.Match dbMatch)
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
                var winningTeamPlayers =
                    dbTeams.Where(x => x.TeamId == winningTeamId).Select(x => x.Players).FirstOrDefault();

                var losingTeamPlayers =
                    dbTeams.Where(x => x.TeamId != winningTeamId).Select(x => x.Players).FirstOrDefault();

                var winningTeam = new MatchTeam
                {
                    Players = ConvertDbPlayersToApiPlayers(winningTeamPlayers)
                };

                var losingTeam = new MatchTeam()
                {
                    Players = ConvertDbPlayersToApiPlayers(losingTeamPlayers)
                };

                matchTeams.Add(winningTeam);
                matchTeams.Add(losingTeam);
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
                foreach (var player in matchPlayers)
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

                        ChampionId = player.ChampionId,
                        ChampionLevel = player.ChampionLevel,

                        //Items

                        //Runes

                        //Summoners

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

    }
}
   
