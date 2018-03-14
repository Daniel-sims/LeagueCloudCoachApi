using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LccWebAPI.Migrations
{
    public partial class Migration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Summoner",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<long>(nullable: false),
                    Level = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ProfileIconId = table.Column<int>(nullable: false),
                    Region = table.Column<int>(nullable: false),
                    RevisionDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Summoner", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Summoners",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SummonerId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Key", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Summoners_Summoner_SummonerId",
                        column: x => x.SummonerId,
                        principalTable: "Summoner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Summoners_SummonerId",
                table: "Summoners",
                column: "SummonerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Summoners");

            migrationBuilder.DropTable(
                name: "Summoner");
        }
    }
}
