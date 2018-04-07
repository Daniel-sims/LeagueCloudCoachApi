using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.ApiStaticData
{
    public class Item
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ImageFull { get; set; }

        public string PlainText { get; set; }

        public string Description { get; set; }
        public string SanitizedDescription { get; set; }
    }
}
