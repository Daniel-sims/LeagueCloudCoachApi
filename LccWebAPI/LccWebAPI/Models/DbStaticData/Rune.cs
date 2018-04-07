﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.DbStaticData
{
    public class Rune
    {
        // Primary Key
        public int Id { get; set; }

        // Rune information
        public int RuneId { get; set; }
        public string RuneName { get; set; }

        public string Key { get; set; }
        public string LongDesc { get; set; }
        public string ShortDesc { get; set; }

        // Parent Rune (I.E top of the tree)
        public int RunePathId { get; set; }
        public string RunePathName { get; set; }
    }
}
