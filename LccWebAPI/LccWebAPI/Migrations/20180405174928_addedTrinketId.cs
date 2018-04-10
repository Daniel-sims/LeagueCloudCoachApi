using Microsoft.EntityFrameworkCore.Migrations;

namespace LccWebAPI.Migrations
{
    public partial class addedTrinketId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TrinketId",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrinketId",
                table: "MatchPlayer");
        }
    }
}
