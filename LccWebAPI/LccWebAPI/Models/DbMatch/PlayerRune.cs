using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.Db
{
    public class PlayerRune
    {
        // Primary Key
        public int Id { get; set; }

        // Data
        public long RuneId { get; set; }

        // Foreign Key
        public int MatchPlayerId { get; set; }
        public virtual MatchPlayer MatchPlayer { get; set; }
    }
}
