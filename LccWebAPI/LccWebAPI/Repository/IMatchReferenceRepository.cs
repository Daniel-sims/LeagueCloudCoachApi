using LccWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Repository
{
    public interface IMatchReferenceRepository : IDisposable
    {
        IEnumerable<LccMatchReference> GetAllMatchReferences();
        LccMatchReference GetMatchReferenceByGameId(long gameId);
        void InsertMatchReference(LccMatchReference matchReference);
        void DeleteMatchReference(long gameId);
        void UpdateMatchReference(LccMatchReference matchReference);
        void Save();
    }
}
