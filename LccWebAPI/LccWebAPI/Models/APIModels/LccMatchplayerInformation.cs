using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.APIModels
{
    public class LccMatchplayerInformation
    {
        public string SummonerName { get; set; }

        public string Division { get; set; }

        public long Kills { get; set; }
        
        public long Deaths { get; set; }

        public long Assists { get; set; }

        public long MinionKills { get; set; }

        public long TrinketId { get; set; }

        public long Item1Id { get; set; }

        public long Item2Id { get; set; }

        public long Item3Id { get; set; }

        public long Item4Id { get; set; }

        public long Item5Id { get; set; }

        public long Item6Id { get; set; }

        public List<long> FirstItemsBought { get; set; }

        public long SummonerOne { get; set; }

        public long SummonerTwo { get; set; }
    }
}
