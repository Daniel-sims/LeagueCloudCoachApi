using System.Collections.Generic;
using Newtonsoft.Json;

namespace LccWebAPI.Models.Match
{
    public class MatchTeam
    {
        // Primary Key
        [JsonIgnore]
        public int Id { get; set; }

        // Match specific data 
        public virtual ICollection<MatchPlayer> Players { get; set; } = new List<MatchPlayer>();
        public int TeamId { get; set; }
        public int BaronKills { get; set; }
        public int DragonKills { get; set; }
        public int RiftHeraldKills { get; set; }
        public int InhibitorKills { get; set; }

        // Foreign Key
        [JsonIgnore]
        public int MatchId { get; set; }
        [JsonIgnore]
        public virtual Match Match { get; set; }
    }
}
