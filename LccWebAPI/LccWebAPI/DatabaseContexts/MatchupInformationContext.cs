using LccWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using RiotSharp.MatchEndpoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.DatabaseContexts
{
    public class MatchupInformationContext : DbContext
    {
        public MatchupInformationContext(DbContextOptions<MatchupInformationContext> options)
            : base(options)
        { }

        public DbSet<LccMatchupInformation> Matches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LccMatchupInformation>().HasKey(c => c.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
