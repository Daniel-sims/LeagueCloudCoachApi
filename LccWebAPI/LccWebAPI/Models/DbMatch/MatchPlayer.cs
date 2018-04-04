using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.Db
{
    public class MatchPlayer
    {
        // Primary Key
        public int Id { get; set; }
        
        // Data 
        public virtual ICollection<PlayerItem> Items { get; set; }
        public virtual ICollection<PlayerRune> Runes { get; set; }
        public virtual ICollection<PlayerSummonerSpell> SummonerSpells { get; set; }
        
        // Foreign Key
        public long MatchTeamId { get; set; }
        public virtual MatchTeam MatchTeam { get; set; }
    }
}
