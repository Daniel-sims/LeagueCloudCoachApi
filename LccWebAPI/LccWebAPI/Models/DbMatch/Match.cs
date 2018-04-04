using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.Db
{
    public class Match
    {
        // Primary Key
        public int Id { get; set; }

        // Game specific data
        public long GameId { get; set; }
        public DateTime MatchDate { get; set; }
        public TimeSpan MatchDuration { get; set; }
        public string MatchPatch { get; set; }
        public int WinningTeamId { get; set; }

        // Data 
        public virtual ICollection<MatchTeam> Teams { get; set; }
    }
}
