using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LccWebAPI.Migrations
{
    public partial class InitialMigration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LccMatchupInformationPlayer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<long>(nullable: false),
                    ChampionId = table.Column<long>(nullable: false),
                    Lane = table.Column<string>(nullable: true),
                    LccMatchupInformationId = table.Column<int>(nullable: true),
                    LccMatchupInformationId1 = table.Column<int>(nullable: true),
                    SummonerName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LccMatchupInformationPlayer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LccMatchupInformationPlayer_Matches_LccMatchupInformationId",
                        column: x => x.LccMatchupInformationId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LccMatchupInformationPlayer_Matches_LccMatchupInformationId1",
                        column: x => x.LccMatchupInformationId1,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LccMatchupInformationPlayer_LccMatchupInformationId",
                table: "LccMatchupInformationPlayer",
                column: "LccMatchupInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_LccMatchupInformationPlayer_LccMatchupInformationId1",
                table: "LccMatchupInformationPlayer",
                column: "LccMatchupInformationId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LccMatchupInformationPlayer");
        }
    }
}
