using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LccWebAPI.Migrations
{
    public partial class newTimespanColumnInTimeline : Migration
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
                name: "Db_LccCachedTeamInformation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BaronKills = table.Column<long>(nullable: false),
                    DragonKills = table.Column<long>(nullable: false),
                    InhibitorKills = table.Column<long>(nullable: false),
                    RiftHeraldKills = table.Column<long>(nullable: false),
                    TotalAssists = table.Column<long>(nullable: false),
                    TotalDeaths = table.Column<long>(nullable: false),
                    TotalKills = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Db_LccCachedTeamInformation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Db_LccMatchTimeline",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FrameInterval = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Db_LccMatchTimeline", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ImageFull = table.Column<string>(nullable: true),
                    ItemId = table.Column<int>(nullable: false),
                    ItemName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Matchups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GameId = table.Column<long>(nullable: false),
                    MatchDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matchups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Runes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Icon = table.Column<string>(nullable: true),
                    Key = table.Column<string>(nullable: true),
                    LongDesc = table.Column<string>(nullable: true),
                    RuneId = table.Column<int>(nullable: false),
                    RuneName = table.Column<string>(nullable: true),
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
                    LastUpdatedTime = table.Column<DateTime>(nullable: false),
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
                    ImageFull = table.Column<string>(nullable: true),
                    SummonerSpellId = table.Column<int>(nullable: false),
                    SummonerSpellName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SummonerSpells", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CalculatedMatchupInformation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EnemyTeamId = table.Column<int>(nullable: true),
                    FriendlyTeamId = table.Column<int>(nullable: true),
                    FriendlyTeamWin = table.Column<bool>(nullable: false),
                    GameId = table.Column<long>(nullable: false),
                    MatchDate = table.Column<DateTime>(nullable: false),
                    MatchDuration = table.Column<TimeSpan>(nullable: false),
                    MatchPatch = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculatedMatchupInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalculatedMatchupInformation_Db_LccCachedTeamInformation_EnemyTeamId",
                        column: x => x.EnemyTeamId,
                        principalTable: "Db_LccCachedTeamInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CalculatedMatchupInformation_Db_LccCachedTeamInformation_FriendlyTeamId",
                        column: x => x.FriendlyTeamId,
                        principalTable: "Db_LccCachedTeamInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Db_LccMatchTimelineFrame",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Db_LccMatchTimelineId = table.Column<int>(nullable: true),
                    Timestamp = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Db_LccMatchTimelineFrame", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Db_LccMatchTimelineFrame_Db_LccMatchTimeline_Db_LccMatchTimelineId",
                        column: x => x.Db_LccMatchTimelineId,
                        principalTable: "Db_LccMatchTimeline",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Db_LccBasicMatchInfoPlayer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChampionId = table.Column<int>(nullable: false),
                    Db_LccBasicMatchInfoId = table.Column<int>(nullable: true),
                    Db_LccBasicMatchInfoId1 = table.Column<int>(nullable: true),
                    Lane = table.Column<string>(nullable: true),
                    PlayerAccountId = table.Column<long>(nullable: false),
                    SummonerName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Db_LccBasicMatchInfoPlayer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Db_LccBasicMatchInfoPlayer_Matchups_Db_LccBasicMatchInfoId",
                        column: x => x.Db_LccBasicMatchInfoId,
                        principalTable: "Matchups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Db_LccBasicMatchInfoPlayer_Matchups_Db_LccBasicMatchInfoId1",
                        column: x => x.Db_LccBasicMatchInfoId1,
                        principalTable: "Matchups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Db_LccCachedPlayerStats",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Assists = table.Column<long>(nullable: false),
                    ChampionId = table.Column<int>(nullable: true),
                    ChampionLevel = table.Column<long>(nullable: false),
                    Db_LccCachedTeamInformationId = table.Column<int>(nullable: true),
                    Deaths = table.Column<long>(nullable: false),
                    ItemFiveId = table.Column<int>(nullable: true),
                    ItemFourId = table.Column<int>(nullable: true),
                    ItemOneId = table.Column<int>(nullable: true),
                    ItemSixId = table.Column<int>(nullable: true),
                    ItemThreeId = table.Column<int>(nullable: true),
                    ItemTwoId = table.Column<int>(nullable: true),
                    Kills = table.Column<long>(nullable: false),
                    MinionKills = table.Column<long>(nullable: false),
                    PrimaryRuneStyleId = table.Column<int>(nullable: true),
                    PrimaryRuneSubFourId = table.Column<int>(nullable: true),
                    PrimaryRuneSubOneId = table.Column<int>(nullable: true),
                    PrimaryRuneSubThreeId = table.Column<int>(nullable: true),
                    PrimaryRuneSubTwoId = table.Column<int>(nullable: true),
                    RankedSoloDivision = table.Column<string>(nullable: true),
                    RankedSoloLeaguePoints = table.Column<string>(nullable: true),
                    RankedSoloLosses = table.Column<long>(nullable: false),
                    RankedSoloTier = table.Column<string>(nullable: true),
                    RankedSoloWins = table.Column<long>(nullable: false),
                    SecondaryRuneStyleId = table.Column<int>(nullable: true),
                    SecondaryRuneSubOneId = table.Column<int>(nullable: true),
                    SecondaryRuneSubTwoId = table.Column<int>(nullable: true),
                    SummonerId = table.Column<long>(nullable: false),
                    SummonerName = table.Column<string>(nullable: true),
                    SummonerOneId = table.Column<int>(nullable: true),
                    SummonerTwoId = table.Column<int>(nullable: true),
                    TimelineId = table.Column<int>(nullable: true),
                    TrinketId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Db_LccCachedPlayerStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Db_LccCachedPlayerStats_Champions_ChampionId",
                        column: x => x.ChampionId,
                        principalTable: "Champions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Db_LccCachedPlayerStats_Db_LccCachedTeamInformation_Db_LccCachedTeamInformationId",
                        column: x => x.Db_LccCachedTeamInformationId,
                        principalTable: "Db_LccCachedTeamInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Db_LccCachedPlayerStats_Items_ItemFiveId",
                        column: x => x.ItemFiveId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Db_LccCachedPlayerStats_Items_ItemFourId",
                        column: x => x.ItemFourId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Db_LccCachedPlayerStats_Items_ItemOneId",
                        column: x => x.ItemOneId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Db_LccCachedPlayerStats_Items_ItemSixId",
                        column: x => x.ItemSixId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Db_LccCachedPlayerStats_Items_ItemThreeId",
                        column: x => x.ItemThreeId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Db_LccCachedPlayerStats_Items_ItemTwoId",
                        column: x => x.ItemTwoId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Db_LccCachedPlayerStats_Runes_PrimaryRuneStyleId",
                        column: x => x.PrimaryRuneStyleId,
                        principalTable: "Runes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Db_LccCachedPlayerStats_Runes_PrimaryRuneSubFourId",
                        column: x => x.PrimaryRuneSubFourId,
                        principalTable: "Runes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Db_LccCachedPlayerStats_Runes_PrimaryRuneSubOneId",
                        column: x => x.PrimaryRuneSubOneId,
                        principalTable: "Runes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Db_LccCachedPlayerStats_Runes_PrimaryRuneSubThreeId",
                        column: x => x.PrimaryRuneSubThreeId,
                        principalTable: "Runes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Db_LccCachedPlayerStats_Runes_PrimaryRuneSubTwoId",
                        column: x => x.PrimaryRuneSubTwoId,
                        principalTable: "Runes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Db_LccCachedPlayerStats_Runes_SecondaryRuneStyleId",
                        column: x => x.SecondaryRuneStyleId,
                        principalTable: "Runes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Db_LccCachedPlayerStats_Runes_SecondaryRuneSubOneId",
                        column: x => x.SecondaryRuneSubOneId,
                        principalTable: "Runes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Db_LccCachedPlayerStats_Runes_SecondaryRuneSubTwoId",
                        column: x => x.SecondaryRuneSubTwoId,
                        principalTable: "Runes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Db_LccCachedPlayerStats_SummonerSpells_SummonerOneId",
                        column: x => x.SummonerOneId,
                        principalTable: "SummonerSpells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Db_LccCachedPlayerStats_SummonerSpells_SummonerTwoId",
                        column: x => x.SummonerTwoId,
                        principalTable: "SummonerSpells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Db_LccCachedPlayerStats_Db_LccMatchTimeline_TimelineId",
                        column: x => x.TimelineId,
                        principalTable: "Db_LccMatchTimeline",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Db_LccCachedPlayerStats_Items_TrinketId",
                        column: x => x.TrinketId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Db_MatchTimelineEvent",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AfterId = table.Column<long>(nullable: true),
                    BeforeId = table.Column<long>(nullable: true),
                    BuildingType = table.Column<string>(nullable: true),
                    CreatorId = table.Column<long>(nullable: true),
                    Db_LccMatchTimelineFrameId = table.Column<int>(nullable: true),
                    ItemId = table.Column<long>(nullable: true),
                    KillerId = table.Column<long>(nullable: true),
                    LaneType = table.Column<string>(nullable: true),
                    LevelUpType = table.Column<string>(nullable: true),
                    MonsterSubType = table.Column<string>(nullable: true),
                    MonsterType = table.Column<string>(nullable: true),
                    ParticipantId = table.Column<long>(nullable: true),
                    SkillSlot = table.Column<long>(nullable: true),
                    TeamId = table.Column<long>(nullable: true),
                    Timestamp = table.Column<TimeSpan>(nullable: false),
                    TowerType = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    VictimId = table.Column<long>(nullable: true),
                    WardType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Db_MatchTimelineEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Db_MatchTimelineEvent_Db_LccMatchTimelineFrame_Db_LccMatchTimelineFrameId",
                        column: x => x.Db_LccMatchTimelineFrameId,
                        principalTable: "Db_LccMatchTimelineFrame",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalculatedMatchupInformation_EnemyTeamId",
                table: "CalculatedMatchupInformation",
                column: "EnemyTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_CalculatedMatchupInformation_FriendlyTeamId",
                table: "CalculatedMatchupInformation",
                column: "FriendlyTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Db_LccBasicMatchInfoPlayer_Db_LccBasicMatchInfoId",
                table: "Db_LccBasicMatchInfoPlayer",
                column: "Db_LccBasicMatchInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Db_LccBasicMatchInfoPlayer_Db_LccBasicMatchInfoId1",
                table: "Db_LccBasicMatchInfoPlayer",
                column: "Db_LccBasicMatchInfoId1");

            migrationBuilder.CreateIndex(
                name: "IX_Db_LccCachedPlayerStats_ChampionId",
                table: "Db_LccCachedPlayerStats",
                column: "ChampionId");

            migrationBuilder.CreateIndex(
                name: "IX_Db_LccCachedPlayerStats_Db_LccCachedTeamInformationId",
                table: "Db_LccCachedPlayerStats",
                column: "Db_LccCachedTeamInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Db_LccCachedPlayerStats_ItemFiveId",
                table: "Db_LccCachedPlayerStats",
                column: "ItemFiveId");

            migrationBuilder.CreateIndex(
                name: "IX_Db_LccCachedPlayerStats_ItemFourId",
                table: "Db_LccCachedPlayerStats",
                column: "ItemFourId");

            migrationBuilder.CreateIndex(
                name: "IX_Db_LccCachedPlayerStats_ItemOneId",
                table: "Db_LccCachedPlayerStats",
                column: "ItemOneId");

            migrationBuilder.CreateIndex(
                name: "IX_Db_LccCachedPlayerStats_ItemSixId",
                table: "Db_LccCachedPlayerStats",
                column: "ItemSixId");

            migrationBuilder.CreateIndex(
                name: "IX_Db_LccCachedPlayerStats_ItemThreeId",
                table: "Db_LccCachedPlayerStats",
                column: "ItemThreeId");

            migrationBuilder.CreateIndex(
                name: "IX_Db_LccCachedPlayerStats_ItemTwoId",
                table: "Db_LccCachedPlayerStats",
                column: "ItemTwoId");

            migrationBuilder.CreateIndex(
                name: "IX_Db_LccCachedPlayerStats_PrimaryRuneStyleId",
                table: "Db_LccCachedPlayerStats",
                column: "PrimaryRuneStyleId");

            migrationBuilder.CreateIndex(
                name: "IX_Db_LccCachedPlayerStats_PrimaryRuneSubFourId",
                table: "Db_LccCachedPlayerStats",
                column: "PrimaryRuneSubFourId");

            migrationBuilder.CreateIndex(
                name: "IX_Db_LccCachedPlayerStats_PrimaryRuneSubOneId",
                table: "Db_LccCachedPlayerStats",
                column: "PrimaryRuneSubOneId");

            migrationBuilder.CreateIndex(
                name: "IX_Db_LccCachedPlayerStats_PrimaryRuneSubThreeId",
                table: "Db_LccCachedPlayerStats",
                column: "PrimaryRuneSubThreeId");

            migrationBuilder.CreateIndex(
                name: "IX_Db_LccCachedPlayerStats_PrimaryRuneSubTwoId",
                table: "Db_LccCachedPlayerStats",
                column: "PrimaryRuneSubTwoId");

            migrationBuilder.CreateIndex(
                name: "IX_Db_LccCachedPlayerStats_SecondaryRuneStyleId",
                table: "Db_LccCachedPlayerStats",
                column: "SecondaryRuneStyleId");

            migrationBuilder.CreateIndex(
                name: "IX_Db_LccCachedPlayerStats_SecondaryRuneSubOneId",
                table: "Db_LccCachedPlayerStats",
                column: "SecondaryRuneSubOneId");

            migrationBuilder.CreateIndex(
                name: "IX_Db_LccCachedPlayerStats_SecondaryRuneSubTwoId",
                table: "Db_LccCachedPlayerStats",
                column: "SecondaryRuneSubTwoId");

            migrationBuilder.CreateIndex(
                name: "IX_Db_LccCachedPlayerStats_SummonerOneId",
                table: "Db_LccCachedPlayerStats",
                column: "SummonerOneId");

            migrationBuilder.CreateIndex(
                name: "IX_Db_LccCachedPlayerStats_SummonerTwoId",
                table: "Db_LccCachedPlayerStats",
                column: "SummonerTwoId");

            migrationBuilder.CreateIndex(
                name: "IX_Db_LccCachedPlayerStats_TimelineId",
                table: "Db_LccCachedPlayerStats",
                column: "TimelineId");

            migrationBuilder.CreateIndex(
                name: "IX_Db_LccCachedPlayerStats_TrinketId",
                table: "Db_LccCachedPlayerStats",
                column: "TrinketId");

            migrationBuilder.CreateIndex(
                name: "IX_Db_LccMatchTimelineFrame_Db_LccMatchTimelineId",
                table: "Db_LccMatchTimelineFrame",
                column: "Db_LccMatchTimelineId");

            migrationBuilder.CreateIndex(
                name: "IX_Db_MatchTimelineEvent_Db_LccMatchTimelineFrameId",
                table: "Db_MatchTimelineEvent",
                column: "Db_LccMatchTimelineFrameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalculatedMatchupInformation");

            migrationBuilder.DropTable(
                name: "Db_LccBasicMatchInfoPlayer");

            migrationBuilder.DropTable(
                name: "Db_LccCachedPlayerStats");

            migrationBuilder.DropTable(
                name: "Db_MatchTimelineEvent");

            migrationBuilder.DropTable(
                name: "Summoners");

            migrationBuilder.DropTable(
                name: "Matchups");

            migrationBuilder.DropTable(
                name: "Champions");

            migrationBuilder.DropTable(
                name: "Db_LccCachedTeamInformation");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Runes");

            migrationBuilder.DropTable(
                name: "SummonerSpells");

            migrationBuilder.DropTable(
                name: "Db_LccMatchTimelineFrame");

            migrationBuilder.DropTable(
                name: "Db_LccMatchTimeline");
        }
    }
}
