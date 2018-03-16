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
    [DbContext(typeof(SummonerContext))]
    [Migration("20180316170819_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LccWebAPI.Models.LccSummoner", b =>
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
#pragma warning restore 612, 618
        }
    }
}
