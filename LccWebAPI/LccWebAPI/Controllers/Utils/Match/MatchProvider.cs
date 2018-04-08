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

        public IEnumerable<Models.Match.Match> GetMatchesForListOfTeamIds(IEnumerable<int> teamOne, IEnumerable<int> teamTwo, int matchCount)
        {
            Console.WriteLine(DateTime.Now + " finding metches of users request.");
            
            var allMatchesInDatabase = _dbContext.Matches
                .Include(x => x.Teams).ThenInclude(x => x.Players).ThenInclude(y => y.Events);

            var matches = allMatchesInDatabase
                    .ToList()
                    .Where(m =>
                        ((!teamOne.Any()) || (!teamTwo.Any()))
                            ?
                            IsMatchSingleTeam(m, ((teamOne.Any()) ? teamOne : teamTwo)) :
                            IsMatchBothTeams(m, teamOne, teamTwo)).ToList();

            Console.WriteLine(DateTime.Now + " found " + matches.Count() + " matches.");

            return matches.Take(matchCount);
        }

        public static bool IsMatchBothTeams(Models.Match.Match match, IEnumerable<int> teamIdsOne, IEnumerable<int> teamIdsTwo)
        {
            return (teamIdsOne.All(id => match.TeamOne.Players.Any(p => p.ChampionId == id)) &&
                    teamIdsTwo.All(id => match.TeamTwo.Players.Any(p => p.ChampionId == id))
                    ||
                    teamIdsTwo.All(id => match.TeamOne.Players.Any(p => p.ChampionId == id)) &&
                    teamIdsOne.All(id => match.TeamTwo.Players.Any(p => p.ChampionId == id)));
        }

        public static bool IsMatchSingleTeam(Models.Match.Match match, IEnumerable<int> teamIds)
        {
            return (teamIds.All(id => match.TeamOne.Players.Any(p => p.ChampionId == id)) ||
                    teamIds.All(id => match.TeamTwo.Players.Any(p => p.ChampionId == id)));
        }
    }
}
   
