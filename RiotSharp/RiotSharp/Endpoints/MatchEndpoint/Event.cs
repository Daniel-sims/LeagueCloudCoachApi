using System;
using Newtonsoft.Json;
using RiotSharp.Endpoints.MatchEndpoint.Enums;
using RiotSharp.Misc.Converters;

namespace RiotSharp.Endpoints.MatchEndpoint
{
    /// <summary>
    /// Class representing a particular event during a match (Match API).
    /// </summary>
    public class Event
    {
        internal Event() { }

        [JsonProperty("type")]
        public EventType Type { get; set; }

        [JsonProperty("timestamp")]
        [JsonConverter(typeof(TimeSpanConverterFromMilliseconds))]
        public TimeSpan Timestamp { get; set; }

        [JsonProperty("participantId")]
        public long? ParticipantId { get; set; }

        [JsonProperty("itemId")]
        public long? ItemId { get; set; }

        [JsonProperty("skillSlot")]
        public long? SkillSlot { get; set; }

        [JsonProperty("levelUpType")]
        public string LevelUpType { get; set; }

        [JsonProperty("wardType")]
        public string WardType { get; set; }

        [JsonProperty("creatorId")]
        public long? CreatorId { get; set; }

        [JsonProperty("position")]
        public Position Position { get; set; }

        [JsonProperty("killerId")]
        public long? KillerId { get; set; }

        [JsonProperty("victimId")]
        public long? VictimId { get; set; }

        [JsonProperty("assistingParticipantIds")]
        public long[] AssistingParticipantIds { get; set; }

        [JsonProperty("afterId")]
        public long? AfterId { get; set; }

        [JsonProperty("beforeId")]
        public long? BeforeId { get; set; }

        [JsonProperty("teamId")]
        public long? TeamId { get; set; }

        [JsonProperty("buildingType")]
        public string BuildingType { get; set; }

        [JsonProperty("laneType")]
        public string LaneType { get; set; }

        [JsonProperty("towerType")]
        public string TowerType { get; set; }

        [JsonProperty("monsterType")]
        public string MonsterType { get; set; }

        [JsonProperty("monsterSubType")]
        public string MonsterSubType { get; set; }
    }
}
