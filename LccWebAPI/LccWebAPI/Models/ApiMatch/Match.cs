using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.ApiMatch
{
    public class Match
    {
        public long GameId { get; set; }
        public DateTime GameDate { get; set; }
        public TimeSpan GameDuration { get; set; }
        public string GamePatch { get; set; }
        public int? WinningTeamId { get; set; }
        public IEnumerable<MatchTeam> Teams { get; set; }
    }
}
