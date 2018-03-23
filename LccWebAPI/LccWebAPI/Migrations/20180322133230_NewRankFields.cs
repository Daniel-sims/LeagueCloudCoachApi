using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LccWebAPI.Migrations
{
    public partial class NewRankFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "Wins",
                table: "LccMatchupInformationPlayer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "Wins",
                table: "LccMatchupInformationPlayer");
        }
    }
}
