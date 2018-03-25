using LccWebAPI.Database.Models.Match;
using System;
using System.Collections.Generic;

namespace LccWebAPI.Repository.Interfaces.Match
{
    public interface IBasicMatchupInformationRepository : IDisposable
    {
        void InsertMatchup(Db_LccBasicMatchInfo match);

        IEnumerable<Db_LccBasicMatchInfo> GetAllMatchups();
        Db_LccBasicMatchInfo GetMatchupByGameId(int gameId);

        void UpdateMatchup(Db_LccBasicMatchInfo matchup);

        void DeleteMatchup(long gameId);

        void Save();
    }
}
