using LccWebAPI.Database.Context;
using LccWebAPI.Database.Models.Match;
using LccWebAPI.Database.Repository.Interfaces.Match;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Database.Repository.Match
{
    public class CachedCalculatedMatchupInformationRepository : ICachedCalculatedMatchupInformationRepository
    {
        private LccDatabaseContext _lccDatabaseContext;

        public CachedCalculatedMatchupInformationRepository(LccDatabaseContext lccDatabaseContext)
        {
            _lccDatabaseContext = lccDatabaseContext;
        }

        public void InsertCalculatedMatchupInfo(Db_LccCachedCalculatedMatchupInfo match)
        {
            _lccDatabaseContext.CalculatedMatchupInformation.Add(match);
        }

        public IEnumerable<Db_LccCachedCalculatedMatchupInfo> GetAllCalculatedMatchups()
        {
            return _lccDatabaseContext.CalculatedMatchupInformation
                .Include(x => x.EnemyTeam)
                .Include(x => x.EnemyTeam.Players)
                .Include(x => x.FriendlyTeam)
                .Include(x => x.FriendlyTeam.Players)
                .Include(x => x.FriendlyTeam.Players.Select(y => y.Trinket))
                .Include(x => x.FriendlyTeam.Players.Select(y => y.ItemOne))
                .Include(x => x.FriendlyTeam.Players.Select(y => y.ItemTwo))
                .Include(x => x.FriendlyTeam.Players.Select(y => y.ItemThree))
                .Include(x => x.FriendlyTeam.Players.Select(y => y.ItemFour))
                .Include(x => x.FriendlyTeam.Players.Select(y => y.ItemFive))
                .Include(x => x.FriendlyTeam.Players.Select(y => y.ItemSix))
                .Include(x => x.FriendlyTeam.Players.Select(y => y.SummonerOne))
                .Include(x => x.FriendlyTeam.Players.Select(y => y.SummonerTwo))
                .Include(x => x.FriendlyTeam.Players.Select(y => y.PrimaryRuneStyle))
                .Include(x => x.FriendlyTeam.Players.Select(y => y.PrimaryRuneSubOne))
                .Include(x => x.FriendlyTeam.Players.Select(y => y.PrimaryRuneSubTwo))
                .Include(x => x.FriendlyTeam.Players.Select(y => y.PrimaryRuneSubThree))
                .Include(x => x.FriendlyTeam.Players.Select(y => y.PrimaryRuneSubFour))
                .Include(x => x.FriendlyTeam.Players.Select(y => y.SecondaryRuneStyle))
                .Include(x => x.FriendlyTeam.Players.Select(y => y.SecondaryRuneSubOne))
                .Include(x => x.FriendlyTeam.Players.Select(y => y.SecondaryRuneSubTwo))
                .ToList();
        }

        public Db_LccCachedCalculatedMatchupInfo GetCalculatedMatchupInfoByGameId(long gameId)
        {
            return _lccDatabaseContext.CalculatedMatchupInformation
                .Include(x => x.EnemyTeam)
                .Include(x => x.FriendlyTeam)
                .Include(x => x.EnemyTeam.Players).ThenInclude(y => y.Trinket)
                .Include(x => x.EnemyTeam.Players).ThenInclude(x => x.ItemOne)
                .Include(x => x.EnemyTeam.Players).ThenInclude(x => x.ItemTwo)
                .Include(x => x.EnemyTeam.Players).ThenInclude(x => x.ItemThree)
                .Include(x => x.EnemyTeam.Players).ThenInclude(x => x.ItemFour)
                .Include(x => x.EnemyTeam.Players).ThenInclude(x => x.ItemFive)
                .Include(x => x.EnemyTeam.Players).ThenInclude(x => x.ItemSix)
                .Include(x => x.EnemyTeam.Players).ThenInclude(x => x.Champion)
                .Include(x => x.EnemyTeam.Players).ThenInclude(x => x.PrimaryRuneStyle)
                .Include(x => x.EnemyTeam.Players).ThenInclude(x => x.PrimaryRuneSubOne)
                .Include(x => x.EnemyTeam.Players).ThenInclude(x => x.PrimaryRuneSubTwo)
                .Include(x => x.EnemyTeam.Players).ThenInclude(x => x.PrimaryRuneSubThree)
                .Include(x => x.EnemyTeam.Players).ThenInclude(x => x.PrimaryRuneSubFour)
                .Include(x => x.EnemyTeam.Players).ThenInclude(x => x.SecondaryRuneStyle)
                .Include(x => x.EnemyTeam.Players).ThenInclude(x => x.SecondaryRuneSubOne)
                .Include(x => x.EnemyTeam.Players).ThenInclude(x => x.SecondaryRuneSubTwo)
                .Include(x => x.EnemyTeam.Players).ThenInclude(x => x.SummonerOne)
                .Include(x => x.EnemyTeam.Players).ThenInclude(x => x.SummonerTwo)
                .Include(x => x.FriendlyTeam.Players).ThenInclude(y => y.Trinket)
                .Include(x => x.FriendlyTeam.Players).ThenInclude(x => x.ItemOne)
                .Include(x => x.FriendlyTeam.Players).ThenInclude(x => x.ItemTwo)
                .Include(x => x.FriendlyTeam.Players).ThenInclude(x => x.ItemThree)
                .Include(x => x.FriendlyTeam.Players).ThenInclude(x => x.ItemFour)
                .Include(x => x.FriendlyTeam.Players).ThenInclude(x => x.ItemFive)
                .Include(x => x.FriendlyTeam.Players).ThenInclude(x => x.ItemSix)
                .Include(x => x.FriendlyTeam.Players).ThenInclude(x => x.Champion)
                .Include(x => x.FriendlyTeam.Players).ThenInclude(x => x.PrimaryRuneStyle)
                .Include(x => x.FriendlyTeam.Players).ThenInclude(x => x.PrimaryRuneSubOne)
                .Include(x => x.FriendlyTeam.Players).ThenInclude(x => x.PrimaryRuneSubTwo)
                .Include(x => x.FriendlyTeam.Players).ThenInclude(x => x.PrimaryRuneSubThree)
                .Include(x => x.FriendlyTeam.Players).ThenInclude(x => x.PrimaryRuneSubFour)
                .Include(x => x.FriendlyTeam.Players).ThenInclude(x => x.SecondaryRuneStyle)
                .Include(x => x.FriendlyTeam.Players).ThenInclude(x => x.SecondaryRuneSubOne)
                .Include(x => x.FriendlyTeam.Players).ThenInclude(x => x.SecondaryRuneSubTwo)
                .Include(x => x.FriendlyTeam.Players).ThenInclude(x => x.SummonerOne)
                .Include(x => x.FriendlyTeam.Players).ThenInclude(x => x.SummonerTwo)
                .FirstOrDefault(x => x.GameId == gameId);
        }

        public void UpdateCalculatedMatchupInfo(Db_LccCachedCalculatedMatchupInfo matchup)
        {
            _lccDatabaseContext.Entry(matchup).State = EntityState.Modified;
        }

        public void DeleteCalculatedMatchupInfo(long gameId)
        {
            Db_LccCachedCalculatedMatchupInfo matchup = 
                _lccDatabaseContext.CalculatedMatchupInformation
                    .Include(x => x.EnemyTeam)
                    .Include(x => x.EnemyTeam.Players)
                    .Include(x => x.FriendlyTeam)
                    .Include(x => x.FriendlyTeam.Players)
                    .ThenInclude(y => y.Trinket)
                    .FirstOrDefault(x => x.GameId == gameId);

            if (matchup != null)
            {
                _lccDatabaseContext.CalculatedMatchupInformation.Remove(matchup);
            }
        }

        public void Save()
        {
            _lccDatabaseContext.SaveChanges();
        }
        
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _lccDatabaseContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
