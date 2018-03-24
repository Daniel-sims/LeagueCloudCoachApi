using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LccWebAPI.Migrations
{
    public partial class AddingMatchDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SummonerName",
                table: "SummonerSpells",
                newName: "SummonerSpellName");

            migrationBuilder.RenameColumn(
                name: "SummonerId",
                table: "SummonerSpells",
                newName: "SummonerSpellId");

            migrationBuilder.AddColumn<DateTime>(
                name: "MatchDate",
                table: "Matches",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MatchDate",
                table: "Matches");

            migrationBuilder.RenameColumn(
                name: "SummonerSpellName",
                table: "SummonerSpells",
                newName: "SummonerName");

            migrationBuilder.RenameColumn(
                name: "SummonerSpellId",
                table: "SummonerSpells",
                newName: "SummonerId");
        }
    }
}
