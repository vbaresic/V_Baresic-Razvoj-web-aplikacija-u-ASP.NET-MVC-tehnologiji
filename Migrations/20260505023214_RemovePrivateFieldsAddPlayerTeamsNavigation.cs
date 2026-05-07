using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace League_of_Legends_Tournament_Hosting.Migrations
{
    /// <inheritdoc />
    public partial class RemovePrivateFieldsAddPlayerTeamsNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Teams_TeamId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_Teams_TeamId1",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_Teams_TeamId3",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_Teams_TeamId4",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Sponsors_Tournaments_TournamentId",
                table: "Sponsors");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamPlayers_Teams_TeamId",
                table: "TeamPlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Tournaments_TournamentId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_TournamentId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Sponsors_TournamentId",
                table: "Sponsors");

            migrationBuilder.DropIndex(
                name: "IX_Players_TeamId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_TeamId1",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_TeamId3",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_TeamId4",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "TournamentId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "TournamentId",
                table: "Sponsors");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "TeamId1",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "TeamId3",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "TeamId4",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "TeamId",
                table: "TeamPlayers",
                newName: "TeamsId");

            migrationBuilder.RenameIndex(
                name: "IX_TeamPlayers_TeamId",
                table: "TeamPlayers",
                newName: "IX_TeamPlayers_TeamsId");

            migrationBuilder.AddColumn<bool>(
                name: "IsRosterConfirmed",
                table: "Teams",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamPlayers_Teams_TeamsId",
                table: "TeamPlayers",
                column: "TeamsId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamPlayers_Teams_TeamsId",
                table: "TeamPlayers");

            migrationBuilder.DropColumn(
                name: "IsRosterConfirmed",
                table: "Teams");

            migrationBuilder.RenameColumn(
                name: "TeamsId",
                table: "TeamPlayers",
                newName: "TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_TeamPlayers_TeamsId",
                table: "TeamPlayers",
                newName: "IX_TeamPlayers_TeamId");

            migrationBuilder.AddColumn<int>(
                name: "TournamentId",
                table: "Teams",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TournamentId",
                table: "Sponsors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamId1",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamId3",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamId4",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Sponsors",
                keyColumn: "Id",
                keyValue: 1,
                column: "TournamentId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sponsors",
                keyColumn: "Id",
                keyValue: 2,
                column: "TournamentId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sponsors",
                keyColumn: "Id",
                keyValue: 3,
                column: "TournamentId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_TournamentId",
                table: "Teams",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sponsors_TournamentId",
                table: "Sponsors",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_TeamId",
                table: "Players",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_TeamId1",
                table: "Players",
                column: "TeamId1");

            migrationBuilder.CreateIndex(
                name: "IX_Players_TeamId3",
                table: "Players",
                column: "TeamId3");

            migrationBuilder.CreateIndex(
                name: "IX_Players_TeamId4",
                table: "Players",
                column: "TeamId4");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Teams_TeamId",
                table: "Players",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Teams_TeamId1",
                table: "Players",
                column: "TeamId1",
                principalTable: "Teams",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Teams_TeamId3",
                table: "Players",
                column: "TeamId3",
                principalTable: "Teams",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Teams_TeamId4",
                table: "Players",
                column: "TeamId4",
                principalTable: "Teams",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sponsors_Tournaments_TournamentId",
                table: "Sponsors",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamPlayers_Teams_TeamId",
                table: "TeamPlayers",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Tournaments_TournamentId",
                table: "Teams",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id");
        }
    }
}
