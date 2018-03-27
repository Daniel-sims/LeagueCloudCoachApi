using LccWebAPI.Controllers.Models.StaticData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Controllers.Models.Match
{
    public class LccPlayerStats
    {
        public string SummonerName { get; set; }

        public long Kills { get; set; }

        public long Deaths { get; set; }

        public long Assists { get; set; }

        public long MinionKills { get; set; }

        public LccItemInformation Trinket { get; set; }

        public LccItemInformation ItemOne { get; set; }

        public LccItemInformation ItemTwo { get; set; }

        public LccItemInformation ItemThree { get; set; }

        public LccItemInformation ItemFour { get; set; }

        public LccItemInformation ItemFive { get; set; }

        public LccItemInformation ItemSix { get; set; }

        public List<LccItemInformation> FirstItems { get; set; }

        public LccSummonerSpellInformation SummonerOne { get; set; }

        public LccSummonerSpellInformation SummonerTwo { get; set; }

        public LccChampionInformation Champion { get; set; }

        public long ChampionLevel { get; set; }

        public LccRuneInformation PrimaryRuneStyle { get; set; }

        public LccRuneInformation PrimaryRuneSubOne { get; set; }

        public LccRuneInformation PrimaryRuneSubTwo { get; set; }

        public LccRuneInformation PrimaryRuneSubThree { get; set; }

        public LccRuneInformation PrimaryRuneSubFour { get; set; }

        public LccRuneInformation SecondaryRuneStyle { get; set; }

        public LccRuneInformation SecondaryRuneSubOne { get; set; }

        public LccRuneInformation SecondaryRuneSubTwo { get; set; }
    }
}
