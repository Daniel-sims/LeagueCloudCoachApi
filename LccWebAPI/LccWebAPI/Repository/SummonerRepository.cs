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
        private SummonerDtoContext _summonerContext;

        public SummonerRepository(SummonerDtoContext summonerContext)
        {
            _summonerContext = summonerContext;
        }

        public void InsertSummoner(SummonerDto summonerDto)
        {
            _summonerContext.Summoners.Add(summonerDto);
        }

        public IEnumerable<SummonerDto> GetAllSummoners()
        {
            return _summonerContext.Summoners.ToList();
        }

        public SummonerDto GetSummonerByAccountId(long accountId)
        {
            return _summonerContext.Summoners.FirstOrDefault(x => x.Summoner.AccountId == accountId);
        }

        public void UpdateSummoner(SummonerDto summoner)
        {
            //_summonerContext.Entry(summoner).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void DeleteSummoner(long accountId)
        {
            SummonerDto summoner = _summonerContext.Summoners.Find(accountId);
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
