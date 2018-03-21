using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.APIModels
{
    public class LccTeamInformation
    {
        public List<LccMatchplayerInformation> Players { get; set; } = new List<LccMatchplayerInformation>();

        public long Kills { get; set; }

        public long Deaths { get; set; }

        public long Assists { get; set; }

        public long DragonKills { get; set; }

        public long BaronKills { get; set; }

        public long RiftHeraldKills { get; set; }

        public long InhibitorKills { get; set; }
    }
}
