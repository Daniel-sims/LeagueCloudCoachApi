using LccWebAPI.Database.Context;
using LccWebAPI.Database.Models.StaticData;
using LccWebAPI.Repository.StaticData.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LccWebAPI.Repository.StaticData
{
    public class ChampionStaticDataRepository : IChampionStaticDataRepository
    {
        private LccDatabaseContext _lccDatabaseContext;

        public ChampionStaticDataRepository(LccDatabaseContext lccDatabaseContext)
        {
            _lccDatabaseContext = lccDatabaseContext;
        }

        public void InsertChampionInformation(Db_LccChampion champion)
        {
            _lccDatabaseContext.Champions.Add(champion);
        }

        public IEnumerable<Db_LccChampion> GetAllChampions()
        {
            return _lccDatabaseContext.Champions.ToList();
        }

        public Db_LccChampion GetChampionById(int championId)
        {
            return _lccDatabaseContext.Champions.FirstOrDefault(x => x.ChampionId == championId);
        }

        public void UpdateChampion(Db_LccChampion champion)
        {
            _lccDatabaseContext.Entry(champion).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void DeleteChampion(long championId)
        {
            Db_LccChampion champion = _lccDatabaseContext.Champions.FirstOrDefault(x => x.ChampionId == championId);
            if (champion != null)
            {
                _lccDatabaseContext.Champions.Remove(champion);
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
