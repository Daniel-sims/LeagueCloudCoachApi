using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LccWebAPI.Migrations
{
    public partial class newRuneThings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RunePathId",
                table: "Runes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RunePathName",
                table: "Runes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RunePathId",
                table: "Runes");

            migrationBuilder.DropColumn(
                name: "RunePathName",
                table: "Runes");
        }
    }
}
