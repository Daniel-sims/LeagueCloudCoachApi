using LccWebAPI.Database.Models.Match;
using RiotSharp.Endpoints.StaticDataEndpoint;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Database.Models.StaticData
{
    public class Db_LccChampion
    {
        public Db_LccChampion() { }
        
        [Key]
        public int Id { get; set; }

        public int ChampionId { get; set; }

        public string ChampionName { get; set; }
        
        public string ImageFull { get; set; }
    }
}
