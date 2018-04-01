using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Database.Models.Match
{
    public class Db_LccMatchTimeline
    {
        public TimeSpan FrameInterval { get; set; }
        
        public virtual IList<Db_LccMatchTimelineFrame> Frames { get; set; }
    }
}
