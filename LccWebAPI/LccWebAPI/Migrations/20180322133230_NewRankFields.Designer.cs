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
    [Migration("20180322133230_NewRankFields")]
    partial class NewRankFields
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LccWebAPI.Models.DatabaseModels.LccMatchupInformation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("GameId");

                    b.HasKey("Id");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("LccWebAPI.Models.DatabaseModels.LccMatchupInformationPlayer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("AccountId");

                    b.Property<long>("ChampionId");

                    b.Property<string>("CurrentLp");

                    b.Property<string>("CurrentRank");

                    b.Property<string>("Lane");

                    b.Property<int?>("LccMatchupInformationId");

                    b.Property<int?>("LccMatchupInformationId1");

                    b.Property<string>("Losses");

                    b.Property<string>("SummonerName");

                    b.Property<string>("Wins");

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