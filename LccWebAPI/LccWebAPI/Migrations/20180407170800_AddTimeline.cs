using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LccWebAPI.Migrations
{
    public partial class AddTimeline : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MatchEvent",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AfterId = table.Column<long>(nullable: true),
                    BeforeId = table.Column<long>(nullable: true),
                    BuildingType = table.Column<string>(nullable: true),
                    CreatorId = table.Column<long>(nullable: true),
                    ItemId = table.Column<long>(nullable: true),
                    KillerId = table.Column<long>(nullable: true),
                    LaneType = table.Column<string>(nullable: true),
                    LevelUpType = table.Column<string>(nullable: true),
                    MatchPlayerId = table.Column<int>(nullable: false),
                    MonsterSubType = table.Column<string>(nullable: true),
                    MonsterType = table.Column<string>(nullable: true),
                    ParticipantId = table.Column<long>(nullable: true),
                    SkillSlot = table.Column<long>(nullable: true),
                    TeamId = table.Column<long>(nullable: true),
                    Timestamp = table.Column<TimeSpan>(nullable: false),
                    TowerType = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    VictimId = table.Column<long>(nullable: true),
                    WardType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchEvent_MatchPlayer_MatchPlayerId",
                        column: x => x.MatchPlayerId,
                        principalTable: "MatchPlayer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MatchEvent_MatchPlayerId",
                table: "MatchEvent",
                column: "MatchPlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatchEvent");
        }
    }
}
