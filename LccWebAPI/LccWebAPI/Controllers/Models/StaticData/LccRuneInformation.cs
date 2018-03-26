using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Controllers.Models.StaticData
{
    public class LccRuneInformation
    {
        public long RuneId { get; set; }

        public string RuneName { get; set; }

        public string RunePathName { get; set; }

        public string ShortDesc { get; set; }

        public string LongDesc { get; set; }

        public string Key { get; set; }

        public string Icon { get; set; }

    }
}
