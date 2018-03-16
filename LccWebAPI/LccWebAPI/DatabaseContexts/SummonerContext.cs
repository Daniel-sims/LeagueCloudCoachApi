using LccWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using RiotSharp.SummonerEndpoint;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.DatabaseContexts
{
    public class SummonerContext : DbContext
    {
        public SummonerContext(DbContextOptions<SummonerContext> options)
            : base(options)
        { }

        public DbSet<LccSummoner> Summoners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LccSummoner>().HasKey(c => c.Id);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
