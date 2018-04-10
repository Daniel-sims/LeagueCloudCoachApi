using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LccWebAPI.Migrations
{
    public partial class ChangedMatchPlayerToHoldIdsNotObjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatchEvent");

            migrationBuilder.DropTable(
                name: "PlayerItem");

            migrationBuilder.DropTable(
                name: "PlayerRune");

            migrationBuilder.DropTable(
                name: "PlayerSummonerSpell");

            migrationBuilder.AddColumn<long>(
                name: "Item1Id",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Item2Id",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Item3Id",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Item4Id",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Item5Id",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Item6Id",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PrimaryRuneStyleId",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PrimaryRuneSubStyleFourId",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PrimaryRuneSubStyleOneId",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PrimaryRuneSubStyleThreeId",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PrimaryRuneSubStyleTwoId",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "SecondaryRuneStyleId",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "SecondaryRuneSubStyleOneId",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "SecondaryRuneSubStyleTwoId",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "SummonerSpellOneId",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "SummonerSpellTwoId",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Item1Id",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "Item2Id",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "Item3Id",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "Item4Id",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "Item5Id",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "Item6Id",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "PrimaryRuneStyleId",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "PrimaryRuneSubStyleFourId",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "PrimaryRuneSubStyleOneId",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "PrimaryRuneSubStyleThreeId",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "PrimaryRuneSubStyleTwoId",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "SecondaryRuneStyleId",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "SecondaryRuneSubStyleOneId",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "SecondaryRuneSubStyleTwoId",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "SummonerSpellOneId",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "SummonerSpellTwoId",
                table: "MatchPlayer");

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
                    MatchPlayerId = table.Column<int>(nullable: false),
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
                        name: "FK_MatchEvent_MatchPlayer_MatchPlayerId",
                        column: x => x.MatchPlayerId,
                        principalTable: "MatchPlayer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ItemId = table.Column<long>(nullable: true),
                    ItemSlot = table.Column<int>(nullable: false),
                    MatchPlayerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerItem_MatchPlayer_MatchPlayerId",
                        column: x => x.MatchPlayerId,
                        principalTable: "MatchPlayer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerRune",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MatchPlayerId = table.Column<int>(nullable: false),
                    RuneId = table.Column<long>(nullable: true),
                    RuneSlot = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerRune", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerRune_MatchPlayer_MatchPlayerId",
                        column: x => x.MatchPlayerId,
                        principalTable: "MatchPlayer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerSummonerSpell",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MatchPlayerId = table.Column<int>(nullable: false),
                    SummonerSpellId = table.Column<long>(nullable: true),
                    SummonerSpellSlot = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerSummonerSpell", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerSummonerSpell_MatchPlayer_MatchPlayerId",
                        column: x => x.MatchPlayerId,
                        principalTable: "MatchPlayer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MatchEvent_MatchPlayerId",
                table: "MatchEvent",
                column: "MatchPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerItem_MatchPlayerId",
                table: "PlayerItem",
                column: "MatchPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerRune_MatchPlayerId",
                table: "PlayerRune",
                column: "MatchPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerSummonerSpell_MatchPlayerId",
                table: "PlayerSummonerSpell",
                column: "MatchPlayerId");
        }
    }
}
