using LccWebAPI.Controllers.Models.Match;
using LccWebAPI.Database.Models.Match;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Controllers.Utils.Match
{
    public interface IMatchControllerUtils
    {
        Task<Db_LccCachedCalculatedMatchupInfo> CreateDatabaseModelForCalculatedMatchupInfo(RiotSharp.Endpoints.MatchEndpoint.Match match, long usersChampionId);
        LccCalculatedMatchupInformation CreateLccCalculatedMatchupInformationFromCache(Db_LccCachedCalculatedMatchupInfo match);
    }
}
