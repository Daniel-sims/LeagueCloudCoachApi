using LccWebAPI.Database.Context;
using LccWebAPI.Repository.Match.Interfaces;
using System;

namespace LccWebAPI.Repository.Match
{
    public class MatchupInformationRepository : IMatchupInformationRepository
    {
        private LccDatabaseContext _lccDatabaseContext;

        public MatchupInformationRepository(LccDatabaseContext lccDatabaseContext)
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
