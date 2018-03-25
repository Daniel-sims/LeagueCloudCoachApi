using LccWebAPI.DatabaseContexts;
using LccWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LccWebAPI.Models.DatabaseModels;
using LccWebAPI.Repository.Match.Interfaces;

namespace LccWebAPI.Repository.Match
{
    public class MatchupInformationRepository : IMatchupInformationRepository
    {
        private REPLACED_LccDatabaseContext _lccDatabaseContext;

        public MatchupInformationRepository(REPLACED_LccDatabaseContext lccDatabaseContext)
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
