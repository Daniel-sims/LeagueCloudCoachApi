using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LccWebAPI.DatabaseContexts;
using LccWebAPI.Models;

namespace LccWebAPI.Repository
{
    public class MatchReferenceRepository : IMatchReferenceRepository, IDisposable
    {
        private MatchReferenceContext _matchReferenceContext;

        public MatchReferenceRepository(MatchReferenceContext matchReferenceContext)
        {
            _matchReferenceContext = matchReferenceContext;
        }

        public void InsertMatchReference(LccMatchReference matchReference)
        {
            _matchReferenceContext.MatchReferences.Add(matchReference);
        }

        public IEnumerable<LccMatchReference> GetAllMatchReferences()
        {
            return _matchReferenceContext.MatchReferences.ToList();
        }

        public LccMatchReference GetMatchReferenceByGameId(long gameId)
        {
            return _matchReferenceContext.MatchReferences.FirstOrDefault(x => x.GameId == gameId);
        }

        public void UpdateMatchReference(LccMatchReference matchReference)
        {
            _matchReferenceContext.Entry(matchReference).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void DeleteMatchReference(long gameId)
        {
            LccMatchReference matchRef = _matchReferenceContext.MatchReferences.FirstOrDefault(x => x.GameId == gameId);
            if (matchRef != null)
                _matchReferenceContext.MatchReferences.Remove(matchRef);
        }
        
        public void Save()
        {
            _matchReferenceContext.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _matchReferenceContext.Dispose();
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
