using LccWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Repository
{
    public interface IMatchReferenceRepository
    {
        IEnumerable<MatchReferenceDto> GetAllMatchReferences();
        MatchReferenceDto GetMatchReferenceByGameId(long gameId);
        void InsertMatchReference(MatchReferenceDto matchReferenceDto);
        void DeleteMatchReference(long gameId);
        void UpdateMatchReference(MatchReferenceDto matchReferenceDto);
        void Save();
    }
}
