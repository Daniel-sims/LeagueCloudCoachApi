using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LccWebAPI.Migrations
{
    public partial class NewFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CurrentAccountId",
                table: "LccMatchupInformationPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "MatchHistoryUri",
                table: "LccMatchupInformationPlayer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProfileIcon",
                table: "LccMatchupInformationPlayer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "SummonerId",
                table: "LccMatchupInformationPlayer",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentAccountId",
                table: "LccMatchupInformationPlayer");

            migrationBuilder.DropColumn(
                name: "MatchHistoryUri",
                table: "LccMatchupInformationPlayer");

            migrationBuilder.DropColumn(
                name: "ProfileIcon",
                table: "LccMatchupInformationPlayer");

            migrationBuilder.DropColumn(
                name: "SummonerId",
                table: "LccMatchupInformationPlayer");
        }
    }
}
