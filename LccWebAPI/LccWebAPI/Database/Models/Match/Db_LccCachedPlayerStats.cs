using LccWebAPI.Controllers.Models.StaticData;
using LccWebAPI.Database.Models.StaticData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Database.Models.Match
{
    public class Db_LccCachedPlayerStats
    {
        public Db_LccCachedPlayerStats() { }
        
        [Key]
        public int Id { get; set; }

        public long SummonerId { get; set; }

        public string SummonerName { get; set; }

        public long Kills { get; set; }

        public long Deaths { get; set; }

        public long Assists { get; set; }

        public long MinionKills { get; set; }

        public string RankedSoloDivision { get; set; }

        public string RankedSoloTier { get; set; }

        public string RankedSoloLeaguePoints { get; set; }

        public long RankedSoloWins { get; set; }

        public long RankedSoloLosses { get; set; }

        public virtual Db_LccItem Trinket { get; set; }

        public virtual Db_LccItem ItemOne { get; set; }

        public virtual Db_LccItem ItemTwo { get; set; }

        public virtual Db_LccItem ItemThree { get; set; }

        public virtual Db_LccItem ItemFour { get; set; }

        public virtual Db_LccItem ItemFive { get; set; }

        public virtual Db_LccItem ItemSix { get; set; }
        
        public virtual Db_LccSummonerSpell SummonerOne { get; set; }

        public virtual Db_LccSummonerSpell SummonerTwo { get; set; }

        public virtual Db_LccChampion Champion { get; set; }

        public long ChampionLevel { get; set; }

        public virtual Db_LccRune PrimaryRuneStyle { get; set; }

        public virtual Db_LccRune PrimaryRuneSubOne { get; set; }

        public virtual Db_LccRune PrimaryRuneSubTwo { get; set; }

        public virtual Db_LccRune PrimaryRuneSubThree { get; set; }

        public virtual Db_LccRune PrimaryRuneSubFour { get; set; }

        public virtual Db_LccRune SecondaryRuneStyle { get; set; }

        public virtual Db_LccRune SecondaryRuneSubOne { get; set; }

        public virtual Db_LccRune SecondaryRuneSubTwo { get; set; }

        public virtual Db_LccMatchTimeline Timeline { get; set; }
    }
}
