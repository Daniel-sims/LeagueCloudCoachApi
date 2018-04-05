﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.DbSummoner
{
    public class Summoner
    {
        public int Id { get; set; }

        // General information about the summoner
        public int ProfileIconId { get; set; }
        public DateTime RevisionDate { get; set; }
        public long Level { get; set; }

        public long SummonerId { get; set; }
        public long AccountId { get; set; }
        public string SummonerName { get; set; }

        // Last date we checked to see for new matches
        public DateTime LastUpdatedDate { get; set; }
}
}
