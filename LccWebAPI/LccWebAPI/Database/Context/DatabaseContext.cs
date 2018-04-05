using Microsoft.EntityFrameworkCore;

namespace LccWebAPI.Database.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        
        //Match information 
        public DbSet<Models.DbMatch.Match> Matches { get; set; }
        // Includes to get full tree
        // .Include(x => x.Teams).ThenInclude(y => y.Players).ThenInclude(x => x.Runes)
        // .Include(x => x.Teams).ThenInclude(y => y.Players).ThenInclude(x => x.Items)
        // .Include(x => x.Teams).ThenInclude(y => y.Players).ThenInclude(x => x.SummonerSpells)

        //Summoner information
        public DbSet<Models.DbSummoner.Summoner> Summoners { get; set; }

        //Static data
        public DbSet<Models.DbStaticData.Rune> Runes { get; set; }
        public DbSet<Models.DbStaticData.Item> Items { get; set; }
        public DbSet<Models.DbStaticData.Champion> Champions { get; set; }
        public DbSet<Models.DbStaticData.SummonerSpell> SummonerSpells { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
    }
}