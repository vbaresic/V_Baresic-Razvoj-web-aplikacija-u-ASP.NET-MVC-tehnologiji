using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace League_of_Legends_Tournament_Hosting.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayersTeamsTournamentsSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "GamerTag", "JoinedAt", "Name", "PreferredPosition", "Role", "SecondaryPosition", "AccountInformation_SummonerName", "AccountInformation_RiotTag", "AccountInformation_Region", "AccountInformation_LeagueTier" },
                values: new object[,]
                {
                    { 1, "MirkoH", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mirko Horvat", 0, 0, 2, "MirkoTop", "MirkoH#EUW1", 0, 5 },
                    { 2, "PetarMark", new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Petar Marković", 2, 0, 1, "PetarJungle", "PetarMark#EUW1", 0, 6 },
                    { 3, "AnaH", new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ana Horvat", 1, 0, 3, "AnaMid", "AnaH#EUW1", 0, 3 },
                    { 4, "JovanN", new DateTime(2024, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jovan Nikolić", 3, 0, 4, "JovanBot", "JovanN#EUW1", 1, 5 },
                    { 5, "MarkoPetro", new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Marko Petrović", 4, 1, 0, "MarkoCaptain", "MarkoPetro#EUW1", 0, 7 },
                    { 6, "FilipA", new DateTime(2024, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Filip Aleksić", 1, 2, 2, "FilipSub", "FilipA#EUW1", 0, 3 },
                    { 7, "IvaF", new DateTime(2024, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Iva Filipović", 3, 2, 1, "IvaSub", "IvaF#EUNE1", 1, 2 },
                    { 8, "LukaK", new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Luka Kovač", 0, 0, 1, "LukaTop2", "LukaK#EUW1", 0, 3 },
                    { 9, "PetraN", new DateTime(2024, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Petra Novak", 2, 0, 4, "PetraJungle", "PetraN#EUW1", 0, 5 },
                    { 10, "MarijaB", new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Marija Bjelica", 1, 0, 2, "MarijaMid", "MarijaB#EUW1", 0, 6 },
                    { 11, "NikolaR", new DateTime(2023, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nikola Radivojević", 3, 1, 1, "NikolaBot", "NikolaR#EUW1", 0, 7 },
                    { 12, "SandraT", new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sandra Todorović", 4, 2, 0, "SandraSup", "SandraT#EUNE1", 1, 0 }
                });

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "Id", "CoachId", "IsRosterConfirmed", "ManagerId", "Name", "RegisteredAt" },
                values: new object[,]
                {
                    { 1, 1, true, 1, "Zagreb Dragons", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 2, true, 2, "Split Warriors", new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 3, false, 3, "Rijeka Titans", new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Tournaments",
                columns: new[] { "Id", "Description", "EndDate", "Format", "Name", "PrizePool", "RegistrationDeadline", "StartDate", "Status", "Type", "VenueId" },
                values: new object[,]
                {
                    { 1, "Spring esports tournament", new DateTime(2025, 6, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Spring Split 2025", 25000m, new DateTime(2025, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 0, 1 },
                    { 2, "Main summer tournament", new DateTime(2025, 8, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Summer Championship 2025", 50000m, new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 3, 2 }
                });

            migrationBuilder.InsertData(
                table: "TeamPlayers",
                columns: new[] { "PlayersListId", "TeamsId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 4, 1 },
                    { 5, 1 },
                    { 6, 1 },
                    { 7, 1 },
                    { 8, 2 },
                    { 9, 2 },
                    { 10, 2 },
                    { 11, 2 },
                    { 12, 2 }
                });

            migrationBuilder.InsertData(
                table: "TournamentSponsors",
                columns: new[] { "SponsorsListId", "TournamentId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 2, 2 },
                    { 3, 2 }
                });

            migrationBuilder.InsertData(
                table: "TournamentTeams",
                columns: new[] { "TeamsListId", "TournamentId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 2, 1 },
                    { 2, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TeamPlayers",
                keyColumns: new[] { "PlayersListId", "TeamsId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "TeamPlayers",
                keyColumns: new[] { "PlayersListId", "TeamsId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "TeamPlayers",
                keyColumns: new[] { "PlayersListId", "TeamsId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "TeamPlayers",
                keyColumns: new[] { "PlayersListId", "TeamsId" },
                keyValues: new object[] { 4, 1 });

            migrationBuilder.DeleteData(
                table: "TeamPlayers",
                keyColumns: new[] { "PlayersListId", "TeamsId" },
                keyValues: new object[] { 5, 1 });

            migrationBuilder.DeleteData(
                table: "TeamPlayers",
                keyColumns: new[] { "PlayersListId", "TeamsId" },
                keyValues: new object[] { 6, 1 });

            migrationBuilder.DeleteData(
                table: "TeamPlayers",
                keyColumns: new[] { "PlayersListId", "TeamsId" },
                keyValues: new object[] { 7, 1 });

            migrationBuilder.DeleteData(
                table: "TeamPlayers",
                keyColumns: new[] { "PlayersListId", "TeamsId" },
                keyValues: new object[] { 8, 2 });

            migrationBuilder.DeleteData(
                table: "TeamPlayers",
                keyColumns: new[] { "PlayersListId", "TeamsId" },
                keyValues: new object[] { 9, 2 });

            migrationBuilder.DeleteData(
                table: "TeamPlayers",
                keyColumns: new[] { "PlayersListId", "TeamsId" },
                keyValues: new object[] { 10, 2 });

            migrationBuilder.DeleteData(
                table: "TeamPlayers",
                keyColumns: new[] { "PlayersListId", "TeamsId" },
                keyValues: new object[] { 11, 2 });

            migrationBuilder.DeleteData(
                table: "TeamPlayers",
                keyColumns: new[] { "PlayersListId", "TeamsId" },
                keyValues: new object[] { 12, 2 });

            migrationBuilder.DeleteData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TournamentSponsors",
                keyColumns: new[] { "SponsorsListId", "TournamentId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "TournamentSponsors",
                keyColumns: new[] { "SponsorsListId", "TournamentId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "TournamentSponsors",
                keyColumns: new[] { "SponsorsListId", "TournamentId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "TournamentSponsors",
                keyColumns: new[] { "SponsorsListId", "TournamentId" },
                keyValues: new object[] { 3, 2 });

            migrationBuilder.DeleteData(
                table: "TournamentTeams",
                keyColumns: new[] { "TeamsListId", "TournamentId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "TournamentTeams",
                keyColumns: new[] { "TeamsListId", "TournamentId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "TournamentTeams",
                keyColumns: new[] { "TeamsListId", "TournamentId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "TournamentTeams",
                keyColumns: new[] { "TeamsListId", "TournamentId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tournaments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tournaments",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
