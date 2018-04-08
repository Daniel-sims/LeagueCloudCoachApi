using LccWebAPI.Controllers.Utils.Match;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using LccWebAPI.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace LccWebAPI.Controllers
{
    [Route("[controller]")]
    public class MatchController : Controller
    {
        private readonly IMatchProvider _matchProvider;
        private readonly DatabaseContext _databaseContext;

        public MatchController(IMatchProvider matchProvider, DatabaseContext databaseContext)
        {
            _matchProvider = matchProvider;
            _databaseContext = databaseContext;
        }

        [HttpGet("Matchup")]
        public JsonResult GetMatchups(int[] teamOneChampionIds, int[] teamTwoChampionIds, int maxMatchLimit = 1)
        {
            return new JsonResult(_matchProvider.GetMatchesForListOfTeamIds(teamOneChampionIds, teamTwoChampionIds, maxMatchLimit));
        }

        [HttpGet("MatchTimeline")]
        public JsonResult GetMatchTimelines(long[] gameId)
        {
            var timelines = _databaseContext.MatchTimelines
                .Include(x => x.Events)
                .ToList()
                .Where(x => gameId.Contains(x.GameId));

            return new JsonResult(timelines);
        }
    }
}