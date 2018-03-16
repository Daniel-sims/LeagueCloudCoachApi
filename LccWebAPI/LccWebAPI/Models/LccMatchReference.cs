using RiotSharp.MatchEndpoint;
using RiotSharp.MatchEndpoint.Enums;
using RiotSharp.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models
{
    public class LccMatchReference
    {
        public LccMatchReference() { }
        public LccMatchReference(MatchReference matchReference)
        {
            ChampionId = matchReference.ChampionID;
            Lane = matchReference.Lane;
            GameId = matchReference.GameId;
            PlatformId = matchReference.PlatformID;
            Queue = matchReference.Queue;
            Region = matchReference.Region;
            Role = matchReference.Role;
            Season = matchReference.Season;
            TimeStamp = matchReference.Timestamp;
        }

        public int Id { get; set; }

        public long ChampionId { get; set; }

        public Lane Lane { get; set; }

        public long GameId { get; set; }

        public Platform PlatformId { get; set; }

        public string Queue { get; set; }

        public Region Region { get; set; }

        public Role Role { get; set; }

        public Season Season { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
