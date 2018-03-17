using RiotSharp.Misc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models
{
    public class LccMatchupInformationPlayer
    {
        public LccMatchupInformationPlayer() { }
        public LccMatchupInformationPlayer(long championId, string lane, LccSummoner lccSummoner, long accountId, string summonerName)
        {
            ChampionId = championId;
            Lane = lane;
            AccountId = accountId;
            SummonerName = summonerName;
        }
        
        public int Id { get; set; }

        //Game specific
        public long ChampionId { get; set; }
        public string Lane { get; set; }

        //Player specific
        public long AccountId { get; set; }
        public string SummonerName { get; set; }
    }
}
