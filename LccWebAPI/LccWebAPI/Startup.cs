using LccWebAPI.Authentication;
using LccWebAPI.Controllers.Utils.Match;
using LccWebAPI.Database.Context;
using LccWebAPI.Models.Authentication;
using LccWebAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
        private const string RiotApiKey = "RGAPI-3cd3339a-60f4-4179-ad76-e00d491442c5";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHostedService, MatchDataCollectionService>();
            services.AddSingleton<IThrottledRequestHelper, ThrottledRequestHelper>();
            services.AddScoped<IStaticDataCollectionService, StaticDataCollectionService>();

            services.AddSingleton<IRiotApi>(RiotApi.GetDevelopmentInstance(RiotApiKey));
            services.AddSingleton<IStaticDataEndpoints>(StaticDataEndpoints.GetInstance(RiotApiKey));

            services.AddScoped<IMatchProvider, MatchProvider>();
            
            var dbConn = @"Server=(localdb)\mssqllocaldb;Database=LccDatabase;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(dbConn));

            var identityDbCon = @"Server=(localdb)\mssqllocaldb;Database=IdentityServerDatabase;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(identityDbCon));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryPersistedGrants()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddAspNetIdentity<ApplicationUser>();
            
            services.AddMvc();

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    // base-address of your identityserver
                    options.Authority = "http://localhost:54547/";

                    // name of the API resource
                    options.Audience = "LccApi";

                    options.RequireHttpsMetadata = false;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IStaticDataCollectionService staticDataCollectionService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            staticDataCollectionService.CollectStaticDataIfNeeded().GetAwaiter().GetResult();

            app.UseMvc();
        }
    }
}
