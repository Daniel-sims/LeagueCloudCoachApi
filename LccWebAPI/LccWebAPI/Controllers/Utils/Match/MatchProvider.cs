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
                var matchedPlayers = teamOneChampionIds
                    .Concat(teamTwoChampionIds)
                    .SelectMany(m =>
                        _dbContext.MatchPlayer.Where(p => p.ChampionId == m).Select(p => new { p.MatchTeam.MatchId, p.MatchTeamId, p.ChampionId })
                    );

                var playersByMatch = matchedPlayers.GroupBy(m => m.MatchId);
                matchedMatches = new List<int>();
                foreach (var matchPlayers in playersByMatch)
                {
                    // The two teams
                    var teams = matchPlayers.GroupBy(m => m.MatchTeamId);
                    var teamOnePlayers = teams.First();
                    var teamTwoPlayers = teams.Last();

                    if ((teamOnePlayers.Any(m => teamOneChampionIds.Contains(m.ChampionId)) && teamTwoPlayers.Any(m => teamTwoChampionIds.Contains(m.ChampionId))) ||
                        (teamTwoPlayers.Any(m => teamOneChampionIds.Contains(m.ChampionId)) && teamOnePlayers.Any(m => teamTwoChampionIds.Contains(m.ChampionId))))
                    {
                        if (!matchedMatches.Contains(matchPlayers.Key))
                        {
                            matchedMatches.Add(matchPlayers.Key);
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
   
