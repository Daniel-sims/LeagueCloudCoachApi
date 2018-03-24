using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LccWebAPI.DatabaseContexts;
using LccWebAPI.Models.DatabaseModels;

namespace LccWebAPI.Repository.StaticData
{
    public class SummonerSpellStaticDataRepository : ISummonerSpellStaticDataRepository, IDisposable
    {
        private LccDatabaseContext _lccDatabaseContext;

        public SummonerSpellStaticDataRepository(LccDatabaseContext lccDatabaseContext)
        {
            _lccDatabaseContext = lccDatabaseContext;
        }

        public IEnumerable<LccSummonerSpellInformation> GetAllSummonerSpells()
        {
            return _lccDatabaseContext.SummonerSpells.ToList();
        }

        public LccSummonerSpellInformation GetSummonerSpellById(int summonerSpellId)
        {
            return _lccDatabaseContext.SummonerSpells.FirstOrDefault(x => x.SummonerSpellId == summonerSpellId);
        }

        public void InsertSummonerSpellInformation(LccSummonerSpellInformation summonerSpellInformation)
        {
            _lccDatabaseContext.SummonerSpells.Add(summonerSpellInformation);
        }

        public void UpdateSummonerSpellInformation(LccSummonerSpellInformation summonerSpellInformation)
        {
            _lccDatabaseContext.Entry(summonerSpellInformation).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void DeleteSummonerSpellInformation(long summonerSpellId)
        {
            LccSummonerSpellInformation summonerSpellInformation = _lccDatabaseContext.SummonerSpells.FirstOrDefault(x => x.SummonerSpellId == summonerSpellId);
            if (summonerSpellInformation != null)
                _lccDatabaseContext.SummonerSpells.Remove(summonerSpellInformation);
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
