using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LccWebAPI.Models.Match
{
    public class PlayerItem
    {
        // Primary Key
        public int Id { get; set; }

        // Game specific data
        public long? ItemId { get; set; }
        public int ItemSlot { get; set; }

        // Foreign Key
        [JsonIgnore]
        public int MatchPlayerId { get; set; }
        [JsonIgnore]
        public virtual MatchPlayer MatchPlayer { get; set; }
    }
}
