using LccWebAPI.Controllers.Utils.Match;
using LccWebAPI.Database.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Logging;

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
        public JsonResult GetMatchTimeline(long gameId)
        {
            var timeline = _databaseContext.MatchTimelines
                    .Include(x => x.Events)
                    .FirstOrDefault(x => x.GameId == gameId);
            
            return new JsonResult(timeline);
        }
    }
}