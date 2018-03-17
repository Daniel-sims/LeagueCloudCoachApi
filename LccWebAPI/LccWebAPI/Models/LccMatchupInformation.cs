using RiotSharp.MatchEndpoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models
{
    //This object is used for quick lookup for matchups
    //You use this to find the GameId of a matchup before requesting it from Riot
    public class LccMatchupInformation
    {
        public LccMatchupInformation() { }
        public LccMatchupInformation(long gameId, List<long> championIds)
        {
            GameId = gameId;
            ChampionIds = championIds;
        }

        public int Id { get; set; }

        public long GameId { get; set; }

        public List<long> ChampionIds { get; set; }
    }
}
