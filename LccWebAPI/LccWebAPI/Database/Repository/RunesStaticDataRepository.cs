using LccWebAPI.Database.Context;
using LccWebAPI.Database.Models.StaticData;
using LccWebAPI.Repository.StaticData.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LccWebAPI.Repository.StaticData
{
    public class RunesStaticDataRepository : IRunesStaticDataRepository
    {
        private LccDatabaseContext _lccDatabaseContext;

        public RunesStaticDataRepository(LccDatabaseContext lccDatabaseContext)
        {
            _lccDatabaseContext = lccDatabaseContext;
        }

        public void InsertRune(Db_LccRune rune)
        {
            _lccDatabaseContext.Runes.Add(rune);
        }

        public IEnumerable<Db_LccRune> GetAllRunes()
        {
            return _lccDatabaseContext.Runes.ToList();
        }

        public Db_LccRune GetRuneById(int runeId)
        {
            return _lccDatabaseContext.Runes.FirstOrDefault(x => x.RuneId == runeId);
        }

        public void UpdateRune(Db_LccRune rune)
        {
            _lccDatabaseContext.Entry(rune).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void DeleteRune(long runeId)
        {
            Db_LccRune rune = _lccDatabaseContext.Runes.FirstOrDefault(x => x.RuneId == runeId);
            if (rune != null)
            {
                _lccDatabaseContext.Runes.Remove(rune);
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
