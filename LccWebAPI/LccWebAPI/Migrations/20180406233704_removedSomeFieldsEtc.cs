using Microsoft.EntityFrameworkCore.Migrations;

namespace LccWebAPI.Migrations
{
    public partial class removedSomeFieldsEtc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFull",
                table: "Runes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageFull",
                table: "Runes",
                nullable: true);
        }
    }
}
