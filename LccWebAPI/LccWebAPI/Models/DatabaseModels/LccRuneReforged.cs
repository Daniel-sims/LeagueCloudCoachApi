using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.APIModels
{
    public class LccRuneReforged
    {
        public LccRuneReforged() { }
        public LccRuneReforged(string runePathName, int runePathId, string name, int runeId, string key,
            string shortDesc, string longDesc, string icon)
        {
            RunePathName = runePathName;
            RunePathId = runePathId;
            Name = name;
            RuneId = runeId;
            Key = key;
            ShortDesc = shortDesc;
            LongDesc = longDesc;
            Icon = icon;
        }
        
        public int Id { get; set; }

        public string RunePathName { get; set; }
        
        public int RunePathId { get; set; }
        
        public string Name { get; set; }
        
        public int RuneId { get; set; }
        
        public string Key { get; set; }
        
        public string ShortDesc { get; set; }
        
        public string LongDesc { get; set; }
        
        public string Icon { get; set; }
    }
}
