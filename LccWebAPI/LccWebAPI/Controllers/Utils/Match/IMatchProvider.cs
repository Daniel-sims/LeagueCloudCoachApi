using System.Collections.Generic;

namespace LccWebAPI.Controllers.Utils.Match
{
    public interface IMatchProvider
    {
        IEnumerable<Models.Match.Match> GetMatchesForListOfTeamIds(IEnumerable<int> teamOne, IEnumerable<int> teamTwo, int matchCount);
    }
}
