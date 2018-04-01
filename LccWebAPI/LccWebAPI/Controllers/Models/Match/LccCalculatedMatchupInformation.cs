using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Controllers.Models.Match
{
    public class LccCalculatedMatchupInformation
    {
        public long GameId { get; set; }

        public DateTime MatchDate { get; set; }

        public string MatchPatch { get; set; }
        
        public TimeSpan MatchDuration { get; set; }

        public bool FriendlyTeamWin { get; set; }

        public LccTeamInformation FriendlyTeam { get; set; }

        public LccTeamInformation EnemyTeam { get; set; }

        public LccMatchTimeline Timeline { get; set; }
    }
}
