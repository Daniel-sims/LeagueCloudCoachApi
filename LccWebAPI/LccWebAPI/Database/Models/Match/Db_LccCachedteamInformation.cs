using LccWebAPI.Controllers.Models.Match;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LccWebAPI.Database.Models.Match
{
    public class Db_LccCachedTeamInformation
    {
        public Db_LccCachedTeamInformation() { }

        [Key]
        public int Id { get; set; }

        public virtual ICollection<Db_LccCachedPlayerStats> Players { get; set; }

        public long TotalKills { get; set; }

        public long TotalDeaths { get; set; }

        public long TotalAssists { get; set; }

        public long DragonKills { get; set; }

        public long BaronKills { get; set; }

        public long RiftHeraldKills { get; set; }

        public long InhibitorKills { get; set; }
    }
}
