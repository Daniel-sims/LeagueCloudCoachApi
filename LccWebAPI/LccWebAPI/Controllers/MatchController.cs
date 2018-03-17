using LccWebAPI.Models.DatabaseModels;
using LccWebAPI.Repository.Match;
using Microsoft.AspNetCore.Mvc;
using RiotSharp.Interfaces;
using RiotSharp.MatchEndpoint;
using System.Collections.Generic;
using System.Linq;

namespace LccWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class MatchController : Controller
    {
        private IMatchupInformationRepository _matchupInformationRepository;
        private IRiotApi _riotApi;

        public MatchController(IMatchupInformationRepository matchupInformationRepository, IRiotApi riotApi)
        {
            _matchupInformationRepository = matchupInformationRepository;
            _riotApi = riotApi;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetMatchup")]
        public JsonResult GetMatchup(long usersChampionId, string usersLane, long[] friendlyTeamChampions, long[] enemyTeamChampions)
        {
            var allMatchesInDatabase = _matchupInformationRepository.GetAllMatchupInformations();

            // All matches with the users champion in, losing or none losing team
            var matchesContainingUsersChampionAndLane 
                = allMatchesInDatabase
                .Where(x => x.LosingTeam.Any(p => p.ChampionId == usersChampionId) || x.WinningTeam.Any(u => u.ChampionId == usersChampionId)).ToList();

            // So we don't care about which teams winning or losing
           
            // Get the id's in lists
            List<long> friendlyTeamChampionIds = new List<long>(friendlyTeamChampions) { usersChampionId };
            List<long> enemyTeamChampionIds = enemyTeamChampions.ToList();

            foreach(LccMatchupInformation match in matchesContainingUsersChampionAndLane)
            {
                /*
                 * if(winningTeam matches friendly team)
                 *      if(losing team matches enemy team)
                 *          // Correct match - users team has won 
                 * 
                 * if(losingTeam matches friendly team)
                 *      if(winning team matches friendly team)
                 *          // Correct match - users team has lost
                 */
            }
           
            //matches to return
            List<Match> matches = new List<Match>();
            return new JsonResult(matches);
        }
    }
}