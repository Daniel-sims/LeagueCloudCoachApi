using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LccWebAPI.Constants;
using LccWebAPI.Controllers.Models.Summoner;
using Microsoft.AspNetCore.Mvc;
using RiotSharp.Endpoints.LeagueEndpoint;
using RiotSharp.Endpoints.SummonerEndpoint;
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

        [HttpGet("SummonerByName/{summonerName}")]
        public async Task<JsonResult> GetSummonerByName(string summonerName)
        {
            Summoner summoner = await _riotApi.Summoner.GetSummonerByNameAsync(RiotSharp.Misc.Region.euw, summonerName);
            List<LeaguePosition> leaguePositions = await _riotApi.League.GetLeaguePositionsAsync(RiotSharp.Misc.Region.euw, summoner.Id);

            LeaguePosition rankedSolo = leaguePositions.FirstOrDefault(x => x.QueueType == LeagueQueue.RankedSolo);

            LccSummoner lccSummoner = new LccSummoner()
            {
                SummonerName = summoner.Name,
                SummonerLevel = summoner.Level,
                RankedSoloWins = rankedSolo.Wins,
                RankedSoloLosses = rankedSolo.Losses,
                RankedSoloDivision = rankedSolo.Rank,
                RankedSoloLeaguePoints = rankedSolo.LeaguePoints.ToString(),
                RankedSoloTier = rankedSolo.Tier
            };
            
            return new JsonResult(lccSummoner);
        }


    }
}