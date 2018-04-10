using LccWebAPI.Database.Context;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
}