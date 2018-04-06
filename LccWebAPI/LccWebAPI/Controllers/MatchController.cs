using LccWebAPI.Controllers.Utils.Match;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LccWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class MatchController : Controller
    {
        private readonly IMatchProvider _matchProvider;

        public MatchController(
            IMatchProvider matchProvider)
        {
            _matchProvider = matchProvider;
        }

        [HttpGet("GetMatchup")]
        public JsonResult GetMatchup(int usersChampionId, int[] friendlyTeamChampions, int[] enemyTeamChampions, int maxMatchLimit = 5)
        {
            var matchList = _matchProvider.GetMatchesForListOfTeamIds(usersChampionId, friendlyTeamChampions, enemyTeamChampions,
                maxMatchLimit);

            return new JsonResult(matchList);
        }
    }
}