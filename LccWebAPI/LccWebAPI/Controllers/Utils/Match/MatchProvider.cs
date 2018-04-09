using LccWebAPI.Database.Context;
using LccWebAPI.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LccWebAPI.Controllers.Utils.Match
{
    public class MatchProvider : IMatchProvider
    {
        private readonly ILogging _logging;

        private readonly DatabaseContext _dbContext;

        public MatchProvider(ILogging logging, DatabaseContext databaseContext)
        {
            _logging = logging;
            _dbContext = databaseContext;
        }

        public IEnumerable<Models.Match.Match> GetMatchesForListOfTeamIds(int[] teamOneChampionIds, int[] teamTwoChampionIds, int matchCount)
        {
            return _dbContext.Matches
                .Include(x => x.Teams).ThenInclude(x => x.Players)
                .ToList()
                .Where(m =>
                    !teamOneChampionIds.Any() || !teamTwoChampionIds.Any()
                        ? IsMatchSingleTeam(m, teamOneChampionIds.Any() ? teamOneChampionIds : teamTwoChampionIds)
                        : IsMatchBothTeams(m, teamOneChampionIds, teamTwoChampionIds)).Take(matchCount);
        }

        public static bool IsMatchBothTeams(Models.Match.Match match, int[] teamIdsOne, int[] teamIdsTwo)
        {
            return  teamIdsOne.All(id => match.TeamOne.Players.Any(p => p.ChampionId == id)) &&
                    teamIdsTwo.All(id => match.TeamTwo.Players.Any(p => p.ChampionId == id))
                    ||
                    teamIdsTwo.All(id => match.TeamOne.Players.Any(p => p.ChampionId == id)) &&
                    teamIdsOne.All(id => match.TeamTwo.Players.Any(p => p.ChampionId == id));
        }

        public static bool IsMatchSingleTeam(Models.Match.Match match, int[] teamIds)
        {
            return  teamIds.All(id => match.TeamOne.Players.Any(p => p.ChampionId == id)) ||
                    teamIds.All(id => match.TeamTwo.Players.Any(p => p.ChampionId == id));
        }
    }
}
   
