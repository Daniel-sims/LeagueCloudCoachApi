using RiotSharp.MatchEndpoint;
using RiotSharp.Misc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.DatabaseModels
{
    public class LccMatchupInformationPlayer
    {
        public LccMatchupInformationPlayer() { }
        public LccMatchupInformationPlayer(long championId, string lane, LccSummoner lccSummoner, long accountId, Player player)
        {
            ChampionId = championId;
            Lane = lane;
            AccountId = accountId;
            AccountId = player.AccountId;
            SummonerName = player.SummonerName;
        }
        
        public int Id { get; set; }
        
        public long AccountId { get; set; }
        public string SummonerName { get; set; }

        //Game specific
        public long ChampionId { get; set; }
        public string Lane { get; set; }
    }
}
