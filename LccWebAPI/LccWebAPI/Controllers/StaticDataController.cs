using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LccWebAPI.Models.DatabaseModels;
using LccWebAPI.Repository.StaticData;
using Microsoft.AspNetCore.Mvc;
using RiotSharp.Interfaces;
using RiotSharp.StaticDataEndpoint.Champion;
using RiotSharp.StaticDataEndpoint.Item;
using RiotSharp.StaticDataEndpoint.SummonerSpell;

namespace LccWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class StaticDataController : Controller
    {
        private IStaticRiotApi _staticRiotApi;
        private IChampionStaticDataRepository _championStaticDataRepository;
        private IItemStaticDataRepository _itemStaticDataRepository;
        private ISummonerSpellStaticDataRepository _summonerSpellStaticDataRepository;

        private bool _getUpdatedData = false;

        public StaticDataController(IStaticRiotApi staticRiotApi,
            IChampionStaticDataRepository championStaticDataRepository, 
            IItemStaticDataRepository itemStaticDataRepository, 
            ISummonerSpellStaticDataRepository summonerSpellStaticDataRepository)
        {
            _staticRiotApi = staticRiotApi;
            _championStaticDataRepository = championStaticDataRepository;
            _itemStaticDataRepository = itemStaticDataRepository;
            _summonerSpellStaticDataRepository = summonerSpellStaticDataRepository;
        }
        
        [HttpGet("GetAllChampionsData")]
        public async Task<JsonResult> GetAllChampionsData()
        {
            if (_getUpdatedData)
            {
                ChampionListStatic championData = await _staticRiotApi.GetChampionsAsync(RiotSharp.Misc.Region.euw);
                foreach (var champion in championData.Champions)
                {
                    _championStaticDataRepository.InsertChampionInformation(new LccChampionInformation(champion.Value.Id, champion.Value.Name));
                }

                _championStaticDataRepository.Save();
            }

            List<LccChampionInformation> championInformation = _championStaticDataRepository.GetAllChampions().ToList();

            return new JsonResult(championInformation);
        }


        [HttpGet("GetAllItemData")]
        public async Task<JsonResult> GetAllItemData()
        {
            if (_getUpdatedData)
            {
                ItemListStatic itemData = await _staticRiotApi.GetItemsAsync(RiotSharp.Misc.Region.euw);
                foreach(var item in itemData.Items)
                {
                    _itemStaticDataRepository.InsertItemInformation(new LccItemInformation(item.Key, item.Value.Name));
                }

                _itemStaticDataRepository.Save();
            }

            List<LccItemInformation> lccItemInformation = _itemStaticDataRepository.GetAllItems().ToList();

            return new JsonResult(lccItemInformation);
        }

        [HttpGet("GetAllSummonerSpellData")]
        public async Task<JsonResult> GetAllSummonerSpellData()
        {
            if (_getUpdatedData)
            {
                SummonerSpellListStatic summonerSpellData = await _staticRiotApi.GetSummonerSpellsAsync(RiotSharp.Misc.Region.euw);
                foreach (var summonerSpell in summonerSpellData.SummonerSpells)
                {
                    _summonerSpellStaticDataRepository.InsertSummonerSpellInformation(new LccSummonerSpellInformation(summonerSpell.Value.Id, summonerSpell.Value.Name));
                }

                _summonerSpellStaticDataRepository.Save();
            }

            List<LccSummonerSpellInformation> lccSummonerSpellInformation = _summonerSpellStaticDataRepository.GetAllSummonerSpells().ToList();

            return new JsonResult(lccSummonerSpellInformation);
        }

    }
}