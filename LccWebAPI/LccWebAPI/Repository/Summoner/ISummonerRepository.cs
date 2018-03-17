using LccWebAPI.Models;
using LccWebAPI.Models.DatabaseModels;
using RiotSharp.SummonerEndpoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
