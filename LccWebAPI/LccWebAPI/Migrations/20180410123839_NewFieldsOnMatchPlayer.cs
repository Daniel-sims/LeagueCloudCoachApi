using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LccWebAPI.Migrations
{
    public partial class NewFieldsOnMatchPlayer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlayerId",
                table: "MatchPlayer",
                newName: "SummonerId");

            migrationBuilder.AddColumn<long>(
                name: "AccountId",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ProfileIconId",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "SummonerName",
                table: "MatchPlayer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "ProfileIconId",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "SummonerName",
                table: "MatchPlayer");

            migrationBuilder.RenameColumn(
                name: "SummonerId",
                table: "MatchPlayer",
                newName: "PlayerId");
        }
    }
}
