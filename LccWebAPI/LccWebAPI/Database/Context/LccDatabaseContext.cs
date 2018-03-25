using LccWebAPI.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Database.Context
{
    public class LccDatabaseContext : DbContext
    {
        public LccDatabaseContext(DbContextOptions<LccDatabaseContext> options) : base(options) { }
        
        //Basic information stored for quick match lookup
        public DbSet<Db_LccBasicMatchInfo> Matches { get; set; }

        //Static Data
        public DbSet<Db_LccChampion> Champions { get; set; }
        public DbSet<Db_LccItem> Items { get; set; }
        public DbSet<Db_LccRune> Runes { get; set; }
        public DbSet<Db_LccSummonerSpell> SummonerSpells { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}