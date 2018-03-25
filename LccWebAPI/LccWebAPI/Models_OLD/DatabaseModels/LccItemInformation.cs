using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.DatabaseModels
{
    public class LccItemInformation
    {
        public LccItemInformation() { }
        public LccItemInformation(int itemId, string itemName)
        {
            ItemId = itemId;
            ItemName = itemName;
        }

        // Primary key
        public int Id { get; set; }

        public int ItemId { get; set; }
        public string ItemName { get; set; }

        //More information about Items if needed
    }
}
