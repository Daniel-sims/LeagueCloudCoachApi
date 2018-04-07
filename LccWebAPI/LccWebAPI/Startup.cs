﻿using LccWebAPI.Controllers.Utils.Match;
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
            services.AddScoped<IStaticDataCollectionService, StaticDataCollectionService>();

            services.AddSingleton<IRiotApi>(RiotApi.GetDevelopmentInstance(RiotApiKey));
            services.AddSingleton<IStaticDataEndpoints>(StaticDataEndpoints.GetInstance(RiotApiKey));

            services.AddScoped<IMatchProvider, MatchProvider>();
            
            var dbConn = @"Server=(localdb)\mssqllocaldb;Database=LccDatabase;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(dbConn));

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IStaticDataCollectionService staticDataCollectionService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            staticDataCollectionService.CollectStaticDataIfNeeded().GetAwaiter().GetResult();

            app.UseMvc();
        }
    }
}
