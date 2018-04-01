using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Database.Models.Match
{
    public class Db_LccMatchTimelineFrame
    {
        public virtual IList<Db_MatchTimelineEvent> Events { get; set; }

        //Can't store this stuff in EF need to think of a different way
        //Probably a List of participantFrame objects
        //public Dictionary<string, ParticipantFrame> ParticipantFrames { get; set; }

        public TimeSpan Timestamp { get; set; }
    }
}
