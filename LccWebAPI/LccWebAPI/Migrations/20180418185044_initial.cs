using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LccWebAPI.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Champions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChampionId = table.Column<int>(nullable: false),
                    ChampionName = table.Column<string>(nullable: true),
                    ImageFull = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Champions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    ImageFull = table.Column<string>(nullable: true),
                    ItemId = table.Column<int>(nullable: false),
                    ItemName = table.Column<string>(nullable: true),
                    PlainText = table.Column<string>(nullable: true),
                    SanitizedDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GameDate = table.Column<DateTime>(nullable: false),
                    GameDuration = table.Column<TimeSpan>(nullable: false),
                    GameId = table.Column<long>(nullable: false),
                    GamePatch = table.Column<string>(nullable: true),
                    WinningTeamId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MatchTimelines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GameId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchTimelines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Runes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<string>(nullable: true),
                    LongDesc = table.Column<string>(nullable: true),
                    RuneId = table.Column<int>(nullable: false),
                    RuneName = table.Column<string>(nullable: true),
                    RunePathId = table.Column<int>(nullable: false),
                    RunePathName = table.Column<string>(nullable: true),
                    ShortDesc = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Runes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Summoners",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<long>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    Level = table.Column<long>(nullable: false),
                    ProfileIconId = table.Column<int>(nullable: false),
                    RevisionDate = table.Column<DateTime>(nullable: false),
                    SummonerId = table.Column<long>(nullable: false),
                    SummonerName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Summoners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SummonerSpells",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    ImageFull = table.Column<string>(nullable: true),
                    SummonerSpellId = table.Column<int>(nullable: false),
                    SummonerSpellName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SummonerSpells", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MatchTeam",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BaronKills = table.Column<int>(nullable: false),
                    DragonKills = table.Column<int>(nullable: false),
                    InhibitorKills = table.Column<int>(nullable: false),
                    MatchId = table.Column<int>(nullable: false),
                    RiftHeraldKills = table.Column<int>(nullable: false),
                    TeamId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchTeam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchTeam_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchEvent",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AfterId = table.Column<long>(nullable: true),
                    BeforeId = table.Column<long>(nullable: true),
                    BuildingType = table.Column<string>(nullable: true),
                    CreatorId = table.Column<long>(nullable: true),
                    ItemId = table.Column<long>(nullable: true),
                    KillerId = table.Column<long>(nullable: true),
                    LaneType = table.Column<string>(nullable: true),
                    LevelUpType = table.Column<string>(nullable: true),
                    MatchTimelineId = table.Column<int>(nullable: false),
                    MonsterSubType = table.Column<string>(nullable: true),
                    MonsterType = table.Column<string>(nullable: true),
                    ParticipantId = table.Column<long>(nullable: true),
                    SkillSlot = table.Column<long>(nullable: true),
                    TeamId = table.Column<long>(nullable: true),
                    Timestamp = table.Column<TimeSpan>(nullable: false),
                    TowerType = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    VictimId = table.Column<long>(nullable: true),
                    WardType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchEvent_MatchTimelines_MatchTimelineId",
                        column: x => x.MatchTimelineId,
                        principalTable: "MatchTimelines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchPlayer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<long>(nullable: false),
                    Assists = table.Column<long>(nullable: false),
                    ChampionId = table.Column<int>(nullable: false),
                    ChampionLevel = table.Column<long>(nullable: false),
                    DamageDealtToObjectives = table.Column<long>(nullable: false),
                    DamageDealtToTurrets = table.Column<long>(nullable: false),
                    Deaths = table.Column<long>(nullable: false),
                    DoubleKills = table.Column<long>(nullable: false),
                    FirstBloodAssist = table.Column<bool>(nullable: false),
                    FirstBloodKill = table.Column<bool>(nullable: false),
                    FirstInhibitorAssist = table.Column<bool>(nullable: false),
                    FirstInhibitorKill = table.Column<bool>(nullable: false),
                    FirstTowerAssist = table.Column<bool>(nullable: false),
                    FirstTowerKill = table.Column<bool>(nullable: false),
                    GoldEarned = table.Column<long>(nullable: false),
                    GoldSpent = table.Column<long>(nullable: false),
                    HighestAcheivedTierLastSeason = table.Column<string>(nullable: true),
                    InhibitorKills = table.Column<long>(nullable: false),
                    Item1Id = table.Column<long>(nullable: false),
                    Item2Id = table.Column<long>(nullable: false),
                    Item3Id = table.Column<long>(nullable: false),
                    Item4Id = table.Column<long>(nullable: false),
                    Item5Id = table.Column<long>(nullable: false),
                    Item6Id = table.Column<long>(nullable: false),
                    KillingSprees = table.Column<long>(nullable: false),
                    Kills = table.Column<long>(nullable: false),
                    LargestCriticalStrike = table.Column<long>(nullable: false),
                    LargestKillingSpree = table.Column<long>(nullable: false),
                    LargestMultiKill = table.Column<long>(nullable: false),
                    MagicDamageDealt = table.Column<long>(nullable: false),
                    MagicDamageDealtToChampions = table.Column<long>(nullable: false),
                    MagicDamageTaken = table.Column<long>(nullable: false),
                    MatchTeamId = table.Column<long>(nullable: false),
                    MatchTeamId1 = table.Column<int>(nullable: true),
                    NeutralMinionsKilled = table.Column<long>(nullable: false),
                    NeutralMinionsKilledEnemyJungle = table.Column<long>(nullable: false),
                    NeutralMinionsKilledTeamJungle = table.Column<long>(nullable: false),
                    ObjectivePlayerScore = table.Column<long>(nullable: false),
                    ParticipantId = table.Column<int>(nullable: false),
                    PentaKills = table.Column<long>(nullable: false),
                    PhysicalDamageDealt = table.Column<long>(nullable: false),
                    PhysicalDamageDealtToChampions = table.Column<long>(nullable: false),
                    PhysicalDamageTaken = table.Column<long>(nullable: false),
                    PrimaryRuneStyleId = table.Column<long>(nullable: false),
                    PrimaryRuneSubStyleFourId = table.Column<long>(nullable: false),
                    PrimaryRuneSubStyleOneId = table.Column<long>(nullable: false),
                    PrimaryRuneSubStyleThreeId = table.Column<long>(nullable: false),
                    PrimaryRuneSubStyleTwoId = table.Column<long>(nullable: false),
                    ProfileIconId = table.Column<long>(nullable: false),
                    QuadraKills = table.Column<long>(nullable: false),
                    SecondaryRuneStyleId = table.Column<long>(nullable: false),
                    SecondaryRuneSubStyleOneId = table.Column<long>(nullable: false),
                    SecondaryRuneSubStyleTwoId = table.Column<long>(nullable: false),
                    SightWardsBoughtInGame = table.Column<long>(nullable: false),
                    SummonerId = table.Column<long>(nullable: false),
                    SummonerName = table.Column<string>(nullable: true),
                    SummonerSpellOneId = table.Column<long>(nullable: false),
                    SummonerSpellTwoId = table.Column<long>(nullable: false),
                    TeamId = table.Column<int>(nullable: false),
                    TimeCCingOthers = table.Column<long>(nullable: false),
                    TotalDamageDealt = table.Column<long>(nullable: false),
                    TotalDamageDealtToChampions = table.Column<long>(nullable: false),
                    TotalDamageTaken = table.Column<long>(nullable: false),
                    TotalHeal = table.Column<long>(nullable: false),
                    TotalMinionsKilled = table.Column<long>(nullable: false),
                    TotalScoreRank = table.Column<long>(nullable: false),
                    TotalTimeCrowdControlDealt = table.Column<long>(nullable: false),
                    TotalUnitsHealed = table.Column<long>(nullable: false),
                    TrinketId = table.Column<long>(nullable: false),
                    TrueDamageDealt = table.Column<long>(nullable: false),
                    TrueDamageDealtToChampions = table.Column<long>(nullable: false),
                    TrueDamageTaken = table.Column<long>(nullable: false),
                    TurretKills = table.Column<long>(nullable: false),
                    VisionScore = table.Column<long>(nullable: false),
                    VisionWardsBoughtInGame = table.Column<long>(nullable: false),
                    WardsPlaced = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchPlayer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchPlayer_MatchTeam_MatchTeamId1",
                        column: x => x.MatchTeamId1,
                        principalTable: "MatchTeam",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MatchEvent_MatchTimelineId",
                table: "MatchEvent",
                column: "MatchTimelineId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchPlayer_MatchTeamId1",
                table: "MatchPlayer",
                column: "MatchTeamId1");

            migrationBuilder.CreateIndex(
                name: "IX_MatchTeam_MatchId",
                table: "MatchTeam",
                column: "MatchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Champions");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "MatchEvent");

            migrationBuilder.DropTable(
                name: "MatchPlayer");

            migrationBuilder.DropTable(
                name: "Runes");

            migrationBuilder.DropTable(
                name: "Summoners");

            migrationBuilder.DropTable(
                name: "SummonerSpells");

            migrationBuilder.DropTable(
                name: "MatchTimelines");

            migrationBuilder.DropTable(
                name: "MatchTeam");

            migrationBuilder.DropTable(
                name: "Matches");
        }
    }
}
