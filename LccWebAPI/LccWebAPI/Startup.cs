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
using LccWebAPI.Repository.StaticData.Interfaces;
using LccWebAPI.Repository.Match.Interfaces;

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

            services.AddSingleton<IRiotApi>(RiotApi.GetDevelopmentInstance("RGAPI-495503df-5078-4696-8355-4ea78620bd57"));

            services.AddTransient<REPLACED_ISummonerRepository, REPLACED_SummonerRepository>();
            services.AddTransient<REPLACED_IMatchupInformationRepository, REPLACED_MatchupInformationRepository>();
            services.AddTransient<REPLACED_IChampionStaticDataRepository, REPLACED_ChampionStaticDataRepository>();
            services.AddTransient<REPLACED_IItemStaticDataRepository, REPLACED_ItemStaticDataRepository>();
            services.AddTransient<REPLACED_ISummonerSpellStaticDataRepository, REPLACED_SummonerSpellStaticDataRepository>();
            services.AddTransient<REPLACED_IRunesReforgedStaticDataRepository, REPLACED_RunesReforgedStaticDataRepository>();


            var dbConn = @"Server=(localdb)\mssqllocaldb;Database=LccDb;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<REPLACED_LccDatabaseContext>(options => options.UseSqlServer(dbConn));

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
