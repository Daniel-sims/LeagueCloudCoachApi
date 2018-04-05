using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.DbMatch
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
    }
}
