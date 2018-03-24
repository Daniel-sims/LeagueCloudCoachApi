using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.DatabaseModels
{
    public class LccSummonerSpellInformation
    {
        public LccSummonerSpellInformation() { }
        public LccSummonerSpellInformation(int summonerSpellId, string summonerSpellName)
        {
            SummonerSpellId = summonerSpellId;
            SummonerSpellName = summonerSpellName;
        }

        // Primary key
        public int Id { get; set; }

        public int SummonerSpellId { get; set; }
        public string SummonerSpellName { get; set; }

        //More information about champions if needed
    }
}
