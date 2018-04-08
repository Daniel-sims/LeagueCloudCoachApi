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
        public JsonResult GetMatchup(int[] friendlyTeamChampions, int[] enemyTeamChampions, int maxMatchLimit = 1)
        {
            var matchList = _matchProvider.GetMatchesForListOfTeamIds(friendlyTeamChampions, enemyTeamChampions, maxMatchLimit);

            return new JsonResult(matchList);
        }
    }
}