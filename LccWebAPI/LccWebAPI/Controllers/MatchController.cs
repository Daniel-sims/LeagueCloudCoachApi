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
        public async Task<JsonResult> GetMatchup(long usersChampionId, string usersLane, long[] friendlyTeamChampions, long[] enemyTeamChampions, int maxMatchLimit = 5)
        {
            IList<Db_LccBasicMatchInfo> allMatchesInDatabase = _matchupInformationRepository.GetAllMatchups().ToList();
            IList<long> friendlyTeamChampionIds = new List<long>(friendlyTeamChampions) { usersChampionId };
            IList<long> enemyTeamChampionIds = enemyTeamChampions.ToList();

            IList<Db_LccBasicMatchInfo> allMatchesContainingUsersChampion = allMatchesInDatabase.Where
                (x => x.LosingTeamChampions.Any(p => p.ChampionId == usersChampionId || x.WinningTeamChampions.Any(u => u.ChampionId == usersChampionId))).ToList();

            IList<Db_LccBasicMatchInfo> allMatchesWithRequestedTeams =
                allMatchesContainingUsersChampion.Where
                //Check to see if the Enemys team Ids are the losing team, and the friendly team are the winning team
                (q => (enemyTeamChampionIds.All(e => q.LosingTeamChampions.Any(l => l.ChampionId == e))
                && friendlyTeamChampionIds.All(f => q.WinningTeamChampions.Any(l => l.ChampionId == f)))
                //Check to see if the winning team Ids are the losing team, and the enemy team are the winning team
                || (enemyTeamChampionIds.All(e => q.WinningTeamChampions.Any(l => l.ChampionId == e))
                && friendlyTeamChampionIds.All(f => q.LosingTeamChampions.Any(l => l.ChampionId == f)))).ToList();

            List<LccCalculatedMatchupInformation> matchesToReturnToUser = new List<LccCalculatedMatchupInformation>();

            if (allMatchesWithRequestedTeams.Any())
            {
                int matchReturnCount = 0;

                foreach (var match in allMatchesWithRequestedTeams.OrderByDescending(x => x.MatchDate))
                {
                    if (matchReturnCount == maxMatchLimit)
                        break;

                    try
                    {
                        Db_LccCachedCalculatedMatchupInfo cachedMatchInfo = _cachedCalculatedMatchupInformaton.GetCalculatedMatchupInfoByGameId(match.GameId);

                        if (cachedMatchInfo == null)
                        {
                            Match riotMatchInformation = await _riotApi.Match.GetMatchAsync(Region.euw, match.GameId);
                            Timeline timeline = await _riotApi.Match.GetMatchTimelineAsync(Region.euw, match.GameId);

                            Db_LccCachedCalculatedMatchupInfo newCachedMatchInfo = await _matchControllerUtils.CreateDatabaseModelForCalculatedMatchupInfo(riotMatchInformation, timeline, usersChampionId);

                            _cachedCalculatedMatchupInformaton.InsertCalculatedMatchupInfo(newCachedMatchInfo);
                            _cachedCalculatedMatchupInformaton.Save();

                            cachedMatchInfo = newCachedMatchInfo;
                        }

                        matchesToReturnToUser.Add(_matchControllerUtils.CreateLccCalculatedMatchupInformationFromCache(cachedMatchInfo));

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