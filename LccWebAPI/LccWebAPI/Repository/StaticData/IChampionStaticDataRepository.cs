using LccWebAPI.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Repository.StaticData
{
    public interface IChampionStaticDataRepository
    {
        IEnumerable<LccChampionInformation> GetAllChampions();
        LccChampionInformation GetChampionById(int championId);
        void InsertChampionInformation(LccChampionInformation championInformation);
        void UpdateChampionInformation(LccChampionInformation championInformation);
        void Save();
    }
}
