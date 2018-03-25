using LccWebAPI.Database.Context;
using LccWebAPI.DatabaseContexts;
using LccWebAPI.Models.APIModels;
using LccWebAPI.Repository.StaticData.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Repository.StaticData
{
    public class RunesStaticDataRepository : IRunesStaticDataRepository
    {
        private LccDatabaseContext _lccDatabaseContext;

        public RunesStaticDataRepository(LccDatabaseContext lccDatabaseContext)
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
