using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.DbMatch
{
    public class MatchPlayer
    {
        // Primary Key
        public int Id { get; set; }
        
        // General player data
        public long PlayerId { get; set; }

        // Game specific data
        public int TeamId { get; set; }
        public int ParticipantId { get; set; }

        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Assists { get; set; }

        public int ChampionId { get; set; }
        
        public virtual ICollection<PlayerItem> Items { get; set; }
        public virtual ICollection<PlayerRune> Runes { get; set; }
        public virtual ICollection<PlayerSummonerSpell> SummonerSpells { get; set; }
        
        // Foreign Key
        public long MatchTeamId { get; set; }
        public virtual MatchTeam MatchTeam { get; set; }
    }
}
