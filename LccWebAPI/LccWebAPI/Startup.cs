using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

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
            //services.AddSingleton<IHostedService, MatchDataCollectionService>();
            //services.AddSingleton<ILogging, Logging>();
            //services.AddSingleton<IThrottledRequestHelper, ThrottledRequestHelper>();

            //services.AddSingleton<IRiotApi>(RiotApi.GetDevelopmentInstance("RGAPI-e495fb49-5211-440e-913d-9cd233ca6a45"));
            //services.AddSingleton<IStaticDataEndpoints>(StaticDataEndpoints.GetInstance("RGAPI-e495fb49-5211-440e-913d-9cd233ca6a45"));

            //services.AddTransient<ISummonerRepository, SummonerRepository>();
            //services.AddTransient<IBasicMatchupInformationRepository, BasicMatchupInformationRepository>();
            //services.AddTransient<ICachedCalculatedMatchupInformationRepository, CachedCalculatedMatchupInformationRepository>();
            //services.AddTransient<IChampionStaticDataRepository, ChampionStaticDataRepository>();
            //services.AddTransient<IItemStaticDataRepository, ItemStaticDataRepository>();
            //services.AddTransient<ISummonerSpellStaticDataRepository,SummonerSpellStaticDataRepository>();
            //services.AddTransient<IRunesStaticDataRepository, RunesStaticDataRepository>();
            //services.AddTransient<IMatchControllerUtils, MatchControllerUtils>();


            //var dbConn = @"Server=(localdb)\mssqllocaldb;Database=LccDatabase;Trusted_Connection=True;ConnectRetryCount=0";
            //services.AddDbContext<LccDatabaseContext>(options => options.UseSqlServer(dbConn));

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
            //var staticDataEndpoints = serviceProvider.GetService<IStaticDataEndpoints>();
            //var championRepository = serviceProvider.GetService<IChampionStaticDataRepository>();
            //var summonerSpellRepository = serviceProvider.GetService<ISummonerSpellStaticDataRepository>();
            //var itemRepository = serviceProvider.GetService<IItemStaticDataRepository>();
            //var runeRepository = serviceProvider.GetService<IRunesStaticDataRepository>();

            ////Champions
            //IEnumerable<Db_LccChampion> championsInDatabase = championRepository.GetAllChampions();

            //if (championsInDatabase.Count() == 0)
            //{
            //    ChampionListStatic championsListFromRiot = await staticDataEndpoints.Champion.GetChampionsAsync(Region.euw);

            //    foreach (ChampionStatic champion in championsListFromRiot.Champions.Values)
            //    {
            //        championRepository.InsertChampionInformation(new Db_LccChampion()
            //        {
            //            ChampionId = champion.Id,
            //            ChampionName = champion.Name,
            //            ImageFull = champion.Image.Full
            //        });
            //    }

            //    championRepository.Save();
            //}

            ////Items
            //IEnumerable<Db_LccItem> itemsInDatabase = itemRepository.GetAllItems();

            //if (itemsInDatabase.Count() == 0)
            //{
            //    ItemListStatic itemsListFromRiot = await staticDataEndpoints.Item.GetItemsAsync(Region.euw);

            //    foreach (ItemStatic item in itemsListFromRiot.Items.Values)
            //    {
            //        itemRepository.InsertItem(new Db_LccItem()
            //        {
            //            ItemId = item.Id,
            //            ItemName = item.Name,
            //            ImageFull = item.Image.Full
            //        });
            //    }

            //    itemRepository.Save();
            //}

            ////SummonerSpells
            //IEnumerable<Db_LccSummonerSpell> lccSummonerSpellInformation = summonerSpellRepository.GetAllSummonerSpells();

            //if (lccSummonerSpellInformation.Count() == 0)
            //{
            //    SummonerSpellListStatic summonerSpellListFromRiot = await staticDataEndpoints.SummonerSpell.GetSummonerSpellsAsync(Region.euw);

            //    foreach (SummonerSpellStatic summoner in summonerSpellListFromRiot.SummonerSpells.Values)
            //    {
            //        summonerSpellRepository.InsertSummonerSpell(new Db_LccSummonerSpell()
            //        {
            //            SummonerSpellId = summoner.Id,
            //            SummonerSpellName = summoner.Name,
            //            ImageFull = summoner.Image.Full
            //        });
            //    }

            //    summonerSpellRepository.Save();
            //}

            ////Runes
            //IEnumerable<Db_LccRune> lccRuneInformation = runeRepository.GetAllRunes();

            //if (lccRuneInformation.Count() == 0)
            //{
            //    IList<RuneReforged> runeListFromRiot = await staticDataEndpoints.Rune.GetRunesReforgedAsync(Region.euw);

            //    foreach (RuneReforged rune in runeListFromRiot)
            //    {
            //        runeRepository.InsertRune(new Db_LccRune()
            //        {
            //            RuneId = rune.Id,
            //            RuneName = rune.Name,
            //            RunePathName = rune.RunePathName,
            //            Key = rune.Key,
            //            ShortDesc = rune.ShortDesc,
            //            LongDesc = rune.LongDesc,
            //            Icon = rune.Icon
            //        });
            //    }

            //    runeRepository.Save();
            //}
        }
    }
}
