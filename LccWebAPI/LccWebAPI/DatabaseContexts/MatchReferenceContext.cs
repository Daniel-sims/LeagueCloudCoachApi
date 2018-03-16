using LccWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.DatabaseContexts
{
    public class MatchReferenceContext : DbContext
    {
        public MatchReferenceContext(DbContextOptions<MatchReferenceContext> options)
            : base(options)
        { }

        public DbSet<LccMatchReference> MatchReferences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LccMatchReference>().HasKey(c => c.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
