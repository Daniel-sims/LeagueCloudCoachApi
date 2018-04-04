using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.Db
{
    public class PlayerSummonerSpell
    {
        // Primary key
        public int Id { get; set; }
        
        // Data
        public long SummonerSpellId { get; set; }

        // Foreign Key
        public int MatchPlayerId { get; set; }
        public virtual MatchPlayer MatchPlayer { get; set; }
    }
}
