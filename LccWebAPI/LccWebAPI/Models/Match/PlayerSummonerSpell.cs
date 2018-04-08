using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LccWebAPI.Models.Match
{
    public class PlayerSummonerSpell
    {
        // Primary key
        [JsonIgnore]
        public int Id { get; set; }
        
        // Game specific data
        public long? SummonerSpellId { get; set; }
        public int SummonerSpellSlot { get; set; }

        // Foreign Key
        [JsonIgnore]
        public int MatchPlayerId { get; set; }
        [JsonIgnore]
        public virtual MatchPlayer MatchPlayer { get; set; }
    }
}
