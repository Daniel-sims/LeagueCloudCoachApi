using LccWebAPI.Database.Models.Match;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Database.Repository.Interfaces.Match
{
    public interface ICachedCalculatedMatchupInformationRepository : IDisposable
    {
        void InsertCalculatedMatchupInfo(Db_LccCachedCalculatedMatchupInfo match);

        IEnumerable<Db_LccCachedCalculatedMatchupInfo> GetAllCalculatedMatchups();
        Db_LccCachedCalculatedMatchupInfo GetCalculatedMatchupInfoByGameId(long gameId);

        void UpdateCalculatedMatchupInfo(Db_LccCachedCalculatedMatchupInfo matchup);

        void DeleteCalculatedMatchupInfo(long gameId);

        void Save();
    }
}
