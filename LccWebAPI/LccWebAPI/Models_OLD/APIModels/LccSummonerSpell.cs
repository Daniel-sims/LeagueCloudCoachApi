using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.APIModels
{
    public class LccSummonerSpell
    {
        public LccSummonerSpell(int summonerSpellId, string summonerSpellName)
        {
            SummonerSpellId = summonerSpellId;
            SummonerSpellName = summonerSpellName;
        }

        public int SummonerSpellId { get; set; }

        public string SummonerSpellName { get; set; }
    }
}
