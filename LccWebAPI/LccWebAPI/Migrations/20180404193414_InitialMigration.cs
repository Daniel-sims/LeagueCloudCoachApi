using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LccWebAPI.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MatchTeam",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MatchId = table.Column<int>(nullable: false)
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
                name: "MatchPlayer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MatchTeamId = table.Column<long>(nullable: false),
                    MatchTeamId1 = table.Column<int>(nullable: true)
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

            migrationBuilder.CreateTable(
                name: "PlayerItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ItemId = table.Column<long>(nullable: false),
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
                    RuneId = table.Column<long>(nullable: false)
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
                    SummonerSpellId = table.Column<long>(nullable: false)
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
                name: "IX_MatchPlayer_MatchTeamId1",
                table: "MatchPlayer",
                column: "MatchTeamId1");

            migrationBuilder.CreateIndex(
                name: "IX_MatchTeam_MatchId",
                table: "MatchTeam",
                column: "MatchId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerItem");

            migrationBuilder.DropTable(
                name: "PlayerRune");

            migrationBuilder.DropTable(
                name: "PlayerSummonerSpell");

            migrationBuilder.DropTable(
                name: "MatchPlayer");

            migrationBuilder.DropTable(
                name: "MatchTeam");

            migrationBuilder.DropTable(
                name: "Matches");
        }
    }
}
