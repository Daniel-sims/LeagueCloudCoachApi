using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.DbStaticData
{
    public class Item
    {
        // Primary Key
        public int Id { get; set; }

        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ImageFull { get; set; }
    }
}
