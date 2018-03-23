using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LccWebAPI.DatabaseContexts;
using LccWebAPI.Models.DatabaseModels;

namespace LccWebAPI.Repository.StaticData
{
    public class ItemStaticDataRepository : IItemStaticDataRepository, IDisposable
    {
        private LccDatabaseContext _lccDatabaseContext;

        public ItemStaticDataRepository(LccDatabaseContext lccDatabaseContext)
        {
            _lccDatabaseContext = lccDatabaseContext;
        }

        public IEnumerable<LccItemInformation> GetAllItems()
        {
            return _lccDatabaseContext.Items.ToList();
        }

        public LccItemInformation GetItemById(int itemId)
        {
            return _lccDatabaseContext.Items.FirstOrDefault(x => x.ItemId == itemId);
        }

        public void InsertItemInformation(LccItemInformation itemInformation)
        {
            _lccDatabaseContext.Items.Add(itemInformation);
        }

        public void UpdateItemInformation(LccItemInformation itemInformation)
        {
            _lccDatabaseContext.Entry(itemInformation).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
