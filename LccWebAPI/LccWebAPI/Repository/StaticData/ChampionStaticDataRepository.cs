using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LccWebAPI.DatabaseContexts;
using LccWebAPI.Models.DatabaseModels;

namespace LccWebAPI.Repository.StaticData
{
    public class ChampionStaticDataRepository : IChampionStaticDataRepository, IDisposable
    {
        private LccDatabaseContext _lccDatabaseContext;

        public ChampionStaticDataRepository(LccDatabaseContext lccDatabaseContext)
        {
            _lccDatabaseContext = lccDatabaseContext;
        }

        public IEnumerable<LccChampionInformation> GetAllChampions()
        {
            return _lccDatabaseContext.Champions.ToList();
        }

        public LccChampionInformation GetChampionById(int championId)
        {
            return _lccDatabaseContext.Champions.FirstOrDefault(x => x.ChampionId == championId);
        }

        public void InsertChampionInformation(LccChampionInformation championInformation)
        {
            _lccDatabaseContext.Champions.Add(championInformation);
        }
        
        public void UpdateChampionInformation(LccChampionInformation championInformation)
        {
            _lccDatabaseContext.Entry(championInformation).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void DeleteChampionInformation(long championId)
        {
            LccChampionInformation championInformation = _lccDatabaseContext.Champions.FirstOrDefault(x => x.ChampionId == championId);
            if (championInformation != null)
                _lccDatabaseContext.Champions.Remove(championInformation);
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
