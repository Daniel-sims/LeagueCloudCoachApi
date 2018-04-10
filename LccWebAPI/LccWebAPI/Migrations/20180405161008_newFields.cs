using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LccWebAPI.Migrations
{
    public partial class newFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MatchPatch",
                table: "Matches",
                newName: "GamePatch");

            migrationBuilder.RenameColumn(
                name: "MatchDuration",
                table: "Matches",
                newName: "GameDuration");

            migrationBuilder.RenameColumn(
                name: "MatchDate",
                table: "Matches",
                newName: "GameDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedDate",
                table: "Summoners",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<long>(
                name: "Kills",
                table: "MatchPlayer",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<long>(
                name: "Deaths",
                table: "MatchPlayer",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<long>(
                name: "Assists",
                table: "MatchPlayer",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "ChampionLevel",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "DamageDealtToObjectives",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DamageDealtToTurrets",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DoubleKills",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "FirstBloodAssist",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "FirstBloodKill",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "FirstInhibitorAssist",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "FirstInhibitorKill",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "FirstTowerAssist",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "FirstTowerKill",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "GoldEarned",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "InhibitorKills",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "KillingSprees",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "LargestCriticalStrike",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "LargestKillingSpree",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "LargestMultiKill",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "MagicDamageDealt",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "MagicDamageDealtToChampions",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "MagicalDamageTaken",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "NeutralMinionsKilled",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "NeutralMinionsKilledEnemyJungle",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "NeutralMinionsKilledTeamJungle",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ObjectivePlayerScore",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PentaKills",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PhysicalDamageDealt",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PhysicalDamageDealtToChampions",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PhysicalDamageTaken",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "QuadraKills",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "SightWardsBoughtInGame",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TimeCCingOthers",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TotalDamageDealt",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TotalDamageDealtToChampions",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TotalDamageTaken",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TotalHeal",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TotalMinionsKilled",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TotalScoreRank",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TotalTimeCrowdControlDealt",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TotalUnitsHealed",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TrueDamageDealt",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TrueDamageDealtToChampions",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TrueDamageTaken",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TurretKills",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "VisionScore",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "VisionWardsBoughtInGame",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "WardsPlaced",
                table: "MatchPlayer",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdatedDate",
                table: "Summoners");

            migrationBuilder.DropColumn(
                name: "ChampionLevel",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "DamageDealtToObjectives",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "DamageDealtToTurrets",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "DoubleKills",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "FirstBloodAssist",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "FirstBloodKill",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "FirstInhibitorAssist",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "FirstInhibitorKill",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "FirstTowerAssist",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "FirstTowerKill",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "GoldEarned",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "InhibitorKills",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "KillingSprees",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "LargestCriticalStrike",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "LargestKillingSpree",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "LargestMultiKill",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "MagicDamageDealt",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "MagicDamageDealtToChampions",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "MagicalDamageTaken",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "NeutralMinionsKilled",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "NeutralMinionsKilledEnemyJungle",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "NeutralMinionsKilledTeamJungle",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "ObjectivePlayerScore",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "PentaKills",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "PhysicalDamageDealt",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "PhysicalDamageDealtToChampions",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "PhysicalDamageTaken",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "QuadraKills",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "SightWardsBoughtInGame",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "TimeCCingOthers",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "TotalDamageDealt",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "TotalDamageDealtToChampions",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "TotalDamageTaken",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "TotalHeal",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "TotalMinionsKilled",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "TotalScoreRank",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "TotalTimeCrowdControlDealt",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "TotalUnitsHealed",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "TrueDamageDealt",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "TrueDamageDealtToChampions",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "TrueDamageTaken",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "TurretKills",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "VisionScore",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "VisionWardsBoughtInGame",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "WardsPlaced",
                table: "MatchPlayer");

            migrationBuilder.RenameColumn(
                name: "GamePatch",
                table: "Matches",
                newName: "MatchPatch");

            migrationBuilder.RenameColumn(
                name: "GameDuration",
                table: "Matches",
                newName: "MatchDuration");

            migrationBuilder.RenameColumn(
                name: "GameDate",
                table: "Matches",
                newName: "MatchDate");

            migrationBuilder.AlterColumn<int>(
                name: "Kills",
                table: "MatchPlayer",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<int>(
                name: "Deaths",
                table: "MatchPlayer",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<int>(
                name: "Assists",
                table: "MatchPlayer",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}
