using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.Db
{
    public class Match
    {
        // Primary Key
        public int Id { get; set; }

        // Data 
        public virtual ICollection<MatchTeam> Teams { get; set; }
    }
}
