using LccWebAPI.Database.Models.StaticData;
using System;
using System.Collections.Generic;

namespace LccWebAPI.Repository.StaticData.Interfaces
{
    public interface ISummonerSpellStaticDataRepository : IDisposable
    {
        void InsertSummonerSpell(Db_LccSummonerSpell summonerSpell);

        IEnumerable<Db_LccSummonerSpell> GetAllSummonerSpells();
        Db_LccSummonerSpell GetSummonerSpellById(int summonerSpellId);

        void UpdateSummonerSpell(Db_LccSummonerSpell summonerSpell);

        void DeleteSummonerSpell(long summonerSpellId);

        void Save();
    }
}
