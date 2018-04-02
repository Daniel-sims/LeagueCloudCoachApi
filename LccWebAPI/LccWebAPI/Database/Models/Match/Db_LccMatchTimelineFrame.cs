using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Database.Models.Match
{
    public class Db_LccMatchTimelineFrame
    {
        [Key]
        public int Id { get; set; }

        public virtual IList<Db_MatchTimelineEvent> Events { get; set; }
        
        public TimeSpan Timestamp { get; set; }
    }
}
