using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace League_of_Legends_Tournament_Hosting.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coaches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GamerTag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HiredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    YearsOfExperience = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coaches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Managers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HiredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    YearsOfExperience = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Venues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    BookingFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BookingTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Venues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tournaments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Format = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PrizePool = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegistrationDeadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VenueId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournaments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tournaments_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sponsors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SponsorshipAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ContractStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContractEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TournamentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sponsors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sponsors_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoachId = table.Column<int>(type: "int", nullable: false),
                    ManagerId = table.Column<int>(type: "int", nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TournamentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_Coaches_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Teams_Managers_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Managers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Teams_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TournamentSponsors",
                columns: table => new
                {
                    SponsorsListId = table.Column<int>(type: "int", nullable: false),
                    TournamentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentSponsors", x => new { x.SponsorsListId, x.TournamentId });
                    table.ForeignKey(
                        name: "FK_TournamentSponsors_Sponsors_SponsorsListId",
                        column: x => x.SponsorsListId,
                        principalTable: "Sponsors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TournamentSponsors_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GamerTag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    PreferredPosition = table.Column<int>(type: "int", nullable: false),
                    SecondaryPosition = table.Column<int>(type: "int", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountInformation_SummonerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountInformation_RiotTag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountInformation_Region = table.Column<int>(type: "int", nullable: false),
                    AccountInformation_LeagueTier = table.Column<int>(type: "int", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: true),
                    TeamId1 = table.Column<int>(type: "int", nullable: true),
                    TeamId3 = table.Column<int>(type: "int", nullable: true),
                    TeamId4 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Players_Teams_TeamId1",
                        column: x => x.TeamId1,
                        principalTable: "Teams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Players_Teams_TeamId3",
                        column: x => x.TeamId3,
                        principalTable: "Teams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Players_Teams_TeamId4",
                        column: x => x.TeamId4,
                        principalTable: "Teams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TournamentTeams",
                columns: table => new
                {
                    TeamsListId = table.Column<int>(type: "int", nullable: false),
                    TournamentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentTeams", x => new { x.TeamsListId, x.TournamentId });
                    table.ForeignKey(
                        name: "FK_TournamentTeams_Teams_TeamsListId",
                        column: x => x.TeamsListId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TournamentTeams_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamPlayers",
                columns: table => new
                {
                    PlayersListId = table.Column<int>(type: "int", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamPlayers", x => new { x.PlayersListId, x.TeamId });
                    table.ForeignKey(
                        name: "FK_TeamPlayers_Players_PlayersListId",
                        column: x => x.PlayersListId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamPlayers_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Coaches",
                columns: new[] { "Id", "GamerTag", "HiredAt", "Name", "YearsOfExperience" },
                values: new object[,]
                {
                    { 1, "IvanCoach", new DateTime(2022, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ivan Horvat", 5 },
                    { 2, "MarkoPeric", new DateTime(2021, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Marko Perić", 7 },
                    { 3, "LukaN", new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Luka Novak", 3 },
                    { 4, "TomiBlazer", new DateTime(2020, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tomislav Blažević", 9 },
                    { 5, "AnteJ", new DateTime(2022, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ante Jurić", 4 },
                    { 6, "NikoSaric", new DateTime(2019, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nikola Šarić", 11 }
                });

            migrationBuilder.InsertData(
                table: "Managers",
                columns: new[] { "Id", "HiredAt", "Name", "YearsOfExperience" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Petra Kovač", 4 },
                    { 2, new DateTime(2021, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ana Babić", 6 },
                    { 3, new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Maja Tomić", 2 },
                    { 4, new DateTime(2020, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sara Marić", 8 },
                    { 5, new DateTime(2022, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Iva Paulić", 3 },
                    { 6, new DateTime(2019, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dora Knežević", 10 }
                });

            migrationBuilder.InsertData(
                table: "Sponsors",
                columns: new[] { "Id", "ContactEmail", "ContactPhone", "ContractEnd", "ContractStart", "Name", "SponsorshipAmount", "TournamentId", "Website" },
                values: new object[,]
                {
                    { 1, "esports@ht.hr", "+385 1 111 2222", new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "HT Telekom", 5000.00m, null, "https://www.t.ht.hr" },
                    { 2, "sponsorships@razer.com", "+1 800 123 4567", new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Razer", 8000.00m, null, "https://www.razer.com" },
                    { 3, "esports@redbull.com", "+43 662 6582 0", new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Red Bull", 10000.00m, null, "https://www.redbull.com" }
                });

            migrationBuilder.InsertData(
                table: "Venues",
                columns: new[] { "Id", "Address", "BookingFrom", "BookingTo", "Capacity", "City", "ContactEmail", "ContactPhone", "IsAvailable", "Name" },
                values: new object[,]
                {
                    { 1, "Ilica 35", new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 6, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 500, "Zagreb", "contact@zagrebesports.hr", "+385 1 234 5678", true, "Zagreb Esports Arena" },
                    { 2, "Domovinskog rata 12", new DateTime(2025, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 300, "Split", "info@splitgaming.hr", "+385 21 345 6789", true, "Split Gaming Hub" },
                    { 3, "Korzo 5", new DateTime(2025, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 8, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 200, "Rijeka", "hello@rjekalanc.hr", "+385 51 456 7890", true, "Rijeka LAN Center" }
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Sponsors_TournamentId",
                table: "Sponsors",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamPlayers_TeamId",
                table: "TeamPlayers",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_CoachId",
                table: "Teams",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_ManagerId",
                table: "Teams",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_TournamentId",
                table: "Teams",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_VenueId",
                table: "Tournaments",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentSponsors_TournamentId",
                table: "TournamentSponsors",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentTeams_TournamentId",
                table: "TournamentTeams",
                column: "TournamentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamPlayers");

            migrationBuilder.DropTable(
                name: "TournamentSponsors");

            migrationBuilder.DropTable(
                name: "TournamentTeams");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Sponsors");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Coaches");

            migrationBuilder.DropTable(
                name: "Managers");

            migrationBuilder.DropTable(
                name: "Tournaments");

            migrationBuilder.DropTable(
                name: "Venues");
        }
    }
}
