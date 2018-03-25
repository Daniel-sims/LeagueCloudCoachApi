using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Database.Models.Match
{
    public class Db_LccBasicMatchInfoPlayer
    {
        public Db_LccBasicMatchInfoPlayer() { }

        [Key]
        public int Id { get; set; }

        public int ChampionId { get; set; }

        public long PlayerAccountId { get; set; }

        public string SummonerName { get; set; }

        public string Lane { get; set; }
    }
}
