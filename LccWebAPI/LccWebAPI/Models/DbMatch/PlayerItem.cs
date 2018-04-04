using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.Db
{
    public class PlayerItem
    {
        // Primary Key
        public int Id { get; set; }

        // Game specific data
        public long ItemId { get; set; }
        public int ItemSlot { get; set; }
        
        // Foreign Key
        public int MatchPlayerId { get; set; }
        public virtual MatchPlayer MatchPlayer { get; set; }
    }
}
