using LccWebAPI.DatabaseContexts;
using LccWebAPI.Models.APIModels;
using LccWebAPI.Repository.StaticData.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Repository.StaticData
{
    public class REPLACED_RunesReforgedStaticDataRepository : REPLACED_IRunesReforgedStaticDataRepository
    {
        //private REPLACED_LccDatabaseContext _lccDatabaseContext;

        //public RunesReforgedStaticDataRepository(REPLACED_LccDatabaseContext lccDatabaseContext)
        //{
        //    _lccDatabaseContext = lccDatabaseContext;
        //}
        
        //public IEnumerable<LccRuneReforged> GetAllRunes()
        //{
        //    return _lccDatabaseContext.Runes.ToList();
        //}

        //public LccRuneReforged GetItemById(int runeId)
        //{
        //    return _lccDatabaseContext.Runes.FirstOrDefault(x => x.Id == runeId);
        //}

        //public void InsertRune(LccRuneReforged rune)
        //{
        //    _lccDatabaseContext.Runes.Add(rune);
        //}

        //public void UpdateRune(LccRuneReforged rune)
        //{
        //    _lccDatabaseContext.Entry(rune).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        //}

        //public void DeleteRune(long runeId)
        //{
        //    LccRuneReforged rune = _lccDatabaseContext.Runes.FirstOrDefault(x => x.Id == runeId);
        //    if (rune != null)
        //        _lccDatabaseContext.Runes.Remove(rune);
        //}

        //public void Save()
        //{
        //    _lccDatabaseContext.SaveChanges();
        //}

        //private bool disposed = false;

        //protected virtual void Dispose(bool disposing)
        //{
        //    if (!this.disposed)
        //    {
        //        if (disposing)
        //        {
        //            _lccDatabaseContext.Dispose();
        //        }
        //    }
        //    this.disposed = true;
        //}

        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}
    }
}
