﻿// <auto-generated />
using LccWebAPI.DatabaseContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using RiotSharp.Misc;
using System;

namespace LccWebAPI.Migrations
{
    [DbContext(typeof(LccDatabaseContext))]
    partial class LccDatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LccWebAPI.Models.APIModels.LccRuneReforged", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Icon");

                    b.Property<string>("Key");

                    b.Property<string>("LongDesc");

                    b.Property<string>("Name");

                    b.Property<int>("RuneId");

                    b.Property<int>("RunePathId");

                    b.Property<string>("RunePathName");

                    b.Property<string>("ShortDesc");

                    b.HasKey("Id");

                    b.ToTable("Runes");
                });

            modelBuilder.Entity("LccWebAPI.Models.DatabaseModels.LccChampionInformation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChampionId");

                    b.Property<string>("ChampionName");

                    b.HasKey("Id");

                    b.ToTable("Champions");
                });

            modelBuilder.Entity("LccWebAPI.Models.DatabaseModels.LccItemInformation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ItemId");

                    b.Property<string>("ItemName");

                    b.HasKey("Id");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("LccWebAPI.Models.DatabaseModels.LccMatchupInformation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("GameId");

                    b.Property<DateTime>("MatchDate");

                    b.HasKey("Id");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("LccWebAPI.Models.DatabaseModels.LccMatchupInformationPlayer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("AccountId");

                    b.Property<long>("ChampionId");

                    b.Property<string>("Lane");

                    b.Property<int?>("LccMatchupInformationId");

                    b.Property<int?>("LccMatchupInformationId1");

                    b.Property<string>("SummonerName");

                    b.HasKey("Id");

                    b.HasIndex("LccMatchupInformationId");

                    b.HasIndex("LccMatchupInformationId1");

                    b.ToTable("LccMatchupInformationPlayer");
                });

            modelBuilder.Entity("LccWebAPI.Models.DatabaseModels.LccSummoner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("AccountId");

                    b.Property<DateTime>("LastUpdated");

                    b.Property<long>("Level");

                    b.Property<string>("Name");

                    b.Property<int>("ProfileIconId");

                    b.Property<int>("Region");

                    b.Property<DateTime>("RevisionDate");

                    b.HasKey("Id");

                    b.ToTable("Summoners");
                });

            modelBuilder.Entity("LccWebAPI.Models.DatabaseModels.LccSummonerSpellInformation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("SummonerSpellId");

                    b.Property<string>("SummonerSpellName");

                    b.HasKey("Id");

                    b.ToTable("SummonerSpells");
                });

            modelBuilder.Entity("LccWebAPI.Models.DatabaseModels.LccMatchupInformationPlayer", b =>
                {
                    b.HasOne("LccWebAPI.Models.DatabaseModels.LccMatchupInformation")
                        .WithMany("LosingTeam")
                        .HasForeignKey("LccMatchupInformationId");

                    b.HasOne("LccWebAPI.Models.DatabaseModels.LccMatchupInformation")
                        .WithMany("WinningTeam")
                        .HasForeignKey("LccMatchupInformationId1");
                });
#pragma warning restore 612, 618
        }
    }
}
