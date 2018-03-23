using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LccWebAPI.Migrations
{
    public partial class NewFields3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SummonerId",
                table: "LccMatchupInformationPlayer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SummonerId",
                table: "LccMatchupInformationPlayer",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
