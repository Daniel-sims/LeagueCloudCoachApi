using LccWebAPI.Models;
using RiotSharp.SummonerEndpoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Repository
{
    public interface ISummonerRepository
    {
        IEnumerable<LccSummoner> GetAllSummoners();
        LccSummoner GetSummonerByAccountId(long accountId);
        void InsertSummoner(LccSummoner summoner);
        void DeleteSummoner(long accountId);
        void UpdateSummoner(LccSummoner summoner);
        void Save();
    }
}
