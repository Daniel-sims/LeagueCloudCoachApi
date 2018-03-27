using LccWebAPI.Database.Models.StaticData;
using System;
using System.Collections.Generic;

namespace LccWebAPI.Repository.Interfaces.StaticData
{
    public interface IRunesStaticDataRepository : IDisposable
    {
        void InsertRune(Db_LccRune rune);

        IEnumerable<Db_LccRune> GetAllRunes();
        Db_LccRune GetRuneById(int runeId);

        void UpdateRune(Db_LccRune rune);

        void DeleteRune(long runeId);

        void Save();
    }
}
