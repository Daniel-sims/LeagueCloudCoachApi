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
            IList<long> friendlyTeamChampionIds = new List<long>(friendlyTeamChampions) { usersChampionId };
            IList<long> enemyTeamChampionIds = enemyTeamChampions.ToList();


            // This is looks horrible but works....
            // first .Where finds if users champion + lane is on winning team or losing team
            // second .Where checks to see if all of the specified champions are on either team, winning or losing
            // Basically it gets the matches the user specified...
            var matchesContainingUsersChampionAndLane 
                = allMatchesInDatabase
                .Where(x => x.LosingTeam.Any(p => p.ChampionId == usersChampionId && p.Lane.ToLower() == usersLane.ToLower() 
                || x.WinningTeam.Any(u => u.ChampionId == usersChampionId && u.Lane.ToLower() == usersLane.ToLower()))).ToList()
                .Where(q => (enemyTeamChampionIds.All(e => q.LosingTeam.Any(l => l.ChampionId == e)) && friendlyTeamChampionIds.All(f => q.WinningTeam.Any(l => l.ChampionId == f)))
                || (enemyTeamChampionIds.All(e => q.WinningTeam.Any(l => l.ChampionId == e)) && friendlyTeamChampionIds.All(f => q.LosingTeam.Any(l => l.ChampionId == f))));

            List<Match> matchesToReturnToUser = new List<Match>();

            if (matchesContainingUsersChampionAndLane.Any())
            {
                int matchReturnCount = 0;

                foreach(var match in matchesContainingUsersChampionAndLane)
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