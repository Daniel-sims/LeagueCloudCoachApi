using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RiotSharp.Interfaces;

namespace LccWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class SummonerController : Controller
    {
        private readonly IRiotApi _riotApi;

        public SummonerController(IRiotApi riotApi)
        {
            _riotApi = riotApi;
        }

        [HttpGet("Summoner/{summonerName}")]
        public async Task<JsonResult> GetSummonerByName(string summonerName)
        {
            var summoner = await _riotApi.GetSummonerByNameAsync(RiotSharp.Misc.Region.euw, summonerName);

            return new JsonResult(summoner);
        }


    }
}