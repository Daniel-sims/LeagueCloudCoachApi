using LccWebAPI.Models.Match;
using LccWebAPI.Models.StaticData;
using LccWebAPI.Models.Summoner;
using Microsoft.EntityFrameworkCore;

namespace LccWebAPI.Database.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        
        //Match information 
        public DbSet<Match> Matches { get; set; }
        public DbSet<MatchTeam> MatchTeam { get; set; }
        public DbSet<MatchPlayer> MatchPlayer { get; set; }

        //Events that relate to the above matches via gameId
        public DbSet<MatchTimeline> MatchTimelines { get; set; }

        //Summoner information
        public DbSet<Summoner> Summoners { get; set; }

        //Static data
        public DbSet<Rune> Runes { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Champion> Champions { get; set; }
        public DbSet<SummonerSpell> SummonerSpells { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
    }
}