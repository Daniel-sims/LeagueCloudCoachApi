using Microsoft.EntityFrameworkCore;

namespace LccWebAPI.Database.Context
{
    public class LccDatabaseContext : DbContext
    {
        public LccDatabaseContext(DbContextOptions<LccDatabaseContext> options) : base(options) { }
        
        public DbSet<Models.Db.Match> Matches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
    }
}