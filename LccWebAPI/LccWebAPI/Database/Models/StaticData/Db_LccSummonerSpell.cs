using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Database.Models.StaticData
{
    public class Db_LccSummonerSpell
    {
        public Db_LccSummonerSpell() { }

        [Key]
        public int Id { get; set; }

        public int SummonerSpellId { get; set; }

        public string SummonerSpellName { get; set; }

        public string ImageFull { get; set; }
    }
}
