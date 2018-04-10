using System.Collections.Generic;

namespace LccWebAPI.Models.Match
{
    public class MatchTimeline
    {
        // primary key
        public int Id { get; set; }

        public long GameId { get; set; }

        public virtual ICollection<MatchEvent> Events { get; set; }
    }
}
