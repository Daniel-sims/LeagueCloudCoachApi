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
                name: "SummonerBase",
                columns: table => new
                {
                    Level = table.Column<long>(nullable: true),
                    ProfileIconId = table.Column<int>(nullable: true),
                    RevisionDate = table.Column<DateTime>(nullable: true),
                    Id = table.Column<long>(nullable: false),
                    AccountId = table.Column<long>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Region = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SummonerBase", x => x.Id);
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
                    table.PrimaryKey("PK_Summoners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Summoners_SummonerBase_SummonerId",
                        column: x => x.SummonerId,
                        principalTable: "SummonerBase",
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
                name: "SummonerBase");
        }
    }
}
