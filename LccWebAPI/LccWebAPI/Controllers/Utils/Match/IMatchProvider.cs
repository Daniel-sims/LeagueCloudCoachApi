using System.Collections.Generic;

namespace LccWebAPI.Controllers.Utils.Match
{
    public interface IMatchProvider
    {
        IEnumerable<Models.Match.Match> GetMatchesForListOfTeamIds(int[] teamOneChampionIds, int[] teamTwoChampionIds, int matchCount);
    }
}
