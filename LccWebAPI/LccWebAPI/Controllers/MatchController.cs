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
            IList<long> friendlyTeamChampionIds = new List<long>(friendlyTeamChampions) { usersChampionId };
            List<long> enemyTeamChampionIds = enemyTeamChampions.ToList();

            foreach(LccMatchupInformation match in matchesContainingUsersChampionAndLane)
            {
                IList<long> listOfWinningTeamIds = match.WinningTeam.Select(x => x.ChampionId).ToList();
                IList<long> listOfLosingTeamIds = match.LosingTeam.Select(x => x.ChampionId).ToList();
                
                if(listOfWinningTeamIds.Contains(friendlyTeamChampionIds) && listOfLosingTeamIds.Contains(enemyTeamChampionIds) ||
                    listOfWinningTeamIds.Contains(enemyTeamChampionIds) && listOfLosingTeamIds.Contains(friendlyTeamChampionIds)
                    )
                {

                    // here
                }

            }
           
            //matches to return
            List<Match> matches = new List<Match>();
            return new JsonResult(matches);
        }
    }
}