using LccWebAPI.Database.Context;
using LccWebAPI.Database.Repository.Interface.Summoner;
using LccWebAPI.Database.Repository.Summoner;
using LccWebAPI.Repository.Interfaces.Match;
using LccWebAPI.Repository.Interfaces.StaticData;
using LccWebAPI.Repository.Match;
using LccWebAPI.Repository.StaticData;
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

            services.AddSingleton<IRiotApi>(RiotApi.GetDevelopmentInstance("RGAPI-99c7b7a6-b658-4a1b-b7e6-9df53ed89b3c"));
            services.AddSingleton<IStaticDataEndpoints>(StaticDataEndpoints.GetInstance("RGAPI-99c7b7a6-b658-4a1b-b7e6-9df53ed89b3c"));

            services.AddTransient<ISummonerRepository, SummonerRepository>();
            services.AddTransient<IBasicMatchupInformationRepository, BasicMatchupInformationRepository>();
            services.AddTransient<IChampionStaticDataRepository, ChampionStaticDataRepository>();
            services.AddTransient<IItemStaticDataRepository, ItemStaticDataRepository>();
            services.AddTransient<ISummonerSpellStaticDataRepository,SummonerSpellStaticDataRepository>();
            services.AddTransient<IRunesStaticDataRepository, RunesStaticDataRepository>();
            
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
