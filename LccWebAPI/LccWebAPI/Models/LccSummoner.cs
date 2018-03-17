using RiotSharp.Misc;
using RiotSharp.SummonerEndpoint;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models
{
    public class LccSummoner
    {
        public LccSummoner() { }
        public LccSummoner(Summoner summoner)
        {
            LastUpdated = DateTime.Now;

            ProfileIconId = summoner.ProfileIconId;
            RevisionDate = summoner.RevisionDate;
            Level = summoner.Level;
            Region = summoner.Region;
            AccountId = summoner.AccountId;
            Name = summoner.Name;
        }

        [Key]
        public int Id { get; set; }

        public DateTime LastUpdated { get; set; }

        public int ProfileIconId { get; set; }

        public DateTime RevisionDate { get; set; }

        public long Level { get; set; }

        public Region Region { get; set; }

        public long AccountId { get; set; }

        public string Name { get; set; }
    }
}
