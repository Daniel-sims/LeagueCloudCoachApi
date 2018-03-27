using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LccWebAPI.Migrations
{
    public partial class AddedSummonerTable_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SummonerId",
                table: "Summoners");

            migrationBuilder.AddColumn<long>(
                name: "AccountId",
                table: "Summoners",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Summoners");

            migrationBuilder.AddColumn<int>(
                name: "SummonerId",
                table: "Summoners",
                nullable: false,
                defaultValue: 0);
        }
    }
}
