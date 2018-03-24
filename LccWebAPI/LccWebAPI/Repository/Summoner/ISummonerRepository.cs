using LccWebAPI.Models.DatabaseModels;
using System;
using System.Collections.Generic;

namespace LccWebAPI.Repository.Summoner
{
    public interface ISummonerRepository : IDisposable
    {
        IEnumerable<LccSummoner> GetAllSummoners();
        LccSummoner GetSummonerByAccountId(long accountId);
        void InsertSummoner(LccSummoner summoner);
        void DeleteSummoner(long accountId);
        void UpdateSummoner(LccSummoner summoner);
        void Save();
    }
}
