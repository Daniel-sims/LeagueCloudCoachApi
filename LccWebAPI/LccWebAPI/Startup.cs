using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LccWebAPI.DatabaseContexts;
using LccWebAPI.Repository.Summoner;
using LccWebAPI.Repository.Match;
using LccWebAPI.Services;
using LccWebAPI.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RiotSharp;
using RiotSharp.Interfaces;
using LccWebAPI.Repository.StaticData;

namespace LccWebAPI
{
    public class Startup
    {
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

            services.AddSingleton<IRiotApi>(RiotApi.GetDevelopmentInstance("RGAPI-41867b58-78da-4c5f-b227-2f9033fcf293"));
            services.AddSingleton<IStaticRiotApi>(StaticRiotApi.GetInstance("RGAPI-41867b58-78da-4c5f-b227-2f9033fcf293"));

            services.AddTransient<ISummonerRepository, SummonerRepository>();
            services.AddTransient<IMatchupInformationRepository, MatchupInformationRepository>();
            services.AddTransient<IChampionStaticDataRepository, ChampionStaticDataRepository>();
            services.AddTransient<IItemStaticDataRepository, ItemStaticDataRepository>();
            services.AddTransient<ISummonerSpellStaticDataRepository, SummonerSpellStaticDataRepository>();

            var dbConn = @"Server=(localdb)\mssqllocaldb;Database=LccDb;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<LccDatabaseContext>(options => options.UseSqlServer(dbConn));

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
