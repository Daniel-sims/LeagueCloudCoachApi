using LccWebAPI.Database.Models.StaticData;
using System;
using System.Collections.Generic;

namespace LccWebAPI.Repository.Interfaces.StaticData
{
    public interface IChampionStaticDataRepository : IDisposable
    {
        void InsertChampionInformation(Db_LccChampion champion);

        IEnumerable<Db_LccChampion> GetAllChampions();
        Db_LccChampion GetChampionById(int championId);
       
        void UpdateChampion(Db_LccChampion champion);

        void DeleteChampion(long championId);

        void Save();
    }
}
