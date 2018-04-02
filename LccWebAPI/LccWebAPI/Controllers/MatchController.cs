using LccWebAPI.Constants;
using LccWebAPI.Controllers.Models.Match;
using LccWebAPI.Controllers.Models.StaticData;
using LccWebAPI.Controllers.Utils.Match;
using LccWebAPI.Database.Models.Match;
using LccWebAPI.Database.Models.StaticData;
using LccWebAPI.Database.Repository.Interfaces.Match;
using LccWebAPI.Repository.Interfaces.Match;
using LccWebAPI.Repository.Interfaces.StaticData;
using Microsoft.AspNetCore.Mvc;
using RiotSharp.Endpoints.LeagueEndpoint;
using RiotSharp.Endpoints.MatchEndpoint;
using RiotSharp.Interfaces;
using RiotSharp.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class MatchController : Controller
    {
        private IRiotApi _riotApi;

        private readonly IBasicMatchupInformationRepository _matchupInformationRepository;
        private readonly ICachedCalculatedMatchupInformationRepository _cachedCalculatedMatchupInformaton;

        private readonly IMatchControllerUtils _matchControllerUtils;

        private static IList<Db_LccCachedCalculatedMatchupInfo> _cachedMatchInfo;

        public MatchController(IRiotApi riotApi,
            IBasicMatchupInformationRepository matchupInformationRepository,
            ICachedCalculatedMatchupInformationRepository cachedCalculatedMatchupInformaton,
            IMatchControllerUtils matchControllerUtils)
        {
            _riotApi = riotApi;

            _matchupInformationRepository = matchupInformationRepository;
            _cachedCalculatedMatchupInformaton = cachedCalculatedMatchupInformaton;

            _matchControllerUtils = matchControllerUtils;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetMatchup")]
        public async Task<JsonResult> GetMatchup(long usersChampionId, string usersLane, long[] friendlyTeamChampions, long[] enemyTeamChampions, int maxMatchLimit = 5, string summonerName = "BOLULU")
        {
            IEnumerable<Db_LccBasicMatchInfo> allMatchesInDatabase = _matchupInformationRepository.GetAllMatchups();
            IList<long> friendlyTeamChampionIds = new List<long>(friendlyTeamChampions) { usersChampionId };
            IList<long> enemyTeamChampionIds = enemyTeamChampions;

            IEnumerable<Db_LccBasicMatchInfo> allMatchesContainingUsersChampion = allMatchesInDatabase.Where
                (x => x.LosingTeamChampions.Any(p => p.ChampionId == usersChampionId || x.WinningTeamChampions.Any(u => u.ChampionId == usersChampionId)));

            IEnumerable<Db_LccBasicMatchInfo> allMatchesWithRequestedTeams = new List<Db_LccBasicMatchInfo>();
            //if a summoner name parameter is passed filter based on this too
            Console.WriteLine(DateTime.Now + ": Finding matches for requested matchup");
            if (summonerName != "")
            {
                allMatchesWithRequestedTeams =
                allMatchesContainingUsersChampion.Where
                //Check to see if the Enemys team Ids are the losing team, and the friendly team are the winning team
                (q => (enemyTeamChampionIds.All(e => q.LosingTeamChampions.Any(l => l.ChampionId == e && l.SummonerName == summonerName))
                && friendlyTeamChampionIds.All(f => q.WinningTeamChampions.Any(l => l.ChampionId == f && l.SummonerName == summonerName)))
                //Check to see if the winning team Ids are the losing team, and the enemy team are the winning team
                || (enemyTeamChampionIds.All(e => q.WinningTeamChampions.Any(l => l.ChampionId == e && l.SummonerName == summonerName))
                && friendlyTeamChampionIds.All(f => q.LosingTeamChampions.Any(l => l.ChampionId == f && l.SummonerName == summonerName))));
            }
            else
            {
                allMatchesContainingUsersChampion.Where
               //Check to see if the Enemys team Ids are the losing team, and the friendly team are the winning team
               (q => (enemyTeamChampionIds.All(e => q.LosingTeamChampions.Any(l => l.ChampionId == e))
               && friendlyTeamChampionIds.All(f => q.WinningTeamChampions.Any(l => l.ChampionId == f)))
               //Check to see if the winning team Ids are the losing team, and the enemy team are the winning team
               || (enemyTeamChampionIds.All(e => q.WinningTeamChampions.Any(l => l.ChampionId == e))
               && friendlyTeamChampionIds.All(f => q.LosingTeamChampions.Any(l => l.ChampionId == f))));
            }
            Console.WriteLine(DateTime.Now + ": Found - " + allMatchesWithRequestedTeams.Count() + " matches");

            List<LccCalculatedMatchupInformation> matchesToReturnToUser = new List<LccCalculatedMatchupInformation>();

            if (allMatchesWithRequestedTeams.Any())
            {
                int matchReturnCount = 0;

                allMatchesWithRequestedTeams = allMatchesWithRequestedTeams.OrderByDescending(x => x.MatchDate);

                foreach (var match in allMatchesWithRequestedTeams)
                {
                    if (matchReturnCount == maxMatchLimit)
                        break;

                    try
                    {
                        Console.WriteLine(DateTime.Now + ": Looking in database cache for match");
                        Db_LccCachedCalculatedMatchupInfo cachedMatchInfo = _cachedCalculatedMatchupInformaton.GetCalculatedMatchupInfoByGameId(match.GameId);
                        Console.WriteLine(DateTime.Now + ": Finished Looking in database cache for match");

                        if (cachedMatchInfo == null)
                        {
                            Console.WriteLine(DateTime.Now + ": Didn't find it");
                            Console.WriteLine(DateTime.Now + ": retrieving match from riot");
                            Match riotMatchInformation = await _riotApi.Match.GetMatchAsync(Region.euw, match.GameId);
                            Console.WriteLine(DateTime.Now + ": retrieved match from riot");

                            Console.WriteLine(DateTime.Now + ": retrieving match timeline from riot");
                            Timeline timeline = await _riotApi.Match.GetMatchTimelineAsync(Region.euw, match.GameId);
                            Console.WriteLine(DateTime.Now + ": retrieved match timeline from riot");

                            Console.WriteLine(DateTime.Now + ": Creating database cache version from this match");
                            Db_LccCachedCalculatedMatchupInfo newCachedMatchInfo = await _matchControllerUtils.CreateDatabaseModelForCalculatedMatchupInfo(riotMatchInformation, timeline, usersChampionId);
                            Console.WriteLine(DateTime.Now + ": Finished creating database cache version from this match");

                            //Add to persistant storage
                            Console.WriteLine(DateTime.Now + ": Saving this match to the database");
                            _cachedCalculatedMatchupInformaton.InsertCalculatedMatchupInfo(newCachedMatchInfo);
                            _cachedCalculatedMatchupInformaton.Save();
                            Console.WriteLine(DateTime.Now + ": Saved this match to the database");

                            cachedMatchInfo = newCachedMatchInfo;
                        }
                        else
                        {
                            Console.WriteLine(DateTime.Now + " Match was in the cache");
                        }

                        Console.WriteLine(DateTime.Now + ": Creating Lcc version of cached match");
                        matchesToReturnToUser.Add(_matchControllerUtils.CreateLccCalculatedMatchupInformationFromCache(cachedMatchInfo));
                        Console.WriteLine(DateTime.Now + ": Finished creating Lcc version of cached match");

                        matchReturnCount++;
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("Exception encountered when creating match :" + e.Message);
                    }
                }
            }
            
            return new JsonResult(matchesToReturnToUser);
        }
    }
}