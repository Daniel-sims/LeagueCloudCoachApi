using LccWebAPI.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Repository.StaticData
{
    public interface ISummonerSpellStaticDataRepository
    {
        IEnumerable<LccSummonerSpellInformation> GetAllSummonerSpells();
        LccSummonerSpellInformation GetSummonerSpellById(int summonerSpellId);
        void InsertSummonerSpellInformation(LccSummonerSpellInformation summonerSpellInformation);
        void UpdateSummonerSpellInformation(LccSummonerSpellInformation summonerSpellInformation);
        void DeleteSummonerSpellInformation(long summonerSpellId);
        void Save();
    }
}
