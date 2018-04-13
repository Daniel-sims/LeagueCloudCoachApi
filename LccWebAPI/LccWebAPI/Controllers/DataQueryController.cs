using System.Collections;
using System.Collections.Generic;
using LccWebAPI.Database.Context;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace LccWebAPI.Controllers
{
    /*
     * Test controller to retrieve database stats
     */
    [Route("[controller]")]
    public class DataQueryController : Controller
    {
        private readonly DatabaseContext _databaseContext;

        public DataQueryController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        [HttpGet("AllDataNumbers")]
        public JsonResult GetAllDataNumbers()
        {
            var dataStats = new DataStats
            {
                Matches = _databaseContext.Matches.Count(),
                MatchTimelines = _databaseContext.MatchTimelines.Count(),
                Summoners = _databaseContext.Summoners.Count(),

                Items = _databaseContext.Items.Count(),
                Runes = _databaseContext.Runes.Count(),
                SummonerSpells = _databaseContext.SummonerSpells.Count(),
                Champions = _databaseContext.Champions.Count()
            };

            return new JsonResult(dataStats);
        }

        [HttpGet("TestMatchIds")]
        public JsonResult GetTestMatchData()
        {
            var testTeamIds = new TestTeamIds();

            var match = _databaseContext.Matches.Include(x => x.Teams).ThenInclude(x => x.Players).FirstOrDefault();

            var teamOne = match.Teams.First(x => x.TeamId == 100);
            foreach (var player in teamOne.Players)
            {
                testTeamIds.TeamOneIds.Add(player.ChampionId);
            }

            var teamTwo = match.Teams.First(x => x.TeamId == 200);
            foreach (var player in teamTwo.Players)
            {
                testTeamIds.TeamTwoIds.Add(player.ChampionId);
            }

            return new JsonResult(testTeamIds);
        }
    }

    public class DataStats
    {
        public int Matches { get; set; }
        public int MatchTimelines { get; set; }
        public int Summoners { get; set; }

        public int Items { get; set; }
        public int Runes { get; set; }
        public int Champions { get; set; }
        public int SummonerSpells { get; set; }
    }

    public class TestTeamIds
    {
        public IList<int> TeamOneIds { get; set; } = new List<int>();
        public IList<int> TeamTwoIds { get; set; } = new List<int>();
    }
}