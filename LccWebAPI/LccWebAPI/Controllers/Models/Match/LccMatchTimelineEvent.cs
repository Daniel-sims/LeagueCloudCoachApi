using RiotSharp.Endpoints.MatchEndpoint.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Controllers.Models.Match
{
    public class LccMatchTimelineEvent
    {
        public LccMatchTimelineEvent() { }

        public string Type { get; set; }

        public long Timestamp { get; set; }

        public long? ParticipantId { get; set; }

        public long? ItemId { get; set; }

        public long? SkillSlot { get; set; }

        public string LevelUpType { get; set; }

        public string WardType { get; set; }

        public long? CreatorId { get; set; }

        public long? KillerId { get; set; }

        public long? VictimId { get; set; }
        
        public long? AfterId { get; set; }

        public long? BeforeId { get; set; }

        public long? TeamId { get; set; }

        public string BuildingType { get; set; }

        public string LaneType { get; set; }

        public string TowerType { get; set; }

        public string MonsterType { get; set; }

        public string MonsterSubType { get; set; }
    }
}
