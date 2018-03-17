using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LccWebAPI.Models.DatabaseModels;
using LccWebAPI.Repository.Match;
using Microsoft.AspNetCore.Mvc;
using RiotSharp.Interfaces;
using RiotSharp.MatchEndpoint;

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
        public JsonResult GetMatchup(long usersChampionId, string usersLane, long[] friendlyTeamChampionIds, long[] enemyTeamChampionIds)
        {
            var allMatchesInDatabase = _matchupInformationRepository.GetAllMatchupInformations();
            var matchContainingUsersChampionAndLane 
                = allMatchesInDatabase.Where(x => x.LosingTeam.Any(p => p.ChampionId == usersChampionId) || x.WinningTeam.Any(u => u.ChampionId == usersChampionId)).ToList();
            
            List<Match> matches = new List<Match>();
            
            foreach(var match in matchContainingUsersChampionAndLane)
            {
                List<LccMatchupInformationPlayer> players = match.LosingTeam.Concat(match.WinningTeam).ToList();

                bool isUsersTeamWinning = match.LosingTeam.Concat(match.WinningTeam).Any(x => x.ChampionId == usersChampionId);

                if(isUsersTeamWinning)
                {
                    //if(match.WinningTeam.Contains(friendlyTeamChampionIds))
                    //{

                    //}
                    //losing team needs to match enemyTeamChampionIds
                    //winning team needs to match friendlyTeamChampionIds
                }
                else
                {
                    //winning team needs to match enemyTeamChampionIds
                    //losing team needs to match friendlyTeamChampionIds
                }
                
            }
            
            matches.Add(new Match());
            return new JsonResult(matches);
        }
    }
}