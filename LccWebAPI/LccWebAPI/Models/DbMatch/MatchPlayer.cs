using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.DbMatch
{
    public class MatchPlayer
    {
        //Primary Key
        public int Id { get; set; }

        

        //ForeignKeys
        public long MatchTeamId { get; set; }
        public virtual MatchTeam MatchTeam { get; set; }
    }
}
