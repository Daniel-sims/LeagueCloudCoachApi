using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.DbMatch
{
    public class PlayerRune
    {
        // Primary Key
        public int Id { get; set; }

        // Game specific data
        public long RuneId { get; set; }
        public int RuneSlot { get; set; }

        // Foreign Key
        public int MatchPlayerId { get; set; }
        public virtual MatchPlayer MatchPlayer { get; set; }
    }
}
