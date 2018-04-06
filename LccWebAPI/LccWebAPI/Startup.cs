using LccWebAPI.Controllers.Utils.Match;
using LccWebAPI.Database.Context;
using LccWebAPI.Models.DbStaticData;
using LccWebAPI.Services;
using LccWebAPI.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RiotSharp;
using RiotSharp.Endpoints.Interfaces.Static;
using RiotSharp.Endpoints.StaticDataEndpoint;
using RiotSharp.Interfaces;
using RiotSharp.Misc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI
{
    public class Startup
    {
        private const string RiotApiKey = "RGAPI-d146063c-9aa6-4ed2-95b9-9656e0599351";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHostedService, MatchDataCollectionService>();
            services.AddSingleton<ILogging, Logging>();
            services.AddSingleton<IThrottledRequestHelper, ThrottledRequestHelper>();

            services.AddSingleton<IRiotApi>(RiotApi.GetDevelopmentInstance(RiotApiKey));
            services.AddSingleton<IStaticDataEndpoints>(StaticDataEndpoints.GetInstance(RiotApiKey));

            services.AddTransient<IMatchProvider, MatchProvider>();
            
            var dbConn = @"Server=(localdb)\mssqllocaldb;Database=LccDatabase;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(dbConn));

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            CollectStaticData(serviceProvider).GetAwaiter().GetResult();

            app.UseMvc();
        }

        private async Task CollectStaticData(IServiceProvider serviceProvider)
        {
            var staticDataEndpoint = serviceProvider.GetRequiredService<IStaticDataEndpoints>();

            using (var dbContext = serviceProvider.GetRequiredService<DatabaseContext>())
            {
                if (!dbContext.Items.Any())
                {
                    var riotItemInformation = await staticDataEndpoint.Item.GetItemsAsync(Region.euw);

                    foreach (var riotItem in riotItemInformation.Items)
                    {
                        dbContext.Items.Add(new Item()
                        {
                            ItemId = riotItem.Value.Id,
                            ItemName = riotItem.Value.Name,
                            ImageFull = riotItem.Value.Image.Full
                        });
                    }

                    dbContext.SaveChanges();
                }

                if (!dbContext.Runes.Any())
                {
                    var riotRuneInformation = await staticDataEndpoint.Rune.GetRunesReforgedAsync(Region.euw);

                    foreach (var riotRune in riotRuneInformation)
                    {
                        dbContext.Runes.Add(new Rune()
                        {
                            RuneId = riotRune.Id,
                            RuneName = riotRune.Name,
                        });
                    }

                    dbContext.SaveChanges();
                }

                if (!dbContext.SummonerSpells.Any())
                {
                    var riotSummonerSpellInformation = await staticDataEndpoint.SummonerSpell.GetSummonerSpellsAsync(Region.euw);

                    foreach (var riotSummonerSpell in riotSummonerSpellInformation.SummonerSpells)
                    {
                        dbContext.SummonerSpells.Add(new SummonerSpell()
                        {
                            SummonerSpellId = riotSummonerSpell.Value.Id,
                            SummonerSpellName = riotSummonerSpell.Value.Name,
                            ImageFull = riotSummonerSpell.Value.Image.Full
                        });
                    }

                    dbContext.SaveChanges();
                }

                if (!dbContext.Champions.Any())
                {
                    var riotChampionInformation = await staticDataEndpoint.Champion.GetChampionsAsync(Region.euw);

                    foreach (var riotChampion in riotChampionInformation.Champions)
                    {
                        dbContext.Champions.Add(new Champion()
                        {
                            ChampionId = riotChampion.Value.Id,
                            ChampionName = riotChampion.Value.Name,
                            ImageFull = riotChampion.Value.Image.Full
                        });
                    }

                    dbContext.SaveChanges();
                }

                
            }
        }
    }
}
