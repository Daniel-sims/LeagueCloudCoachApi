using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.Db
{
    public class MatchTeam
    {
        // Primary Key
        public int Id { get; set; }

        // Data 
        public virtual ICollection<MatchPlayer> Players { get; set; }

        // Foreign Key
        public int MatchId { get; set; }
        public virtual Match Match { get; set; }
    }
}
