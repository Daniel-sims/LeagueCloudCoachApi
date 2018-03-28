using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Controllers.Models.Summoner
{
    public class LccSummoner
    {
        public string SummonerName { get; set; }

        public long SummonerLevel { get; set; }

        public string RankedSoloDivision { get; set; }

        public string RankedSoloTier { get; set; }

        public string RankedSoloLeaguePoints { get; set; }

        public long RankedSoloWins { get; set; }

        public long RankedSoloLosses { get; set; }
    }
}
