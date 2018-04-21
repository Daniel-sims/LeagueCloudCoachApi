using System;
using LccWebAPI.Controllers.Utils.Match;
using LccWebAPI.Database.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

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
            Console.WriteLine(DateTime.Now + " match request received");

            var response = new JsonResult(_matchProvider.GetMatchesForListOfTeamIds(teamOneChampionIds, teamTwoChampionIds, maxMatchLimit));

            Console.WriteLine(DateTime.Now + " match request returned.");

            return response;
        }

        [HttpGet("MatchTimeline")]
        public JsonResult GetMatchTimeline(long gameId)
        {
            Console.WriteLine(DateTime.Now + " MatchTimeline request received");

            var timeline = _databaseContext.MatchTimelines
                    .Include(x => x.Events)
                    .FirstOrDefault(x => x.GameId == gameId);

            Console.WriteLine(DateTime.Now + " MatchTimeline request responded");

            return new JsonResult(timeline);
        }
    }
}