using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LccWebAPI.DatabaseContexts;
using LccWebAPI.Models;
using RiotSharp.SummonerEndpoint;

namespace LccWebAPI.Repository
{
    public class SummonerRepository : ISummonerRepository, IDisposable
    {
        private SummonerContext _summonerContext;

        public SummonerRepository(SummonerContext summonerContext)
        {
            _summonerContext = summonerContext;
        }

        public void InsertSummoner(LccSummoner summonerDto)
        {
            _summonerContext.Summoners.Add(summonerDto);
        }

        public IEnumerable<LccSummoner> GetAllSummoners()
        {
            return _summonerContext.Summoners.ToList();
        }

        public LccSummoner GetSummonerByAccountId(long accountId)
        {
            return _summonerContext.Summoners.FirstOrDefault(x => x.AccountId == accountId);
        }

        public void UpdateSummoner(LccSummoner summoner)
        {
            _summonerContext.Entry(summoner).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void DeleteSummoner(long accountId)
        {
            LccSummoner summoner = _summonerContext.Summoners.FirstOrDefault(x => x.AccountId == accountId);
            if(summoner != null)
                _summonerContext.Summoners.Remove(summoner);
        }
        
        public void Save()
        {
            _summonerContext.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _summonerContext.Dispose();
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
