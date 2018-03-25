using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Database.Models
{
    public class Db_LccBasicMatchInfo
    {
        public Db_LccBasicMatchInfo() { }

        public int Id { get; set; }

        public long GameId { get; set; }

        public DateTime MatchDate { get; set; }

        public string MatchPatch { get; set; }

        public IList<int> WinningTeamChampionIds { get; set; }

        public IList<int> LosingTeamChampionIds { get; set; }
    }
}
