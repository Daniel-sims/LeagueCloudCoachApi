using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Controllers.Models.Match
{
    public class LccMatchTimelineFrame
    {
        public IList<LccMatchTimelineEvent> Events { get; set; }

        public TimeSpan Timestamp { get; set; }
    }
}
