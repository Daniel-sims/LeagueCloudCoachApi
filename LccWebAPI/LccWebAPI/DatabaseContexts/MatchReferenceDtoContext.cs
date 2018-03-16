using LccWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using RiotSharp.MatchEndpoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.DatabaseContexts
{
    public class MatchReferenceDtoContext : DbContext
    {
        public MatchReferenceDtoContext(DbContextOptions<MatchReferenceDtoContext> options)
            : base(options)
        { }

        public DbSet<MatchReferenceDto> MatchReferences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MatchReferenceDto>().HasKey(c => c.Id);
            base.OnModelCreating(modelBuilder);
        }
    }
}
