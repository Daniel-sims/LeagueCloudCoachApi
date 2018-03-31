using LccWebAPI.Controllers.Models.Match;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Database.Models.Match
{
    public class Db_LccCachedCalculatedMatchupInfo
    {
        public Db_LccCachedCalculatedMatchupInfo() { }

        [Key]
        public int Id { get; set; }

        public long GameId { get; set; }

        public DateTime MatchDate { get; set; }

        public TimeSpan MatchDuration { get; set; }

        public string MatchPatch { get; set; }

        public bool FriendlyTeamWin { get; set; }

        public virtual Db_LccCachedTeamInformation FriendlyTeam { get; set; }

        public virtual Db_LccCachedTeamInformation EnemyTeam { get; set; }
    }
}
