using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LccWebAPI.Migrations
{
    public partial class addedNewFieldsAndRenames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MagicalDamageTaken",
                table: "MatchPlayer",
                newName: "MagicDamageTaken");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MagicDamageTaken",
                table: "MatchPlayer",
                newName: "MagicalDamageTaken");
        }
    }
}
