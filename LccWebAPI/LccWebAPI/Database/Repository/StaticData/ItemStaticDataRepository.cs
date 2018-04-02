using LccWebAPI.Database.Context;
using LccWebAPI.Database.Models.StaticData;
using LccWebAPI.Repository.Interfaces.StaticData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LccWebAPI.Repository.StaticData
{
    public class ItemStaticDataRepository : IItemStaticDataRepository
    {
        private LccDatabaseContext _lccDatabaseContext;

        public ItemStaticDataRepository(LccDatabaseContext lccDatabaseContext)
        {
            _lccDatabaseContext = lccDatabaseContext;
        }

        public void InsertItem(Db_LccItem item)
        {
            _lccDatabaseContext.Items.Add(item);
        }

        public IEnumerable<Db_LccItem> GetAllItems()
        {
            return _lccDatabaseContext.Items;
        }

        public Db_LccItem GetItemById(int itemId)
        {
            return _lccDatabaseContext.Items.FirstOrDefault(x => x.ItemId == itemId);
        }

        public void UpdateItem(Db_LccItem item)
        {
            _lccDatabaseContext.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void DeleteItem(long itemId)
        {
            Db_LccItem item = _lccDatabaseContext.Items.FirstOrDefault(x => x.ItemId == itemId);
            if (item != null)
            {
                _lccDatabaseContext.Items.Remove(item);
            }
        }

        public void Save()
        {
            _lccDatabaseContext.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _lccDatabaseContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
