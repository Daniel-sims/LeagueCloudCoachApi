using System;
using LccWebAPI.Database.Context;
using LccWebAPI.Models.StaticData;
using RiotSharp.Endpoints.Interfaces.Static;
using RiotSharp.Misc;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Services
{
    public class StaticDataCollectionService : IStaticDataCollectionService
    {
        private readonly DatabaseContext _dbContext;
        private readonly IStaticDataEndpoints _staticDataEndpoints;

        public StaticDataCollectionService(DatabaseContext databaseContext, IStaticDataEndpoints staticDataEndpoints)
        {
            _dbContext = databaseContext;
            _staticDataEndpoints = staticDataEndpoints;
        }

        public async Task CollectStaticDataIfNeeded()
        {
            try
            {
                await CollectItemStaticData();
                await CollectChampionStaticData();
                await CollectRuneStaticData();
                await CollectSummonerSpellStaticData();
            }
            catch (Exception)
            {
            }

            _dbContext.SaveChanges();
        }

        private async Task CollectItemStaticData()
        {
            if (_dbContext.Items.Any())
                return;

            Console.WriteLine("Collection items.");

            var riotItemInformation = await _staticDataEndpoints.Item.GetItemsAsync(Region.euw);

            foreach (var riotItem in riotItemInformation.Items)
            {
               
                _dbContext.Items.Add(new Item()
                {
                    ItemId = riotItem.Value.Id,
                    ItemName = riotItem.Value.Name,
                    ImageFull = riotItem.Value.Image.Full,

                    PlainText = riotItem.Value.PlainText,

                    Description = riotItem.Value.Description,
                    SanitizedDescription = riotItem.Value.SanitizedDescription
                });
            }
        }

        private async Task CollectChampionStaticData()
        {

            if (_dbContext.Champions.Any())
                return;

            Console.WriteLine("Collection champions.");

            var riotChampionInformation = await _staticDataEndpoints.Champion.GetChampionsAsync(Region.euw);

            foreach (var riotChampion in riotChampionInformation.Champions)
            {
                _dbContext.Champions.Add(new Champion()
                {
                    ChampionId = riotChampion.Value.Id,
                    ChampionName = riotChampion.Value.Name,
                    ImageFull = riotChampion.Value.Image.Full
                });
            }
        }

        private async Task CollectSummonerSpellStaticData()
        {
            if (_dbContext.SummonerSpells.Any())
                return;

            Console.WriteLine("Collection summonerSpells.");

            var riotSummonerSpellInformation = await _staticDataEndpoints.SummonerSpell.GetSummonerSpellsAsync(Region.euw);

            foreach (var riotSummonerSpell in riotSummonerSpellInformation.SummonerSpells)
            {
                _dbContext.SummonerSpells.Add(new SummonerSpell()
                {
                    SummonerSpellId = riotSummonerSpell.Value.Id,
                    SummonerSpellName = riotSummonerSpell.Value.Name,
                    Description = riotSummonerSpell.Value.Description,
                    ImageFull = riotSummonerSpell.Value.Image.Full
                });
            }
        }

        private async Task CollectRuneStaticData()
        {
            if (_dbContext.Runes.Any())
                return;

            Console.WriteLine("Collection runes.");

            var riotRuneInformation = await _staticDataEndpoints.Rune.GetRunesReforgedAsync(Region.euw);

            foreach (var riotRune in riotRuneInformation)
            {
                _dbContext.Runes.Add(new Rune()
                {
                    RuneId = riotRune.Id,
                    RuneName = riotRune.Name,

                    //Parent style of this rune
                    RunePathId = riotRune.RunePathId,
                    RunePathName = riotRune.RunePathName,

                    Key = riotRune.Key,
                    ShortDesc = riotRune.ShortDesc,
                    LongDesc = riotRune.LongDesc
                });
            }
        }
    }
}
