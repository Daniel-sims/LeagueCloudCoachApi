using LccWebAPI.Models;
using LccWebAPI.Models.APIModels;
using LccWebAPI.Models.DatabaseModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.DatabaseContexts
{
    public class LccDatabaseContext : DbContext
    {
        public LccDatabaseContext(DbContextOptions<LccDatabaseContext> options)
            : base(options)
        { }

        public DbSet<LccSummoner> Summoners { get; set; }
        public DbSet<LccMatchupInformation> Matches { get; set; }
        public DbSet<LccChampionInformation> Champions { get; set; }
        public DbSet<LccItemInformation> Items { get; set; }
        public DbSet<LccSummonerSpellInformation> SummonerSpells { get; set; }
        public DbSet<LccRuneReforged> Runes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LccMatchupInformation>().HasMany(x => x.WinningTeam);
            modelBuilder.Entity<LccMatchupInformation>().HasMany(x => x.LosingTeam);
        }
    }
}
