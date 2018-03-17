using LccWebAPI.DatabaseContexts;
using LccWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Repository
{
    public class MatchupInformationRepository : IMatchupInformationRepository, IDisposable
    {
        private MatchupInformationContext _matchupInformationContext;

        public MatchupInformationRepository(MatchupInformationContext matchupInformationContext)
        {
            _matchupInformationContext = matchupInformationContext;
        }

        public void InsertMatchupInformation(LccMatchupInformation match)
        {
            _matchupInformationContext.Matches.Add(match);
        }

        public IEnumerable<LccMatchupInformation> GetAllMatchupInformations()
        {
            return _matchupInformationContext.Matches.ToList();
        }

        public LccMatchupInformation GetMatchupInformationByGameId(long gameId)
        {
            return _matchupInformationContext.Matches.FirstOrDefault(x => x.GameId == gameId);
        }

        public void UpdateMatchupInformation(LccMatchupInformation match)
        {
            _matchupInformationContext.Entry(match).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void DeleteMatchupInformation(long gameId)
        {
            LccMatchupInformation matchupInformation = _matchupInformationContext.Matches.FirstOrDefault(x => x.GameId == gameId);
            if (matchupInformation != null)
                _matchupInformationContext.Matches.Remove(matchupInformation);
        }

        public void Save()
        {
            _matchupInformationContext.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _matchupInformationContext.Dispose();
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
