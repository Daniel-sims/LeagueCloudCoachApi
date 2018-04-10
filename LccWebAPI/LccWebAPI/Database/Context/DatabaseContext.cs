using Microsoft.EntityFrameworkCore;

namespace LccWebAPI.Database.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        
        //Match information 
        public DbSet<Models.Match.Match> Matches { get; set; }
        public DbSet<Models.Match.MatchTeam> MatchTeam { get; set; }
        public DbSet<Models.Match.MatchPlayer> MatchPlayer { get; set; }

        //Events that relate to the above matches via gameId
        public DbSet<Models.Match.MatchTimeline> MatchTimelines { get; set; }

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