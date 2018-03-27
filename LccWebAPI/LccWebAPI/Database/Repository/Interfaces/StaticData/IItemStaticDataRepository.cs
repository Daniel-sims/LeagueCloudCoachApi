using LccWebAPI.Database.Models.StaticData;
using System.Collections.Generic;

namespace LccWebAPI.Repository.Interfaces.StaticData
{
    public interface IItemStaticDataRepository
    {
        void InsertItem(Db_LccItem item);

        IEnumerable<Db_LccItem> GetAllItems();
        Db_LccItem GetItemById(int itemId);

        void UpdateItem(Db_LccItem item);

        void DeleteItem(long itemId);

        void Save();
    }
}
