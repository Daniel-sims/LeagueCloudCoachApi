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
        IEnumerable<SummonerDto> GetAllSummoners();
        SummonerDto GetSummonerByAccountId(long accountId);
        void InsertSummoner(SummonerDto summoner);
        void DeleteSummoner(long accountId);
        void UpdateSummoner(SummonerDto summoner);
        void Save();
    }
}
