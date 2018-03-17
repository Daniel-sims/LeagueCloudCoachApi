using LccWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Repository.Match
{
    public interface IMatchupInformationRepository : IDisposable
    {
        IEnumerable<LccMatchupInformation> GetAllMatchupInformations();
        LccMatchupInformation GetMatchupInformationByGameId(long gameId);
        void InsertMatchupInformation(LccMatchupInformation matchupInformation);
        void DeleteMatchupInformation(long gameId);
        void UpdateMatchupInformation(LccMatchupInformation matchupInformation);
        void Save();
    }
}
