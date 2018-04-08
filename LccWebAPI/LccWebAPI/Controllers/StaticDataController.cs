using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LccWebAPI.Controllers.Utils.Match;
using LccWebAPI.Database.Context;
using Microsoft.AspNetCore.Mvc;

namespace LccWebAPI.Controllers
{
    [Route("[controller]")]
    public class StaticDataController
    {
        private readonly DatabaseContext _dbContext;

        public StaticDataController(
            DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("Items")]
        public JsonResult GetItems()
        {
            return new JsonResult(_dbContext.Items);
        }

        [HttpGet("Runes")]
        public JsonResult GetRunes()
        {
            return new JsonResult(_dbContext.Runes);
        }

        [HttpGet("SummonerSpells")]
        public JsonResult GetSummonerSpells()
        {
            return new JsonResult(_dbContext.SummonerSpells);
        }

        [HttpGet("Champions")]
        public JsonResult GetChampions()
        {
            return new JsonResult(_dbContext.Champions);
        }
    }
}
