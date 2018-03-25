using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Database.Models
{
    public class Db_LccRune
    {
        public Db_LccRune() { }

        [Key]
        public int Id { get; set; }

        public int RuneId { get; set; }

        public string RuneName { get; set; }
    }
}
