﻿// <auto-generated />
using LccWebAPI.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace LccWebAPI.Migrations
{
    [DbContext(typeof(LccDatabaseContext))]
    [Migration("20180331092918_addedMatchDuration")]
    partial class addedMatchDuration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LccWebAPI.Database.Models.Match.Db_LccBasicMatchInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("GameId");

                    b.Property<DateTime>("MatchDate");

                    b.HasKey("Id");

                    b.ToTable("Matchups");
                });

            modelBuilder.Entity("LccWebAPI.Database.Models.Match.Db_LccBasicMatchInfoPlayer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChampionId");

                    b.Property<int?>("Db_LccBasicMatchInfoId");

                    b.Property<int?>("Db_LccBasicMatchInfoId1");

                    b.Property<string>("Lane");

                    b.Property<long>("PlayerAccountId");

                    b.Property<string>("SummonerName");

                    b.HasKey("Id");

                    b.HasIndex("Db_LccBasicMatchInfoId");

                    b.HasIndex("Db_LccBasicMatchInfoId1");

                    b.ToTable("Db_LccBasicMatchInfoPlayer");
                });

            modelBuilder.Entity("LccWebAPI.Database.Models.Match.Db_LccCachedCalculatedMatchupInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("EnemyTeamId");

                    b.Property<int?>("FriendlyTeamId");

                    b.Property<bool>("FriendlyTeamWin");

                    b.Property<long>("GameId");

                    b.Property<DateTime>("MatchDate");

                    b.Property<TimeSpan>("MatchDuration");

                    b.Property<string>("MatchPatch");

                    b.HasKey("Id");

                    b.HasIndex("EnemyTeamId");

                    b.HasIndex("FriendlyTeamId");

                    b.ToTable("CalculatedMatchupInformation");
                });

            modelBuilder.Entity("LccWebAPI.Database.Models.Match.Db_LccCachedPlayerStats", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Assists");

                    b.Property<int?>("ChampionId");

                    b.Property<long>("ChampionLevel");

                    b.Property<int?>("Db_LccCachedTeamInformationId");

                    b.Property<long>("Deaths");

                    b.Property<int?>("ItemFiveId");

                    b.Property<int?>("ItemFourId");

                    b.Property<int?>("ItemOneId");

                    b.Property<int?>("ItemSixId");

                    b.Property<int?>("ItemThreeId");

                    b.Property<int?>("ItemTwoId");

                    b.Property<long>("Kills");

                    b.Property<long>("MinionKills");

                    b.Property<int?>("PrimaryRuneStyleId");

                    b.Property<int?>("PrimaryRuneSubFourId");

                    b.Property<int?>("PrimaryRuneSubOneId");

                    b.Property<int?>("PrimaryRuneSubThreeId");

                    b.Property<int?>("PrimaryRuneSubTwoId");

                    b.Property<string>("RankedSoloDivision");

                    b.Property<string>("RankedSoloLeaguePoints");

                    b.Property<long>("RankedSoloLosses");

                    b.Property<string>("RankedSoloTier");

                    b.Property<long>("RankedSoloWins");

                    b.Property<int?>("SecondaryRuneStyleId");

                    b.Property<int?>("SecondaryRuneSubOneId");

                    b.Property<int?>("SecondaryRuneSubTwoId");

                    b.Property<long>("SummonerId");

                    b.Property<string>("SummonerName");

                    b.Property<int?>("SummonerOneId");

                    b.Property<int?>("SummonerTwoId");

                    b.Property<int?>("TrinketId");

                    b.HasKey("Id");

                    b.HasIndex("ChampionId");

                    b.HasIndex("Db_LccCachedTeamInformationId");

                    b.HasIndex("ItemFiveId");

                    b.HasIndex("ItemFourId");

                    b.HasIndex("ItemOneId");

                    b.HasIndex("ItemSixId");

                    b.HasIndex("ItemThreeId");

                    b.HasIndex("ItemTwoId");

                    b.HasIndex("PrimaryRuneStyleId");

                    b.HasIndex("PrimaryRuneSubFourId");

                    b.HasIndex("PrimaryRuneSubOneId");

                    b.HasIndex("PrimaryRuneSubThreeId");

                    b.HasIndex("PrimaryRuneSubTwoId");

                    b.HasIndex("SecondaryRuneStyleId");

                    b.HasIndex("SecondaryRuneSubOneId");

                    b.HasIndex("SecondaryRuneSubTwoId");

                    b.HasIndex("SummonerOneId");

                    b.HasIndex("SummonerTwoId");

                    b.HasIndex("TrinketId");

                    b.ToTable("Db_LccCachedPlayerStats");
                });

            modelBuilder.Entity("LccWebAPI.Database.Models.Match.Db_LccCachedTeamInformation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("BaronKills");

                    b.Property<long>("DragonKills");

                    b.Property<long>("InhibitorKills");

                    b.Property<long>("RiftHeraldKills");

                    b.Property<long>("TotalAssists");

                    b.Property<long>("TotalDeaths");

                    b.Property<long>("TotalKills");

                    b.HasKey("Id");

                    b.ToTable("Db_LccCachedTeamInformation");
                });

            modelBuilder.Entity("LccWebAPI.Database.Models.StaticData.Db_LccChampion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChampionId");

                    b.Property<string>("ChampionName");

                    b.Property<string>("ImageFull");

                    b.HasKey("Id");

                    b.ToTable("Champions");
                });

            modelBuilder.Entity("LccWebAPI.Database.Models.StaticData.Db_LccItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ImageFull");

                    b.Property<int>("ItemId");

                    b.Property<string>("ItemName");

                    b.HasKey("Id");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("LccWebAPI.Database.Models.StaticData.Db_LccRune", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Icon");

                    b.Property<string>("Key");

                    b.Property<string>("LongDesc");

                    b.Property<int>("RuneId");

                    b.Property<string>("RuneName");

                    b.Property<string>("RunePathName");

                    b.Property<string>("ShortDesc");

                    b.HasKey("Id");

                    b.ToTable("Runes");
                });

            modelBuilder.Entity("LccWebAPI.Database.Models.StaticData.Db_LccSummonerSpell", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ImageFull");

                    b.Property<int>("SummonerSpellId");

                    b.Property<string>("SummonerSpellName");

                    b.HasKey("Id");

                    b.ToTable("SummonerSpells");
                });

            modelBuilder.Entity("LccWebAPI.Database.Models.Summoner.Db_LccSummoner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("AccountId");

                    b.Property<DateTime>("LastUpdatedTime");

                    b.Property<string>("SummonerName");

                    b.HasKey("Id");

                    b.ToTable("Summoners");
                });

            modelBuilder.Entity("LccWebAPI.Database.Models.Match.Db_LccBasicMatchInfoPlayer", b =>
                {
                    b.HasOne("LccWebAPI.Database.Models.Match.Db_LccBasicMatchInfo")
                        .WithMany("LosingTeamChampions")
                        .HasForeignKey("Db_LccBasicMatchInfoId");

                    b.HasOne("LccWebAPI.Database.Models.Match.Db_LccBasicMatchInfo")
                        .WithMany("WinningTeamChampions")
                        .HasForeignKey("Db_LccBasicMatchInfoId1");
                });

            modelBuilder.Entity("LccWebAPI.Database.Models.Match.Db_LccCachedCalculatedMatchupInfo", b =>
                {
                    b.HasOne("LccWebAPI.Database.Models.Match.Db_LccCachedTeamInformation", "EnemyTeam")
                        .WithMany()
                        .HasForeignKey("EnemyTeamId");

                    b.HasOne("LccWebAPI.Database.Models.Match.Db_LccCachedTeamInformation", "FriendlyTeam")
                        .WithMany()
                        .HasForeignKey("FriendlyTeamId");
                });

            modelBuilder.Entity("LccWebAPI.Database.Models.Match.Db_LccCachedPlayerStats", b =>
                {
                    b.HasOne("LccWebAPI.Database.Models.StaticData.Db_LccChampion", "Champion")
                        .WithMany()
                        .HasForeignKey("ChampionId");

                    b.HasOne("LccWebAPI.Database.Models.Match.Db_LccCachedTeamInformation")
                        .WithMany("Players")
                        .HasForeignKey("Db_LccCachedTeamInformationId");

                    b.HasOne("LccWebAPI.Database.Models.StaticData.Db_LccItem", "ItemFive")
                        .WithMany()
                        .HasForeignKey("ItemFiveId");

                    b.HasOne("LccWebAPI.Database.Models.StaticData.Db_LccItem", "ItemFour")
                        .WithMany()
                        .HasForeignKey("ItemFourId");

                    b.HasOne("LccWebAPI.Database.Models.StaticData.Db_LccItem", "ItemOne")
                        .WithMany()
                        .HasForeignKey("ItemOneId");

                    b.HasOne("LccWebAPI.Database.Models.StaticData.Db_LccItem", "ItemSix")
                        .WithMany()
                        .HasForeignKey("ItemSixId");

                    b.HasOne("LccWebAPI.Database.Models.StaticData.Db_LccItem", "ItemThree")
                        .WithMany()
                        .HasForeignKey("ItemThreeId");

                    b.HasOne("LccWebAPI.Database.Models.StaticData.Db_LccItem", "ItemTwo")
                        .WithMany()
                        .HasForeignKey("ItemTwoId");

                    b.HasOne("LccWebAPI.Database.Models.StaticData.Db_LccRune", "PrimaryRuneStyle")
                        .WithMany()
                        .HasForeignKey("PrimaryRuneStyleId");

                    b.HasOne("LccWebAPI.Database.Models.StaticData.Db_LccRune", "PrimaryRuneSubFour")
                        .WithMany()
                        .HasForeignKey("PrimaryRuneSubFourId");

                    b.HasOne("LccWebAPI.Database.Models.StaticData.Db_LccRune", "PrimaryRuneSubOne")
                        .WithMany()
                        .HasForeignKey("PrimaryRuneSubOneId");

                    b.HasOne("LccWebAPI.Database.Models.StaticData.Db_LccRune", "PrimaryRuneSubThree")
                        .WithMany()
                        .HasForeignKey("PrimaryRuneSubThreeId");

                    b.HasOne("LccWebAPI.Database.Models.StaticData.Db_LccRune", "PrimaryRuneSubTwo")
                        .WithMany()
                        .HasForeignKey("PrimaryRuneSubTwoId");

                    b.HasOne("LccWebAPI.Database.Models.StaticData.Db_LccRune", "SecondaryRuneStyle")
                        .WithMany()
                        .HasForeignKey("SecondaryRuneStyleId");

                    b.HasOne("LccWebAPI.Database.Models.StaticData.Db_LccRune", "SecondaryRuneSubOne")
                        .WithMany()
                        .HasForeignKey("SecondaryRuneSubOneId");

                    b.HasOne("LccWebAPI.Database.Models.StaticData.Db_LccRune", "SecondaryRuneSubTwo")
                        .WithMany()
                        .HasForeignKey("SecondaryRuneSubTwoId");

                    b.HasOne("LccWebAPI.Database.Models.StaticData.Db_LccSummonerSpell", "SummonerOne")
                        .WithMany()
                        .HasForeignKey("SummonerOneId");

                    b.HasOne("LccWebAPI.Database.Models.StaticData.Db_LccSummonerSpell", "SummonerTwo")
                        .WithMany()
                        .HasForeignKey("SummonerTwoId");

                    b.HasOne("LccWebAPI.Database.Models.StaticData.Db_LccItem", "Trinket")
                        .WithMany()
                        .HasForeignKey("TrinketId");
                });
#pragma warning restore 612, 618
        }
    }
}
