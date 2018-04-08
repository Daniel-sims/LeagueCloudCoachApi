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
    [DbContext(typeof(DatabaseContext))]
    [Migration("20180408115208_ChangedMatchPlayerToHoldIdsNotObjects")]
    partial class ChangedMatchPlayerToHoldIdsNotObjects
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LccWebAPI.Models.Match.Match", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("GameDate");

                    b.Property<TimeSpan>("GameDuration");

                    b.Property<long>("GameId");

                    b.Property<string>("GamePatch");

                    b.Property<int?>("WinningTeamId");

                    b.HasKey("Id");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("LccWebAPI.Models.Match.MatchPlayer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Assists");

                    b.Property<int>("ChampionId");

                    b.Property<long>("ChampionLevel");

                    b.Property<long>("DamageDealtToObjectives");

                    b.Property<long>("DamageDealtToTurrets");

                    b.Property<long>("Deaths");

                    b.Property<long>("DoubleKills");

                    b.Property<bool>("FirstBloodAssist");

                    b.Property<bool>("FirstBloodKill");

                    b.Property<bool>("FirstInhibitorAssist");

                    b.Property<bool>("FirstInhibitorKill");

                    b.Property<bool>("FirstTowerAssist");

                    b.Property<bool>("FirstTowerKill");

                    b.Property<long>("GoldEarned");

                    b.Property<long>("GoldSpent");

                    b.Property<string>("HighestAcheivedTierLastSeason");

                    b.Property<long>("InhibitorKills");

                    b.Property<long>("Item1Id");

                    b.Property<long>("Item2Id");

                    b.Property<long>("Item3Id");

                    b.Property<long>("Item4Id");

                    b.Property<long>("Item5Id");

                    b.Property<long>("Item6Id");

                    b.Property<long>("KillingSprees");

                    b.Property<long>("Kills");

                    b.Property<long>("LargestCriticalStrike");

                    b.Property<long>("LargestKillingSpree");

                    b.Property<long>("LargestMultiKill");

                    b.Property<long>("MagicDamageDealt");

                    b.Property<long>("MagicDamageDealtToChampions");

                    b.Property<long>("MagicDamageTaken");

                    b.Property<long>("MatchTeamId");

                    b.Property<int?>("MatchTeamId1");

                    b.Property<long>("NeutralMinionsKilled");

                    b.Property<long>("NeutralMinionsKilledEnemyJungle");

                    b.Property<long>("NeutralMinionsKilledTeamJungle");

                    b.Property<long>("ObjectivePlayerScore");

                    b.Property<int>("ParticipantId");

                    b.Property<long>("PentaKills");

                    b.Property<long>("PhysicalDamageDealt");

                    b.Property<long>("PhysicalDamageDealtToChampions");

                    b.Property<long>("PhysicalDamageTaken");

                    b.Property<long>("PlayerId");

                    b.Property<long>("PrimaryRuneStyleId");

                    b.Property<long>("PrimaryRuneSubStyleFourId");

                    b.Property<long>("PrimaryRuneSubStyleOneId");

                    b.Property<long>("PrimaryRuneSubStyleThreeId");

                    b.Property<long>("PrimaryRuneSubStyleTwoId");

                    b.Property<long>("QuadraKills");

                    b.Property<long>("SecondaryRuneStyleId");

                    b.Property<long>("SecondaryRuneSubStyleOneId");

                    b.Property<long>("SecondaryRuneSubStyleTwoId");

                    b.Property<long>("SightWardsBoughtInGame");

                    b.Property<long>("SummonerSpellOneId");

                    b.Property<long>("SummonerSpellTwoId");

                    b.Property<int>("TeamId");

                    b.Property<long>("TimeCCingOthers");

                    b.Property<long>("TotalDamageDealt");

                    b.Property<long>("TotalDamageDealtToChampions");

                    b.Property<long>("TotalDamageTaken");

                    b.Property<long>("TotalHeal");

                    b.Property<long>("TotalMinionsKilled");

                    b.Property<long>("TotalScoreRank");

                    b.Property<long>("TotalTimeCrowdControlDealt");

                    b.Property<long>("TotalUnitsHealed");

                    b.Property<long>("TrinketId");

                    b.Property<long>("TrueDamageDealt");

                    b.Property<long>("TrueDamageDealtToChampions");

                    b.Property<long>("TrueDamageTaken");

                    b.Property<long>("TurretKills");

                    b.Property<long>("VisionScore");

                    b.Property<long>("VisionWardsBoughtInGame");

                    b.Property<long>("WardsPlaced");

                    b.HasKey("Id");

                    b.HasIndex("MatchTeamId1");

                    b.ToTable("MatchPlayer");
                });

            modelBuilder.Entity("LccWebAPI.Models.Match.MatchTeam", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BaronKills");

                    b.Property<int>("DragonKills");

                    b.Property<int>("InhibitorKills");

                    b.Property<int>("MatchId");

                    b.Property<int>("RiftHeraldKills");

                    b.Property<int>("TeamId");

                    b.HasKey("Id");

                    b.HasIndex("MatchId");

                    b.ToTable("MatchTeam");
                });

            modelBuilder.Entity("LccWebAPI.Models.StaticData.Champion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChampionId");

                    b.Property<string>("ChampionName");

                    b.Property<string>("ImageFull");

                    b.HasKey("Id");

                    b.ToTable("Champions");
                });

            modelBuilder.Entity("LccWebAPI.Models.StaticData.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("ImageFull");

                    b.Property<int>("ItemId");

                    b.Property<string>("ItemName");

                    b.Property<string>("PlainText");

                    b.Property<string>("SanitizedDescription");

                    b.HasKey("Id");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("LccWebAPI.Models.StaticData.Rune", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Key");

                    b.Property<string>("LongDesc");

                    b.Property<int>("RuneId");

                    b.Property<string>("RuneName");

                    b.Property<int>("RunePathId");

                    b.Property<string>("RunePathName");

                    b.Property<string>("ShortDesc");

                    b.HasKey("Id");

                    b.ToTable("Runes");
                });

            modelBuilder.Entity("LccWebAPI.Models.StaticData.SummonerSpell", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("ImageFull");

                    b.Property<int>("SummonerSpellId");

                    b.Property<string>("SummonerSpellName");

                    b.HasKey("Id");

                    b.ToTable("SummonerSpells");
                });

            modelBuilder.Entity("LccWebAPI.Models.Summoner.Summoner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("AccountId");

                    b.Property<DateTime>("LastUpdatedDate");

                    b.Property<long>("Level");

                    b.Property<int>("ProfileIconId");

                    b.Property<DateTime>("RevisionDate");

                    b.Property<long>("SummonerId");

                    b.Property<string>("SummonerName");

                    b.HasKey("Id");

                    b.ToTable("Summoners");
                });

            modelBuilder.Entity("LccWebAPI.Models.Match.MatchPlayer", b =>
                {
                    b.HasOne("LccWebAPI.Models.Match.MatchTeam", "MatchTeam")
                        .WithMany("Players")
                        .HasForeignKey("MatchTeamId1");
                });

            modelBuilder.Entity("LccWebAPI.Models.Match.MatchTeam", b =>
                {
                    b.HasOne("LccWebAPI.Models.Match.Match", "Match")
                        .WithMany("Teams")
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
