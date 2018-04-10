using Newtonsoft.Json;

namespace LccWebAPI.Models.StaticData
{
    public class Rune
    {
        // Primary Key
        [JsonIgnore]
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
