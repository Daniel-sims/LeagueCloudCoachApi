using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.ApiStaticData
{
    public class Rune
    {
        public int RuneId { get; set; }
        public string RuneName { get; set; }
        
        public string Key { get; set; }
        public string LongDesc { get; set; }
        public string ShortDesc { get; set; }

    }
}
