using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Database.Models.StaticData
{
    public class Db_LccRune
    {
        public Db_LccRune() { }

        [Key]
        public int Id { get; set; }

        public int RuneId { get; set; }

        public string RuneName { get; set; }

        public string RunePathName { get; set; }

        public string ShortDesc { get; set; }

        public string LongDesc { get; set; }

        public string Key { get; set; }

        public string Icon { get; set; }
    }
}
