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
        public async Task<JsonResult> GetAllChampionsData()
        {
            IList<Db_LccChampion> championsInDatabase = _championStaticDataRepository.GetAllChampions().ToList();

            if (championsInDatabase.Count() == 0)
            {
                ChampionListStatic championsListFromRiot = await _staticDataEndpoints.Champion.GetChampionsAsync(Region.euw);

                foreach(ChampionStatic champion in championsListFromRiot.Champions.Values)
                {
                    _championStaticDataRepository.InsertChampionInformation(new Db_LccChampion()
                    {
                        ChampionId = champion.Id,
                        ChampionName = champion.Name,
                        ImageFull = champion.Image.Full
                    });
                }

                _championStaticDataRepository.Save();
            }

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
        public async Task<JsonResult> GetAllItemData()
        {
            IList<Db_LccItem> itemsInDatabase = _itemStaticDataRepository.GetAllItems().ToList();

            if (itemsInDatabase.Count() == 0)
            {
                ItemListStatic itemsListFromRiot = await _staticDataEndpoints.Item.GetItemsAsync(Region.euw);

                foreach (ItemStatic item in itemsListFromRiot.Items.Values)
                {
                    _itemStaticDataRepository.InsertItem(new Db_LccItem()
                    {
                        ItemId = item.Id,
                        ItemName = item.Name
                    });
                }

                _itemStaticDataRepository.Save();
            }

            IList<LccItemInformation> itemInformationList = new List<LccItemInformation>();

            foreach (Db_LccItem dbItem in _itemStaticDataRepository.GetAllItems().ToList())
            {
                itemInformationList.Add(new LccItemInformation()
                {
                    ItemId = dbItem.ItemId,
                    ItemName = dbItem.ItemName
                });
            }

            return new JsonResult(itemInformationList);
        }

        [HttpGet("GetAllSummonerSpellsData")]
        public async Task<JsonResult> GetAllSummonerSpellData()
        {
            IList<Db_LccSummonerSpell> lccSummonerSpellInformation = _summonerSpellStaticDataRepository.GetAllSummonerSpells().ToList();

            if (lccSummonerSpellInformation.Count() == 0)
            {
                SummonerSpellListStatic summonerSpellListFromRiot = await _staticDataEndpoints.SummonerSpell.GetSummonerSpellsAsync(Region.euw);

                foreach (SummonerSpellStatic summoner in summonerSpellListFromRiot.SummonerSpells.Values)
                {
                    _summonerSpellStaticDataRepository.InsertSummonerSpell(new Db_LccSummonerSpell()
                    {
                        SummonerSpellId = summoner.Id,
                        SummonerSpellName = summoner.Name
                    });
                }

                _summonerSpellStaticDataRepository.Save();
            }

            List<LccSummonerSpellInformation> summonerSpellInformationList = new List<LccSummonerSpellInformation>();

            foreach (Db_LccSummonerSpell dbSummonerSpell in _summonerSpellStaticDataRepository.GetAllSummonerSpells().ToList())
            {
                summonerSpellInformationList.Add(new LccSummonerSpellInformation()
                {
                    SummonerSpellId = dbSummonerSpell.SummonerSpellId,
                    SummonerSpellName = dbSummonerSpell.SummonerSpellName
                });

            }

            return new JsonResult(lccSummonerSpellInformation);
        }

        [HttpGet("GetAllRunesData")]
        public async Task<JsonResult> GetAllRunesData()
        {
            IList<Db_LccRune> lccRuneInformation = _runesStaticDataRepository.GetAllRunes().ToList();

            if (lccRuneInformation.Count() == 0)
            {
                IList<RuneReforged> runeListFromRiot = await _staticDataEndpoints.Rune.GetRunesReforgedAsync(Region.euw);

                foreach (RuneReforged rune in runeListFromRiot)
                {
                    _runesStaticDataRepository.InsertRune(new Db_LccRune()
                    {
                        RuneId = rune.Id,
                        RuneName = rune.Name,
                        RunePathName = rune.RunePathName,
                        Key = rune.Key,
                        ShortDesc = rune.ShortDesc,
                        LongDesc = rune.LongDesc,
                        Icon = rune.Icon
                       
                    });
                }

                _runesStaticDataRepository.Save();
            }

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