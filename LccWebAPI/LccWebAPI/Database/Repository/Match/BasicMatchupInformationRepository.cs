using LccWebAPI.Database.Context;
using LccWebAPI.Database.Models.Match;
using LccWebAPI.Repository.Interfaces.Match;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LccWebAPI.Repository.Match
{
    public class BasicMatchupInformationRepository : IBasicMatchupInformationRepository
    {
        private LccDatabaseContext _lccDatabaseContext;

        public BasicMatchupInformationRepository(LccDatabaseContext lccDatabaseContext)
        {
            _lccDatabaseContext = lccDatabaseContext;
        }

        public void InsertMatchup(Db_LccBasicMatchInfo match)
        {
            _lccDatabaseContext.Matchups.Add(match);
        }

        public IEnumerable<Db_LccBasicMatchInfo> GetAllMatchups()
        {
            return _lccDatabaseContext.Matchups.ToList();
        }

        public Db_LccBasicMatchInfo GetMatchupByGameId(int gameId)
        {
            return _lccDatabaseContext.Matchups.FirstOrDefault(x => x.GameId == gameId);
        }

        public void UpdateMatchup(Db_LccBasicMatchInfo matchup)
        {
            _lccDatabaseContext.Entry(matchup).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void DeleteMatchup(long gameId)
        {
            Db_LccBasicMatchInfo matchup = _lccDatabaseContext.Matchups.FirstOrDefault(x => x.GameId == gameId);
            if (matchup != null)
            {
                _lccDatabaseContext.Matchups.Remove(matchup);
            }
        }

        public void Save()
        {
            throw new NotImplementedException();
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
