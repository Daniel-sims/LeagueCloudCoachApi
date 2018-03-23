using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LccWebAPI.Migrations
{
    public partial class NewFields2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentAccountId",
                table: "LccMatchupInformationPlayer");

            migrationBuilder.DropColumn(
                name: "CurrentLp",
                table: "LccMatchupInformationPlayer");

            migrationBuilder.DropColumn(
                name: "CurrentRank",
                table: "LccMatchupInformationPlayer");

            migrationBuilder.DropColumn(
                name: "Losses",
                table: "LccMatchupInformationPlayer");

            migrationBuilder.DropColumn(
                name: "MatchHistoryUri",
                table: "LccMatchupInformationPlayer");

            migrationBuilder.DropColumn(
                name: "ProfileIcon",
                table: "LccMatchupInformationPlayer");

            migrationBuilder.DropColumn(
                name: "Wins",
                table: "LccMatchupInformationPlayer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CurrentAccountId",
                table: "LccMatchupInformationPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "CurrentLp",
                table: "LccMatchupInformationPlayer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentRank",
                table: "LccMatchupInformationPlayer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Losses",
                table: "LccMatchupInformationPlayer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatchHistoryUri",
                table: "LccMatchupInformationPlayer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProfileIcon",
                table: "LccMatchupInformationPlayer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Wins",
                table: "LccMatchupInformationPlayer",
                nullable: true);
        }
    }
}
