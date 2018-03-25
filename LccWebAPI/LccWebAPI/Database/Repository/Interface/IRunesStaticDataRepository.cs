using LccWebAPI.Database.Models.StaticData;
using LccWebAPI.Models.APIModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Repository.StaticData.Interfaces
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
