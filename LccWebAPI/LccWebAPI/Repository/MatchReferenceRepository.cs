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
        private MatchReferenceDtoContext _matchReferenceDtoContext;

        public MatchReferenceRepository(MatchReferenceDtoContext matchReferenceDtoContext)
        {
            _matchReferenceDtoContext = matchReferenceDtoContext;
        }

        public void InsertMatchReference(MatchReferenceDto matchReferenceDto)
        {
            _matchReferenceDtoContext.MatchReferences.Add(matchReferenceDto);
        }

        public IEnumerable<MatchReferenceDto> GetAllMatchReferences()
        {
            return _matchReferenceDtoContext.MatchReferences.ToList();
        }

        public MatchReferenceDto GetMatchReferenceByGameId(long gameId)
        {
            return _matchReferenceDtoContext.MatchReferences.FirstOrDefault(x => x.MatchReference.GameId == gameId);
        }

        public void UpdateMatchReference(MatchReferenceDto matchReferenceDto)
        {
            _matchReferenceDtoContext.Entry(matchReferenceDto).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void DeleteMatchReference(long gameId)
        {
            MatchReferenceDto matchReference = _matchReferenceDtoContext.MatchReferences.Find(gameId);
            _matchReferenceDtoContext.MatchReferences.Remove(matchReference);
        }
        
        public void Save()
        {
            _matchReferenceDtoContext.SaveChanges();
        }
        
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _matchReferenceDtoContext.Dispose();
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
