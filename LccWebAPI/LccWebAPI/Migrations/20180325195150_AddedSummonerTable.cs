using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LccWebAPI.Migrations
{
    public partial class AddedSummonerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Db_LccBasicMatchInfoPlayer_Matches_Db_LccBasicMatchInfoId",
                table: "Db_LccBasicMatchInfoPlayer");

            migrationBuilder.DropForeignKey(
                name: "FK_Db_LccBasicMatchInfoPlayer_Matches_Db_LccBasicMatchInfoId1",
                table: "Db_LccBasicMatchInfoPlayer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Matches",
                table: "Matches");

            migrationBuilder.RenameTable(
                name: "Matches",
                newName: "Matchups");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Matchups",
                table: "Matchups",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Summoners",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LastUpdatedTime = table.Column<DateTime>(nullable: false),
                    SummonerId = table.Column<int>(nullable: false),
                    SummonerName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Summoners", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Db_LccBasicMatchInfoPlayer_Matchups_Db_LccBasicMatchInfoId",
                table: "Db_LccBasicMatchInfoPlayer",
                column: "Db_LccBasicMatchInfoId",
                principalTable: "Matchups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Db_LccBasicMatchInfoPlayer_Matchups_Db_LccBasicMatchInfoId1",
                table: "Db_LccBasicMatchInfoPlayer",
                column: "Db_LccBasicMatchInfoId1",
                principalTable: "Matchups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Db_LccBasicMatchInfoPlayer_Matchups_Db_LccBasicMatchInfoId",
                table: "Db_LccBasicMatchInfoPlayer");

            migrationBuilder.DropForeignKey(
                name: "FK_Db_LccBasicMatchInfoPlayer_Matchups_Db_LccBasicMatchInfoId1",
                table: "Db_LccBasicMatchInfoPlayer");

            migrationBuilder.DropTable(
                name: "Summoners");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Matchups",
                table: "Matchups");

            migrationBuilder.RenameTable(
                name: "Matchups",
                newName: "Matches");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Matches",
                table: "Matches",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Db_LccBasicMatchInfoPlayer_Matches_Db_LccBasicMatchInfoId",
                table: "Db_LccBasicMatchInfoPlayer",
                column: "Db_LccBasicMatchInfoId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Db_LccBasicMatchInfoPlayer_Matches_Db_LccBasicMatchInfoId1",
                table: "Db_LccBasicMatchInfoPlayer",
                column: "Db_LccBasicMatchInfoId1",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
