using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LccWebAPI.Migrations
{
    public partial class addingImagestringsToStaticData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageFull",
                table: "SummonerSpells",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageFull",
                table: "Items",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFull",
                table: "SummonerSpells");

            migrationBuilder.DropColumn(
                name: "ImageFull",
                table: "Items");
        }
    }
}
