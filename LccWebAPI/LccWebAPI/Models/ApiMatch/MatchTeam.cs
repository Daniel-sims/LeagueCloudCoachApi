using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.ApiMatch
{
    public class MatchTeam
    {
        public IEnumerable<MatchPlayer> Players { get; set; } = new List<MatchPlayer>();
        public int TeamId { get; set; }
        public int BaronKills { get; set; }
        public int DragonKills { get; set; }
        public int RiftHeraldKills { get; set; }
        public int InhibitorKills { get; set; }
    }
}
