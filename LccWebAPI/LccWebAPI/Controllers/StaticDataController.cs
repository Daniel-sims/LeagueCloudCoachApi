using LccWebAPI.Models.DatabaseModels;
using LccWebAPI.Repository.StaticData;
using Microsoft.AspNetCore.Mvc;
using RiotSharp.Endpoints.StaticDataEndpoint.Champion;
using RiotSharp.Endpoints.StaticDataEndpoint.Item;
using RiotSharp.Endpoints.StaticDataEndpoint.SummonerSpell;
using RiotSharp.Interfaces;
using System.Collections.Generic;
using System.Linq;
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

        private bool _getUpdatedChampionData = true;
        private bool _getUpdatedItemData = true;
        private bool _getUpdatedSummonerSpellData = true;

        public StaticDataController(IRiotApi riotApi,
            IChampionStaticDataRepository championStaticDataRepository, 
            IItemStaticDataRepository itemStaticDataRepository, 
            ISummonerSpellStaticDataRepository summonerSpellStaticDataRepository)
        {
            _riotApi = riotApi;
            _championStaticDataRepository = championStaticDataRepository;
            _itemStaticDataRepository = itemStaticDataRepository;
            _summonerSpellStaticDataRepository = summonerSpellStaticDataRepository;
        }
        
        [HttpGet("GetAllChampionsData")]
        public async Task<JsonResult> GetAllChampionsData()
        {
            if (_getUpdatedChampionData)
            {
                var currentChampions = _championStaticDataRepository.GetAllChampions();
                if (currentChampions.Count() == 0)
                {
                    ChampionListStatic championData = await _riotApi.Static.Champion.GetChampionsAsync(RiotSharp.Misc.Region.euw);
                    foreach (var champion in championData.Champions)
                    {
                        _championStaticDataRepository.InsertChampionInformation(new LccChampionInformation(champion.Value.Id, champion.Value.Name));
                    }

                    _championStaticDataRepository.Save();
                }
            }

            List<LccChampionInformation> championInformation = _championStaticDataRepository.GetAllChampions().ToList();

            return new JsonResult(championInformation);
        }
        
        [HttpGet("GetAllItemData")]
        public async Task<JsonResult> GetAllItemData()
        {
            if (_getUpdatedItemData)
            {
                var currentItems = _itemStaticDataRepository.GetAllItems();
                if(currentItems.Count() == 0)
                {
                    ItemListStatic itemData = await _riotApi.Static.Item.GetItemsAsync(RiotSharp.Misc.Region.euw);
                    foreach (var item in itemData.Items)
                    {
                        _itemStaticDataRepository.InsertItemInformation(new LccItemInformation(item.Key, item.Value.Name));
                    }

                    _itemStaticDataRepository.Save();
                }
            }

            List<LccItemInformation> lccItemInformation = _itemStaticDataRepository.GetAllItems().ToList();

            return new JsonResult(lccItemInformation);
        }

        [HttpGet("GetAllSummonerSpellData")]
        public async Task<JsonResult> GetAllSummonerSpellData()
        {
            if (_getUpdatedSummonerSpellData)
            {
                var currentSummonerSpellData = _summonerSpellStaticDataRepository.GetAllSummonerSpells();
                if(currentSummonerSpellData.Count() == 0)
                {
                    SummonerSpellListStatic summonerSpellData = await _riotApi.Static.SummonerSpell.GetSummonerSpellsAsync(RiotSharp.Misc.Region.euw);
                    foreach (var summonerSpell in summonerSpellData.SummonerSpells)
                    {
                        _summonerSpellStaticDataRepository.InsertSummonerSpellInformation(new LccSummonerSpellInformation(summonerSpell.Value.Id, summonerSpell.Value.Name));
                    }

                    _summonerSpellStaticDataRepository.Save();
                }
            }

            List<LccSummonerSpellInformation> lccSummonerSpellInformation = _summonerSpellStaticDataRepository.GetAllSummonerSpells().ToList();

            return new JsonResult(lccSummonerSpellInformation);
        }

    }
}