using System;
using System.Collections.Generic;
using System.Linq;

namespace LccWebAPI.Models.Match
{
    public class Match
    {
        // Primary Key
        public int Id { get; set; }

        // Game specific data
        public long GameId { get; set; }
        public DateTime GameDate { get; set; }
        public TimeSpan GameDuration { get; set; }
        public string GamePatch { get; set; }
        public int? WinningTeamId { get; set; }

        // Data 
        public virtual ICollection<MatchTeam> Teams { get; set; } = new List<MatchTeam>();
        public MatchTeam TeamOne => Teams.ElementAt(0);
        public MatchTeam TeamTwo => Teams.ElementAt(1);
    }
}
