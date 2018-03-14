using LccWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.DatabaseContexts
{
    public class SummonerDtoContext : DbContext
    {
        public SummonerDtoContext(DbContextOptions<SummonerDtoContext> options)
            : base(options)
        { }

        public DbSet<SummonerDto> Summoners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SummonerDto>()
                .HasKey(e => e.Id)
                .HasName("PK_Key");
        }
    }
}
