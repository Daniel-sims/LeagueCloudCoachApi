using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LccWebAPI.DatabaseContexts;
using LccWebAPI.Models;
using RiotSharp.SummonerEndpoint;

namespace LccWebAPI.Repository.Summoner
{
    public class SummonerRepository : ISummonerRepository, IDisposable
    {
        private LccDatabaseContext _lccDatabaseContext;

        public SummonerRepository(LccDatabaseContext lccDatabaseContext)
        {
            _lccDatabaseContext = lccDatabaseContext;
        }

        public void InsertSummoner(LccSummoner summonerDto)
        {
            _lccDatabaseContext.Summoners.Add(summonerDto);
        }

        public IEnumerable<LccSummoner> GetAllSummoners()
        {
            return _lccDatabaseContext.Summoners.ToList();
        }

        public LccSummoner GetSummonerByAccountId(long accountId)
        {
            return _lccDatabaseContext.Summoners.FirstOrDefault(x => x.AccountId == accountId);
        }

        public void UpdateSummoner(LccSummoner summoner)
        {
            _lccDatabaseContext.Entry(summoner).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void DeleteSummoner(long accountId)
        {
            LccSummoner summoner = _lccDatabaseContext.Summoners.FirstOrDefault(x => x.AccountId == accountId);
            if(summoner != null)
                _lccDatabaseContext.Summoners.Remove(summoner);
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
