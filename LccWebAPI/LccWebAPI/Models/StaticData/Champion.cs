using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.StaticData
{
    public class Champion
    {
        // Primary Key
        public int Id { get; set; }

        public int ChampionId { get; set; }
        public string ChampionName { get; set; }
        public string ImageFull { get; set; }
    }
}
