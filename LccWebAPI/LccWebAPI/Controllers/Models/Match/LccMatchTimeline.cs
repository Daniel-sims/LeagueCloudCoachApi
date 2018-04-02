using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Controllers.Models.Match
{
    public class LccMatchTimeline
    {
        public LccMatchTimeline() { }

        public TimeSpan FrameInterval { get; set; }

        public IList<LccMatchTimelineFrame> Frames { get; set; }
    }
}
