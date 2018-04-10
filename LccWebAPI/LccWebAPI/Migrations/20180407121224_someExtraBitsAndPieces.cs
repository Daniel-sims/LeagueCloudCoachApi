using Microsoft.EntityFrameworkCore.Migrations;

namespace LccWebAPI.Migrations
{
    public partial class someExtraBitsAndPieces : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SummonerSpells",
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
                name: "ShortDesc",
                table: "Runes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlainText",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SanitizedDescription",
                table: "Items",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "SummonerSpells");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "Runes");

            migrationBuilder.DropColumn(
                name: "LongDesc",
                table: "Runes");

            migrationBuilder.DropColumn(
                name: "ShortDesc",
                table: "Runes");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "PlainText",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "SanitizedDescription",
                table: "Items");
        }
    }
}
