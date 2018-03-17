using RiotSharp.MatchEndpoint;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models
{
    //This object is used for quick lookup for matchups
    //You use this to find the GameId of a matchup before requesting it from Riot
    public class LccMatchupInformation
    {
        public LccMatchupInformation() { }
        public LccMatchupInformation(long gameId, List<LccMatchupInformationPlayer> winningTeam, List<LccMatchupInformationPlayer> losingTeam)
        {
            GameId = gameId;
            WinningTeam = winningTeam;
            LosingTeam = losingTeam;
        }

        public int Id { get; set; }

        public long GameId { get; set; }
        
        public virtual IList<LccMatchupInformationPlayer> WinningTeam { get; set; } = new List<LccMatchupInformationPlayer>();
        
        public virtual IList<LccMatchupInformationPlayer> LosingTeam { get; set; } = new List<LccMatchupInformationPlayer>();
    }
}
