using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.APIModels
{
    public class LccItem
    {
        public LccItem(int itemId, string itemName)
        {
            ItemId = itemId;
            ItemName = itemName;
        }

        public int ItemId { get; set; }

        public string ItemName { get; set; }
    }
}
