using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LccWebAPI.Migrations
{
    public partial class UpdatedSomeFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ChampionLevel",
                table: "MatchPlayer",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<long>(
                name: "GoldSpent",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoldSpent",
                table: "MatchPlayer");

            migrationBuilder.AlterColumn<int>(
                name: "ChampionLevel",
                table: "MatchPlayer",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}
