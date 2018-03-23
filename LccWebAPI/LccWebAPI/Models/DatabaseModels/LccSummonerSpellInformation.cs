using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.DatabaseModels
{
    public class LccSummonerSpellInformation
    {
        public LccSummonerSpellInformation() { }
        public LccSummonerSpellInformation(int summonerId, string summonerName)
        {
            SummonerId = summonerId;
            SummonerName = summonerName;
        }

        // Primary key
        public int Id { get; set; }

        public int SummonerId { get; set; }
        public string SummonerName { get; set; }

        //More information about champions if needed
    }
}
