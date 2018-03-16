using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LccWebAPI.DatabaseContexts;
using LccWebAPI.Repository;
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
            services.AddSingleton<IRiotApi>(RiotApi.GetDevelopmentInstance("RGAPI-d32cf7d0-3419-43b4-b905-7f345db30969"));

            services.AddTransient<ISummonerRepository, SummonerRepository>();
            services.AddTransient<IMatchReferenceRepository, MatchReferenceRepository>();

            var summonerConnection = @"Server=(localdb)\mssqllocaldb;Database=LccSummonerDb;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<SummonerContext>(options => options.UseSqlServer(summonerConnection));

            var matchReferenceConnection = @"Server=(localdb)\mssqllocaldb;Database=LccMatchReferenceDb;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<MatchReferenceContext>(options => options.UseSqlServer(matchReferenceConnection));

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
