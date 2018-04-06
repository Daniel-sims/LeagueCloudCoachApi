using LccWebAPI.Database.Context;
using LccWebAPI.Models.ApiMatch;
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
        private readonly IServiceProvider _serviceProvider;

        public MatchProvider(ILogging logging, IServiceProvider serviceProvider)
        {
            _logging = logging;
            _serviceProvider = serviceProvider;
        }

        public IEnumerable<Models.ApiMatch.Match> GetMatchesForListOfTeamIds(long usersChampionId, IEnumerable<int> teamOne, IEnumerable<int> teamTwo, int matchCount)
        {
            var matchList = new List<Models.ApiMatch.Match>();

            using (var dbContext = _serviceProvider.GetRequiredService<DatabaseContext>())
            {
                try
                {
                    //Find matches in the database matching the users query
                    IList<Models.DbMatch.Match> allMatchesContainingUsersChampion = dbContext.Matches
                        .Include(x => x.Teams).ThenInclude(y => y.Players).ThenInclude(x => x.Runes)
                        .Include(x => x.Teams).ThenInclude(y => y.Players).ThenInclude(x => x.Items)
                        .Include(x => x.Teams).ThenInclude(y => y.Players).ThenInclude(x => x.SummonerSpells).ToList();

                    var fullMatchupMatches = allMatchesContainingUsersChampion
                        .Where(x => x.Teams.Any(y => y.Players.Any(v => v.ChampionId == usersChampionId)))
                        .Where(q => q.Teams
                        .All(t =>
                            teamOne.All(f => t.Players.Select(p => p.ChampionId).Contains(f)) ||
                            teamTwo.All(f => t.Players.Select(p => p.ChampionId).Contains(f)))).ToList();

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
   
