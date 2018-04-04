using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LccWebAPI.Migrations
{
    public partial class addedNewFieldsToDbObjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SummonerSpellSlot",
                table: "PlayerSummonerSpell",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RuneSlot",
                table: "PlayerRune",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemSlot",
                table: "PlayerItem",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BaronKills",
                table: "MatchTeam",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DragonKills",
                table: "MatchTeam",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InhibitorKills",
                table: "MatchTeam",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RiftHeraldKills",
                table: "MatchTeam",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "MatchTeam",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Assists",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ChampionId",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Deaths",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Kills",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ParticipantId",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "PlayerId",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "MatchDate",
                table: "Matches",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "MatchDuration",
                table: "Matches",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<string>(
                name: "MatchPatch",
                table: "Matches",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WinningTeamId",
                table: "Matches",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SummonerSpellSlot",
                table: "PlayerSummonerSpell");

            migrationBuilder.DropColumn(
                name: "RuneSlot",
                table: "PlayerRune");

            migrationBuilder.DropColumn(
                name: "ItemSlot",
                table: "PlayerItem");

            migrationBuilder.DropColumn(
                name: "BaronKills",
                table: "MatchTeam");

            migrationBuilder.DropColumn(
                name: "DragonKills",
                table: "MatchTeam");

            migrationBuilder.DropColumn(
                name: "InhibitorKills",
                table: "MatchTeam");

            migrationBuilder.DropColumn(
                name: "RiftHeraldKills",
                table: "MatchTeam");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "MatchTeam");

            migrationBuilder.DropColumn(
                name: "Assists",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "ChampionId",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "Deaths",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "Kills",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "ParticipantId",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "MatchDate",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "MatchDuration",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "MatchPatch",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "WinningTeamId",
                table: "Matches");
        }
    }
}
