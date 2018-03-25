using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Database.Models.Match
{
    public class Db_LccBasicMatchInfo
    {
        public Db_LccBasicMatchInfo() { }

        public int Id { get; set; }

        public long GameId { get; set; }
        
        public virtual IList<Db_LccBasicMatchInfoPlayer> WinningTeamChampions { get; set; }

        public virtual IList<Db_LccBasicMatchInfoPlayer> LosingTeamChampions { get; set; }
    }
}
