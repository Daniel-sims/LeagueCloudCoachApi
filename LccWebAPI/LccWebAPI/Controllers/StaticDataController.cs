using Microsoft.AspNetCore.Mvc;
using RiotSharp.Endpoints.Interfaces.Static;

namespace LccWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class StaticDataController : Controller
    {
        private IStaticDataEndpoints _staticDataEndpoints;
        public StaticDataController(IStaticDataEndpoints staticDataEndpoints
           )
        {
            _staticDataEndpoints = staticDataEndpoints;
        }

        [HttpGet("GetAllChampionsData")]
        public JsonResult GetAllChampionsData()
        {
            //List<LccChampionInformation> championInformationList = new List<LccChampionInformation>();

            //foreach (Db_LccChampion dbChampion in _championStaticDataRepository.GetAllChampions().ToList())
            //{
            //    championInformationList.Add(new LccChampionInformation()
            //    {
            //        ChampionId = dbChampion.ChampionId,
            //        ChampionName = dbChampion.ChampionName,
            //        ImageFull = dbChampion.ImageFull
            //    });
            //}

            return new JsonResult("");
        }

        [HttpGet("GetAllItemData")]
        public JsonResult GetAllItemData()
        {
            //IList<LccItemInformation> itemInformationList = new List<LccItemInformation>();

            //foreach (Db_LccItem dbItem in _itemStaticDataRepository.GetAllItems().ToList())
            //{
            //    itemInformationList.Add(new LccItemInformation()
            //    {
            //        ItemId = dbItem.ItemId,
            //        ItemName = dbItem.ItemName,
            //        ImageFull = dbItem.ImageFull
            //    });
            //}

            return new JsonResult("");
        }

        [HttpGet("GetAllSummonerSpellsData")]
        public JsonResult GetAllSummonerSpellData()
        {
            //List<LccSummonerSpellInformation> summonerSpellInformationList = new List<LccSummonerSpellInformation>();

            //foreach (Db_LccSummonerSpell dbSummonerSpell in _summonerSpellStaticDataRepository.GetAllSummonerSpells().ToList())
            //{
            //    summonerSpellInformationList.Add(new LccSummonerSpellInformation()
            //    {
            //        SummonerSpellId = dbSummonerSpell.SummonerSpellId,
            //        SummonerSpellName = dbSummonerSpell.SummonerSpellName,
            //        ImageFull = dbSummonerSpell.ImageFull
            //    });
            //}

            return new JsonResult("");
        }

        [HttpGet("GetAllRunesData")]
        public JsonResult GetAllRunesData()
        {
            //List<LccRuneInformation> runeInformationList = new List<LccRuneInformation>();

            //foreach (Db_LccRune dbRune in _runesStaticDataRepository.GetAllRunes().ToList())
            //{
            //    runeInformationList.Add(new LccRuneInformation()
            //    {
            //        RuneId = dbRune.RuneId,
            //        RuneName = dbRune.RuneName,
            //        RunePathName = dbRune.RunePathName,
            //        Key = dbRune.Key,
            //        ShortDesc = dbRune.ShortDesc,
            //        LongDesc = dbRune.LongDesc,
            //        Icon = dbRune.Icon
            //    });
            //}

            return new JsonResult("");
        }

    }
}