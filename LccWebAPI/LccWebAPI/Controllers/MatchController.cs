using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LccWebAPI.Repository.Match;
using Microsoft.AspNetCore.Mvc;
using RiotSharp.MatchEndpoint;

namespace LccWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class MatchController : Controller
    {
        IMatchupInformationRepository _matchupInformationRepository;

        public MatchController(IMatchupInformationRepository matchupInformationRepository)
        {
            _matchupInformationRepository = matchupInformationRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetMatchWithChampion")]
        public JsonResult GetMatchWithChampion(long id)
        {
            var fullMatches = _matchupInformationRepository.GetAllMatchupInformations();

            var matchWithSelectedChampion = fullMatches.FirstOrDefault(x => x.LosingTeam.Any(y => y.ChampionId == id) || x.WinningTeam.Any(y => y.ChampionId == id));

            return new JsonResult(matchWithSelectedChampion);

        }
    }
}