using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.DbStaticData
{
    public class Rune
    {
        // Primary Key
        public int Id { get; set; }

        public int RuneId { get; set; }
        public string RuneName { get; set; }
    }
}
