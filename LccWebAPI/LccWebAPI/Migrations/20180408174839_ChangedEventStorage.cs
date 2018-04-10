using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LccWebAPI.Migrations
{
    public partial class ChangedEventStorage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchEvent_MatchPlayer_MatchPlayerId",
                table: "MatchEvent");

            migrationBuilder.RenameColumn(
                name: "MatchPlayerId",
                table: "MatchEvent",
                newName: "MatchTimelineId");

            migrationBuilder.RenameIndex(
                name: "IX_MatchEvent_MatchPlayerId",
                table: "MatchEvent",
                newName: "IX_MatchEvent_MatchTimelineId");

            migrationBuilder.CreateTable(
                name: "MatchTimelines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GameId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchTimelines", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_MatchEvent_MatchTimelines_MatchTimelineId",
                table: "MatchEvent",
                column: "MatchTimelineId",
                principalTable: "MatchTimelines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchEvent_MatchTimelines_MatchTimelineId",
                table: "MatchEvent");

            migrationBuilder.DropTable(
                name: "MatchTimelines");

            migrationBuilder.RenameColumn(
                name: "MatchTimelineId",
                table: "MatchEvent",
                newName: "MatchPlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_MatchEvent_MatchTimelineId",
                table: "MatchEvent",
                newName: "IX_MatchEvent_MatchPlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchEvent_MatchPlayer_MatchPlayerId",
                table: "MatchEvent",
                column: "MatchPlayerId",
                principalTable: "MatchPlayer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
