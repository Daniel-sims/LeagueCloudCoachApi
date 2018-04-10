using Microsoft.EntityFrameworkCore.Migrations;

namespace LccWebAPI.Migrations
{
    public partial class AddedRankedInfoToPlayer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HighestAcheivedTierLastSeason",
                table: "MatchPlayer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HighestAcheivedTierLastSeason",
                table: "MatchPlayer");
        }
    }
}
