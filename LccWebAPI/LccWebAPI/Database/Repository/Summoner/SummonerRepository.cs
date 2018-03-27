using LccWebAPI.Database.Context;
using LccWebAPI.Database.Models.Summoner;
using LccWebAPI.Database.Repository.Interface.Summoner;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LccWebAPI.Database.Repository.Summoner
{
    public class SummonerRepository : ISummonerRepository
    {
        private LccDatabaseContext _lccDatabaseContext;

        public SummonerRepository(LccDatabaseContext lccDatabaseContext)
        {
            _lccDatabaseContext = lccDatabaseContext;
        }

        public void InsertSummoner(Db_LccSummoner summoner)
        {
            _lccDatabaseContext.Summoners.Add(summoner);
        }

        public IEnumerable<Db_LccSummoner> GetAllSummoner()
        {
            return _lccDatabaseContext.Summoners.ToList();
        }

        public Db_LccSummoner GetSummonerByAccountId(long accountId)
        {
            return _lccDatabaseContext.Summoners.FirstOrDefault(x => x.AccountId == accountId);
        }

        public void UpdateSummoner(Db_LccSummoner summoner)
        {
            _lccDatabaseContext.Entry(summoner).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void DeleteSummoner(long accountId)
        {
            Db_LccSummoner summoner = _lccDatabaseContext.Summoners.FirstOrDefault(x => x.AccountId == accountId);
            if (summoner != null)
            {
                _lccDatabaseContext.Summoners.Remove(summoner);
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
