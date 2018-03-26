using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LccWebAPI.Migrations
{
    public partial class addedNewRuneFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Runes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "Runes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LongDesc",
                table: "Runes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RunePathName",
                table: "Runes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortDesc",
                table: "Runes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Runes");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "Runes");

            migrationBuilder.DropColumn(
                name: "LongDesc",
                table: "Runes");

            migrationBuilder.DropColumn(
                name: "RunePathName",
                table: "Runes");

            migrationBuilder.DropColumn(
                name: "ShortDesc",
                table: "Runes");
        }
    }
}
