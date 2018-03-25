using LccWebAPI.Repository.Match.Interfaces;

namespace LccWebAPI.Repository.Match
{
    public class REPLACED_MatchupInformationRepository : REPLACED_IMatchupInformationRepository
    {
        //private REPLACED_LccDatabaseContext _lccDatabaseContext;

        //public MatchupInformationRepository(REPLACED_LccDatabaseContext lccDatabaseContext)
        //{
        //    _lccDatabaseContext = lccDatabaseContext;
        //}

        //public void InsertMatchupInformation(LccMatchupInformation match)
        //{
        //    _lccDatabaseContext.Matches.Add(match);
        //}

        //public IEnumerable<LccMatchupInformation> GetAllMatchupInformations()
        //{
        //    return _lccDatabaseContext.Matches.Include(r => r.LosingTeam).Include(x => x.WinningTeam);
        //}

        //public LccMatchupInformation GetMatchupInformationByGameId(long gameId)
        //{
        //    return _lccDatabaseContext.Matches.Include(r => r.LosingTeam).Include(x => x.WinningTeam).FirstOrDefault(x => x.GameId == gameId);
        //}

        //public void UpdateMatchupInformation(LccMatchupInformation match)
        //{
        //    _lccDatabaseContext.Entry(match).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        //}

        //public void DeleteMatchupInformation(long gameId)
        //{
        //    LccMatchupInformation matchupInformation = _lccDatabaseContext.Matches.Include(r => r.LosingTeam).Include(x => x.WinningTeam).FirstOrDefault(x => x.GameId == gameId);
        //    if (matchupInformation != null)
        //        _lccDatabaseContext.Matches.Remove(matchupInformation);
        //}

        //public void Save()
        //{
        //    _lccDatabaseContext.SaveChanges();
        //}

        //private bool disposed = false;

        //protected virtual void Dispose(bool disposing)
        //{
        //    if (!this.disposed)
        //    {
        //        if (disposing)
        //        {
        //            _lccDatabaseContext.Dispose();
        //        }
        //    }
        //    this.disposed = true;
        //}

        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}
    }
}
