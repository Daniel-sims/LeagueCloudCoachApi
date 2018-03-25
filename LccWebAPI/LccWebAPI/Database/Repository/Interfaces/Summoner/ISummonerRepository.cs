using LccWebAPI.Database.Models.Summoner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Database.Repository.Interface.Summoner
{
    public interface ISummonerRepository : IDisposable
    {
        void InsertSummoner(Db_LccSummoner summoner);

        IEnumerable<Db_LccSummoner> GetAllSummoner();
        Db_LccSummoner GetSummonerByAccountId(long accountId);

        void UpdateSummoner(Db_LccSummoner summoner);

        void DeleteSummoner(long accountId);

        void Save();
    }
}
