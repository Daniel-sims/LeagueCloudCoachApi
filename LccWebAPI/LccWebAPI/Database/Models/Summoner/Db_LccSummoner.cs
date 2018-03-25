using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Database.Models.Summoner
{
    public class Db_LccSummoner
    {
        public Db_LccSummoner() { }

        [Key]
        public int Id { get; set; }

        public int SummonerId { get; set; }
        
        public string SummonerName { get; set; }

        public DateTime LastUpdatedTime { get; set; }
    }
}
