using LccWebAPI.Controllers.Models.StaticData;
using LccWebAPI.Database.Models.StaticData;
using LccWebAPI.Repository.Interfaces.StaticData;
using Microsoft.AspNetCore.Mvc;
using RiotSharp.Endpoints.ChampionEndpoint;
using RiotSharp.Endpoints.Interfaces.Static;
using RiotSharp.Endpoints.StaticDataEndpoint.Champion;
using RiotSharp.Endpoints.StaticDataEndpoint.Item;
using RiotSharp.Endpoints.StaticDataEndpoint.ReforgedRune;
using RiotSharp.Endpoints.StaticDataEndpoint.Rune;
using RiotSharp.Endpoints.StaticDataEndpoint.SummonerSpell;
using RiotSharp.Interfaces;
using RiotSharp.Misc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class StaticDataController : Controller
    {
        private IStaticDataEndpoints _staticDataEndpoints;
        private IChampionStaticDataRepository _championStaticDataRepository;
        private IItemStaticDataRepository _itemStaticDataRepository;
        private ISummonerSpellStaticDataRepository _summonerSpellStaticDataRepository;
        private IRunesStaticDataRepository _runesStaticDataRepository;
        
        public StaticDataController(IStaticDataEndpoints staticDataEndpoints,
            IChampionStaticDataRepository championStaticDataRepository, 
            IItemStaticDataRepository itemStaticDataRepository, 
            ISummonerSpellStaticDataRepository summonerSpellStaticDataRepository,
            IRunesStaticDataRepository runesStaticDataRepository)
        {
            _staticDataEndpoints = staticDataEndpoints;
            _championStaticDataRepository = championStaticDataRepository;
            _itemStaticDataRepository = itemStaticDataRepository;
            _summonerSpellStaticDataRepository = summonerSpellStaticDataRepository;
            _runesStaticDataRepository = runesStaticDataRepository;
        }

        [HttpGet("GetAllChampionsData")]
        public JsonResult GetAllChampionsData()
        {
            List<LccChampionInformation> championInformationList = new List<LccChampionInformation>();

            foreach (Db_LccChampion dbChampion in _championStaticDataRepository.GetAllChampions().ToList())
            {
                championInformationList.Add(new LccChampionInformation()
                {
                    ChampionId = dbChampion.ChampionId,
                    ChampionName = dbChampion.ChampionName,
                    ImageFull = dbChampion.ImageFull
                });
            }

            return new JsonResult(championInformationList);
        }

        [HttpGet("GetAllItemData")]
        public JsonResult GetAllItemData()
        {
            IList<LccItemInformation> itemInformationList = new List<LccItemInformation>();

            foreach (Db_LccItem dbItem in _itemStaticDataRepository.GetAllItems().ToList())
            {
                itemInformationList.Add(new LccItemInformation()
                {
                    ItemId = dbItem.ItemId,
                    ItemName = dbItem.ItemName,
                    ImageFull = dbItem.ImageFull
                });
            }

            return new JsonResult(itemInformationList);
        }

        [HttpGet("GetAllSummonerSpellsData")]
        public JsonResult GetAllSummonerSpellData()
        {
            List<LccSummonerSpellInformation> summonerSpellInformationList = new List<LccSummonerSpellInformation>();

            foreach (Db_LccSummonerSpell dbSummonerSpell in _summonerSpellStaticDataRepository.GetAllSummonerSpells().ToList())
            {
                summonerSpellInformationList.Add(new LccSummonerSpellInformation()
                {
                    SummonerSpellId = dbSummonerSpell.SummonerSpellId,
                    SummonerSpellName = dbSummonerSpell.SummonerSpellName,
                    ImageFull = dbSummonerSpell.ImageFull
                });
            }

            return new JsonResult(summonerSpellInformationList);
        }

        [HttpGet("GetAllRunesData")]
        public JsonResult GetAllRunesData()
        {
            List<LccRuneInformation> runeInformationList = new List<LccRuneInformation>();

            foreach (Db_LccRune dbRune in _runesStaticDataRepository.GetAllRunes().ToList())
            {
                runeInformationList.Add(new LccRuneInformation()
                {
                    RuneId = dbRune.RuneId,
                    RuneName = dbRune.RuneName,
                    RunePathName = dbRune.RunePathName,
                    Key = dbRune.Key,
                    ShortDesc = dbRune.ShortDesc,
                    LongDesc = dbRune.LongDesc,
                    Icon = dbRune.Icon
                });
            }

            return new JsonResult(runeInformationList);
        }

    }
}