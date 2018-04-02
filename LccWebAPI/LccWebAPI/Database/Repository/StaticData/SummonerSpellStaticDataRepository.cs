using LccWebAPI.Database.Context;
using LccWebAPI.Database.Models.StaticData;
using LccWebAPI.Repository.Interfaces.StaticData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LccWebAPI.Repository.StaticData
{
    public class SummonerSpellStaticDataRepository : ISummonerSpellStaticDataRepository
    {
        private LccDatabaseContext _lccDatabaseContext;

        public SummonerSpellStaticDataRepository(LccDatabaseContext lccDatabaseContext)
        {
            _lccDatabaseContext = lccDatabaseContext;
        }
        
        public void InsertSummonerSpell(Db_LccSummonerSpell summonerSpell)
        {
            _lccDatabaseContext.SummonerSpells.Add(summonerSpell);
        }

        public IEnumerable<Db_LccSummonerSpell> GetAllSummonerSpells()
        {
            return _lccDatabaseContext.SummonerSpells;
        }

        public Db_LccSummonerSpell GetSummonerSpellById(int summonerSpellId)
        {
            return _lccDatabaseContext.SummonerSpells.FirstOrDefault(x => x.SummonerSpellId == summonerSpellId);
        }

        public void UpdateSummonerSpell(Db_LccSummonerSpell summonerSpell)
        {
            _lccDatabaseContext.Entry(summonerSpell).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void DeleteSummonerSpell(long summonerSpellId)
        {
            Db_LccSummonerSpell summonerSpell = _lccDatabaseContext.SummonerSpells.FirstOrDefault(x => x.SummonerSpellId == summonerSpellId);
            if (summonerSpell != null)
            {
                _lccDatabaseContext.SummonerSpells.Remove(summonerSpell);
            }
        }

        public void Save()
        {
            _lccDatabaseContext.SaveChanges();
        }
        
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _lccDatabaseContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
