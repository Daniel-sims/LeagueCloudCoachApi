using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Database.Models.Match
{
    public class Db_LccMatchTimeline
    {
        public Db_LccMatchTimeline() { }

        [Key]
        public int Id { get; set; }

        public TimeSpan FrameInterval { get; set; }
        
        public virtual ICollection<Db_LccMatchTimelineFrame> Frames { get; set; }
    }
}
