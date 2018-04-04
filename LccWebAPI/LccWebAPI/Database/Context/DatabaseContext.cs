using Microsoft.EntityFrameworkCore;

namespace LccWebAPI.Database.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        
        public DbSet<Models.Db.Match> Matches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
    }
}