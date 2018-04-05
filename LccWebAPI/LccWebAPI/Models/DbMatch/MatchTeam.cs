using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.DbMatch
{
    public class MatchTeam
    {
        // Primary Key
        public int Id { get; set; }

        // Match specific data 
        public virtual ICollection<MatchPlayer> Players { get; set; } = new List<MatchPlayer>();
        public int TeamId { get; set; }
        public int BaronKills { get; set; }
        public int DragonKills { get; set; }
        public int RiftHeraldKills { get; set; }
        public int InhibitorKills { get; set; }

        // Foreign Key
        public int MatchId { get; set; }
        public virtual Match Match { get; set; }
    }
}
