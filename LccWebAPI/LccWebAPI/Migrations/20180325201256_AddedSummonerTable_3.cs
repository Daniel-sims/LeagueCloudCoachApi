using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LccWebAPI.Migrations
{
    public partial class AddedSummonerTable_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MatchDate",
                table: "Matchups");

            migrationBuilder.DropColumn(
                name: "MatchPatch",
                table: "Matchups");

            migrationBuilder.AddColumn<string>(
                name: "Lane",
                table: "Db_LccBasicMatchInfoPlayer",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PlayerAccountId",
                table: "Db_LccBasicMatchInfoPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "SummonerName",
                table: "Db_LccBasicMatchInfoPlayer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lane",
                table: "Db_LccBasicMatchInfoPlayer");

            migrationBuilder.DropColumn(
                name: "PlayerAccountId",
                table: "Db_LccBasicMatchInfoPlayer");

            migrationBuilder.DropColumn(
                name: "SummonerName",
                table: "Db_LccBasicMatchInfoPlayer");

            migrationBuilder.AddColumn<DateTime>(
                name: "MatchDate",
                table: "Matchups",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "MatchPatch",
                table: "Matchups",
                nullable: true);
        }
    }
}
