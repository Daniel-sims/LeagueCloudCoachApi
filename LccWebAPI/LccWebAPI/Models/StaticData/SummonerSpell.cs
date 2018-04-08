using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.StaticData
{
    public class SummonerSpell
    {
        // Primary Key
        [JsonIgnore]
        public int Id { get; set; }

        public int SummonerSpellId { get; set; }
        public string SummonerSpellName { get; set; }

        public string Description { get; set; }

        public string ImageFull { get; set; }
    }
}
