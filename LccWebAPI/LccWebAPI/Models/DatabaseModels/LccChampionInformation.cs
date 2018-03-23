using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.DatabaseModels
{
    public class LccChampionInformation
    {
        public LccChampionInformation() { }
        public LccChampionInformation(int championId, string championName)
        {
            ChampionId = championId;
            ChampionName = championName;
        }

        // Primary key
        public int Id { get; set; }

        public int ChampionId { get; set; }
        public string ChampionName { get; set; }

        //More information about champions if needed
    }
}
