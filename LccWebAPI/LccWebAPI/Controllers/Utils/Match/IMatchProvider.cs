using System.Collections.Generic;

namespace LccWebAPI.Controllers.Utils.Match
{
    public interface IMatchProvider
    {
        IEnumerable<Models.ApiMatch.Match> GetMatchesForListOfTeamIds(long usersChampionId, IEnumerable<int> teamOne,
            IEnumerable<int> teamTwo, int matchCount);
    }
}
