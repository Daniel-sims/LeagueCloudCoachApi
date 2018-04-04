using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.DbMatch
{
    public class PlayerSummonerSpell
    {
        public int Id { get; set; }

        public int MatchPlayerId { get; set; }
        public virtual MatchPlayer MatchPlayer { get; set; }
    }
}
