using LccWebAPI.Repository.StaticData.Interfaces;
using Microsoft.AspNetCore.Mvc;
using RiotSharp.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LccWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class StaticDataController : Controller
    {
        private IRiotApi _riotApi;
        private IChampionStaticDataRepository _championStaticDataRepository;
        private IItemStaticDataRepository _itemStaticDataRepository;
        private ISummonerSpellStaticDataRepository _summonerSpellStaticDataRepository;
        private IRunesStaticDataRepository _runesStaticDataRepository;
        
        public StaticDataController(IRiotApi riotApi,
            IChampionStaticDataRepository championStaticDataRepository, 
            IItemStaticDataRepository itemStaticDataRepository, 
            ISummonerSpellStaticDataRepository summonerSpellStaticDataRepository,
            IRunesStaticDataRepository runesStaticDataRepository)
        {
            _riotApi = riotApi;
            _championStaticDataRepository = championStaticDataRepository;
            _itemStaticDataRepository = itemStaticDataRepository;
            _summonerSpellStaticDataRepository = summonerSpellStaticDataRepository;
            _runesStaticDataRepository = runesStaticDataRepository;
        }
        
        //[HttpGet("GetAllChampionsData")]
        //public async Task<JsonResult> GetAllChampionsData()
        //{
        //    List<LccChampionInformation> championInformation = null;

        //    return new JsonResult(championInformation);
        //}
        
        //[HttpGet("GetAllItemData")]
        //public async Task<JsonResult> GetAllItemData()
        //{
        //    List<LccItemInformation> lccItemInformation = null;

        //    return new JsonResult(lccItemInformation);
        //}

        //[HttpGet("GetAllSummonerSpellData")]
        //public async Task<JsonResult> GetAllSummonerSpellData()
        //{
        //    List<LccSummonerSpellInformation> lccSummonerSpellInformation = null;

        //    return new JsonResult(lccSummonerSpellInformation);
        //}

        //[HttpGet("GetAllRunesReforgedData")]
        //public async Task<JsonResult> GetAllRunesReforgedData()
        //{
        //    List<LccRuneReforged> lccSummonerSpellInformation = null;

        //    return new JsonResult(lccSummonerSpellInformation);
        //}

    }
}