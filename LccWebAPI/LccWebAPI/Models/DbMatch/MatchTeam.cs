using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.DbMatch
{
    public class MatchTeam
    {
        //PrimaryKey
        public int Id { get; set; }
        
        //Foreign Key
        public int MatchId { get; set; }
        public virtual Match Match { get; set; }
    }
}
