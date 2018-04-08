using Microsoft.EntityFrameworkCore;

namespace LccWebAPI.Database.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        
        //Match information 
        public DbSet<Models.Match.Match> Matches { get; set; }
        // Includes to get full tree
        // .Include(x => x.Teams).ThenInclude(y => y.Players).ThenInclude(x => x.Runes)
        // .Include(x => x.Teams).ThenInclude(y => y.Players).ThenInclude(x => x.Items)
        // .Include(x => x.Teams).ThenInclude(y => y.Players).ThenInclude(x => x.SummonerSpells)

        //Summoner information
        public DbSet<Models.Summoner.Summoner> Summoners { get; set; }

        //Static data
        public DbSet<Models.StaticData.Rune> Runes { get; set; }
        public DbSet<Models.StaticData.Item> Items { get; set; }
        public DbSet<Models.StaticData.Champion> Champions { get; set; }
        public DbSet<Models.StaticData.SummonerSpell> SummonerSpells { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
    }
}