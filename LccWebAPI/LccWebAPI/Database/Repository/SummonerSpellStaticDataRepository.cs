using LccWebAPI.Database.Context;
using LccWebAPI.Repository.StaticData.Interfaces;
using System;

namespace LccWebAPI.Repository.StaticData
{
    public class SummonerSpellStaticDataRepository : ISummonerSpellStaticDataRepository
    {
        private LccDatabaseContext _lccDatabaseContext;

        public SummonerSpellStaticDataRepository(LccDatabaseContext lccDatabaseContext)
        {
            _lccDatabaseContext = lccDatabaseContext;
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
