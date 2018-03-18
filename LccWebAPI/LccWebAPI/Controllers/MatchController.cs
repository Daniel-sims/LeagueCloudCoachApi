using LccWebAPI.Models.DatabaseModels;
using LccWebAPI.Repository.Match;
using Microsoft.AspNetCore.Mvc;
using RiotSharp.Interfaces;
using RiotSharp.MatchEndpoint;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<JsonResult> GetMatchup(long usersChampionId, string usersLane, long[] friendlyTeamChampions, long[] enemyTeamChampions, int maxMatchLimit = 5)
        {
            var allMatchesInDatabase = _matchupInformationRepository.GetAllMatchupInformations();

            // All matches with the users champion in, losing or none losing team
            var matchesContainingUsersChampionAndLane 
                = allMatchesInDatabase
                .Where(x => x.LosingTeam.Any(p => p.ChampionId == usersChampionId && p.Lane.ToLower() == usersLane.ToLower() 
                || x.WinningTeam.Any(u => u.ChampionId == usersChampionId && u.Lane.ToLower() == usersLane.ToLower()))).ToList();

            // So we don't care about which teams winning or losing
           
            // Get the id's in lists
            IList<long> friendlyTeamChampionIds = new List<long>(friendlyTeamChampions) { usersChampionId };
            IList<long> enemyTeamChampionIds = enemyTeamChampions.ToList();

            var compMatches = matchesContainingUsersChampionAndLane.Where(q =>
            (enemyTeamChampionIds.All(e => q.LosingTeam.Any(l => l.ChampionId == e)) && friendlyTeamChampionIds.All(f => q.WinningTeam.Any(l => l.ChampionId == f)))
            || (enemyTeamChampionIds.All(e => q.WinningTeam.Any(l => l.ChampionId == e)) && friendlyTeamChampionIds.All(f => q.LosingTeam.Any(l => l.ChampionId == f))));

            List<Match> matchesToReturnToUser = new List<Match>();

            if (compMatches.Any())
            {
                int matchReturnCount = 0;

                foreach(var match in compMatches)
                {
                    if(matchReturnCount <= maxMatchLimit)
                    {
                        var matchToReturn = await _riotApi.GetMatchAsync(RiotSharp.Misc.Region.euw, match.GameId);
                        matchesToReturnToUser.Add(matchToReturn);

                        matchReturnCount++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            
            return new JsonResult(matchesToReturnToUser);
        }
    }
}