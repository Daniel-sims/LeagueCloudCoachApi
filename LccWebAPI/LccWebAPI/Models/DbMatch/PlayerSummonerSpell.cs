using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.DbMatch
{
    public class PlayerSummonerSpell
    {
        // Primary key
        public int Id { get; set; }
        
        // Game specific data
        public long SummonerSpellId { get; set; }
        public int SummonerSpellSlot { get; set; }
        // Foreign Key
        public int MatchPlayerId { get; set; }
        public virtual MatchPlayer MatchPlayer { get; set; }
    }
}
