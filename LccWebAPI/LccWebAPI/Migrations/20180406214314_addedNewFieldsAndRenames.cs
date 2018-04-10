using Microsoft.EntityFrameworkCore.Migrations;

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
