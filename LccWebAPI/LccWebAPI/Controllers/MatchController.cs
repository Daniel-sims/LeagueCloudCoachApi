using System;
using System.Collections.Generic;
using System.Linq;
using LccWebAPI.Controllers.Utils.Match;
using Microsoft.AspNetCore.Mvc;
using RiotSharp.Interfaces;
using System.Threading.Tasks;
using LccWebAPI.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LccWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class MatchController : Controller
    {
        private IRiotApi _riotApi;
        private readonly IServiceProvider _serviceProvider;

        public MatchController(
            IRiotApi riotApi, 
            IServiceProvider serviceProvider
            )
        {
            _riotApi = riotApi;
            _serviceProvider = serviceProvider;
        }

        [HttpGet("GetMatchup")]
        public async Task<JsonResult> GetMatchup(long usersChampionId, long[] friendlyTeamChampions, long[] enemyTeamChampions, int maxMatchLimit = 5)
        {
            IList<long> friendlyTeamChampionIds = new List<long>(friendlyTeamChampions) { usersChampionId };
            IList<long> enemyTeamChampionIds = enemyTeamChampions;

            using (var dbContext = _serviceProvider.GetRequiredService<DatabaseContext>())
            {
                try
                {
                    var allMatchesContainingUsersChampion = dbContext.Matches
                        .Include(x => x.Teams).ThenInclude(y => y.Players).ThenInclude(x => x.Runes)
                        .Include(x => x.Teams).ThenInclude(y => y.Players).ThenInclude(x => x.Items)
                        .Include(x => x.Teams).ThenInclude(y => y.Players).ThenInclude(x => x.SummonerSpells)
                        .Where(x => x.Teams.Any(y => y.Players.Any(v => v.ChampionId == usersChampionId))).ToList();

                    //This query
                    var fullMatchupMatches = allMatchesContainingUsersChampion
                        .Where(x => x.Teams
                            .All(y => y.Players
                                .Any(v => (friendlyTeamChampionIds.Contains(v.ChampionId)) || (enemyTeamChampionIds.Contains(v.ChampionId))))).ToList();

                }
                catch (Exception e)
                {
                    int i = 0;
                    i++;
                }
                
            }
            
            //var m = _basicMatchupInformationRepository.GetAllMatchups();
            //IEnumerable<Db_LccBasicMatchInfo> allMatchesInDatabase = _matchupInformationRepository.GetAllMatchups();
            //IList<long> friendlyTeamChampionIds = new List<long>(friendlyTeamChampions) { usersChampionId };
            //IList<long> enemyTeamChampionIds = enemyTeamChampions;

            //IEnumerable<Db_LccBasicMatchInfo> allMatchesContainingUsersChampion = allMatchesInDatabase.Where
            //    (x => x.LosingTeamChampions.Any(p => p.ChampionId == usersChampionId || x.WinningTeamChampions.Any(u => u.ChampionId == usersChampionId)));

            //IEnumerable<Db_LccBasicMatchInfo> allMatchesWithRequestedTeams = new List<Db_LccBasicMatchInfo>();
            ////if a summoner name parameter is passed filter based on this too
            //Console.WriteLine(DateTime.Now + ": Finding matches for requested matchup");
            //if (summonerName != "")
            //{
            //    allMatchesWithRequestedTeams =
            //    allMatchesContainingUsersChampion.Where
            //    //Check to see if the Enemys team Ids are the losing team, and the friendly team are the winning team
            //    (q => (enemyTeamChampionIds.All(e => q.LosingTeamChampions.Any(l => l.ChampionId == e && l.SummonerName == summonerName))
            //    && friendlyTeamChampionIds.All(f => q.WinningTeamChampions.Any(l => l.ChampionId == f && l.SummonerName == summonerName)))
            //    //Check to see if the winning team Ids are the losing team, and the enemy team are the winning team
            //    || (enemyTeamChampionIds.All(e => q.WinningTeamChampions.Any(l => l.ChampionId == e && l.SummonerName == summonerName))
            //    && friendlyTeamChampionIds.All(f => q.LosingTeamChampions.Any(l => l.ChampionId == f && l.SummonerName == summonerName))));
            //}
            //else
            //{
            //    allMatchesContainingUsersChampion.Where
            //   //Check to see if the Enemys team Ids are the losing team, and the friendly team are the winning team
            //   (q => (enemyTeamChampionIds.All(e => q.LosingTeamChampions.Any(l => l.ChampionId == e))
            //   && friendlyTeamChampionIds.All(f => q.WinningTeamChampions.Any(l => l.ChampionId == f)))
            //   //Check to see if the winning team Ids are the losing team, and the enemy team are the winning team
            //   || (enemyTeamChampionIds.All(e => q.WinningTeamChampions.Any(l => l.ChampionId == e))
            //   && friendlyTeamChampionIds.All(f => q.LosingTeamChampions.Any(l => l.ChampionId == f))));
            //}
            //Console.WriteLine(DateTime.Now + ": Found - " + allMatchesWithRequestedTeams.Count() + " matches");

            //List<LccCalculatedMatchupInformation> matchesToReturnToUser = new List<LccCalculatedMatchupInformation>();

            //if (allMatchesWithRequestedTeams.Any())
            //{
            //    int matchReturnCount = 0;

            //    allMatchesWithRequestedTeams = allMatchesWithRequestedTeams.OrderByDescending(x => x.MatchDate);

            //    foreach (var match in allMatchesWithRequestedTeams)
            //    {
            //        if (matchReturnCount == maxMatchLimit)
            //            break;

            //        try
            //        {
            //            Console.WriteLine(DateTime.Now + ": Looking in database cache for match");
            //            Db_LccCachedCalculatedMatchupInfo cachedMatchInfo = _cachedCalculatedMatchupInformaton.GetCalculatedMatchupInfoByGameId(match.GameId);
            //            Console.WriteLine(DateTime.Now + ": Finished Looking in database cache for match");

            //            if (cachedMatchInfo == null)
            //            {
            //                Console.WriteLine(DateTime.Now + ": Didn't find it");
            //                Console.WriteLine(DateTime.Now + ": retrieving match from riot");
            //                Match riotMatchInformation = await _riotApi.Match.GetMatchAsync(Region.euw, match.GameId);
            //                Console.WriteLine(DateTime.Now + ": retrieved match from riot");

            //                Console.WriteLine(DateTime.Now + ": retrieving match timeline from riot");
            //                Timeline timeline = await _riotApi.Match.GetMatchTimelineAsync(Region.euw, match.GameId);
            //                Console.WriteLine(DateTime.Now + ": retrieved match timeline from riot");

            //                Console.WriteLine(DateTime.Now + ": Creating database cache version from this match");
            //                Db_LccCachedCalculatedMatchupInfo newCachedMatchInfo = await _matchControllerUtils.CreateDatabaseModelForCalculatedMatchupInfo(riotMatchInformation, timeline, usersChampionId);
            //                Console.WriteLine(DateTime.Now + ": Finished creating database cache version from this match");

            //                //Add to persistant storage
            //                Console.WriteLine(DateTime.Now + ": Saving this match to the database");
            //                _cachedCalculatedMatchupInformaton.InsertCalculatedMatchupInfo(newCachedMatchInfo);
            //                _cachedCalculatedMatchupInformaton.Save();
            //                Console.WriteLine(DateTime.Now + ": Saved this match to the database");

            //                cachedMatchInfo = newCachedMatchInfo;
            //            }
            //            else
            //            {
            //                Console.WriteLine(DateTime.Now + " Match was in the cache");
            //            }

            //            Console.WriteLine(DateTime.Now + ": Creating Lcc version of cached match");
            //            matchesToReturnToUser.Add(_matchControllerUtils.CreateLccCalculatedMatchupInformationFromCache(cachedMatchInfo));
            //            Console.WriteLine(DateTime.Now + ": Finished creating Lcc version of cached match");

            //            matchReturnCount++;
            //        }
            //        catch(Exception e)
            //        {
            //            Console.WriteLine("Exception encountered when creating match :" + e.Message);
            //        }
            //    }
            //}

            return new JsonResult("");
        }
    }
}