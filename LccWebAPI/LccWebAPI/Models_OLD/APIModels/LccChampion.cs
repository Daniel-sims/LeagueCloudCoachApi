using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.APIModels
{
    public class LccChampion
    {
        public LccChampion(int championId, string itemName)
        {
            ChampionId = championId;
            ItemName = itemName;
        }

        public int ChampionId { get; set; }

        public string ItemName { get; set; }
    }
}
