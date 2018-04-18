using System;
using LccWebAPI.Database.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace LccWebAPI.Controllers.Utils.Match
{
    public class MatchProvider : IMatchProvider
    {
        private readonly DatabaseContext _dbContext;

        public MatchProvider(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }

        public IEnumerable<Models.Match.Match> GetMatchesForListOfTeamIds(int[] teamOneChampionIds, int[] teamTwoChampionIds, int matchCount)
        {
            // Collection of Ids (PK not Match)
            var matchedMatches = default(ICollection<int>);

            //If we only want one team to match a list
            if (teamOneChampionIds.Length == 0 || teamTwoChampionIds.Length == 0)
            {
                // get the list with the Ids in
                var championIds = teamOneChampionIds.Length == 0 ? teamTwoChampionIds : teamOneChampionIds;


                matchedMatches =
                    // Select many players where the champion Id matches the current itteration in championIds
                    championIds.SelectMany(m => _dbContext.MatchPlayer.Where(p => p.ChampionId == m)
                        // Get the matchId from this
                        .Select(p => p.MatchTeam.MatchId))
                    .Distinct()
                    .ToList();
            }
            else
            {
                // Find matches which contain championIds from the list
                // will be a list of this; { matchId = 6123123, TeamId = 100, ChampionId = 15 }
                var matchedPlayers = 
                    teamOneChampionIds.Concat(teamTwoChampionIds)
                    .SelectMany(m =>
                        _dbContext.MatchPlayer.Where(p => p.ChampionId == m).Select(p => new { p.MatchTeam.MatchId, p.TeamId, p.ChampionId })
                    );

                //Gets the players for each match using the matchId as all MatchPlayers will have the same MatchId
                var playersByMatch = matchedPlayers.GroupBy(m => m.MatchId);

                matchedMatches = new List<int>();
                foreach (var matchPlayers in playersByMatch)
                {
                    // Group by TeamId to seperate into two teams
                    var teams = matchPlayers.GroupBy(m => m.TeamId).ToList();
                    
                    if (teams.Count == 2)
                    {
                        var teamOnePlayers = teams.First();
                        var teamTwoPlayers = teams.Last();

                        // check to see if the teams contain the Ids
                        if ((teamOneChampionIds.All(m => teamOnePlayers.Any(x => x.ChampionId == m)) && teamTwoChampionIds.All(m => teamTwoPlayers.Any(x => x.ChampionId == m))) ||
                            (teamTwoChampionIds.All(m => teamOnePlayers.Any(x => x.ChampionId == m)) && teamOneChampionIds.All(m => teamTwoPlayers.Any(x => x.ChampionId == m))))
                        {
                            // If this match isn't already in the list
                            if (!matchedMatches.Contains(matchPlayers.Key))
                            {
                                matchedMatches.Add(matchPlayers.Key);
                            }
                        }
                    }
                }
            }

            // Order the Ids by descending order to get the latest and get only the amount we want to return
            var matchIds = matchedMatches.OrderByDescending(x => x).Take(matchCount);
            
            return _dbContext.Matches
                .Include(x => x.Teams)
                .ThenInclude(x => x.Players)
                //Order matches by date
                .OrderByDescending(x => x.GameDate)
                //where the matchIds contains the current games Id
                .Where(x => matchIds.Contains(x.Id));
        }
    }
}
   
