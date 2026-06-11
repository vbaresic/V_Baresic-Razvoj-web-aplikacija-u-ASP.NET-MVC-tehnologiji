using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using League_of_Legends_Tournament_Hosting.DTOs;
using League_of_Legends_Tournament_Hosting.Models;

namespace League_of_Legends_Tournament_Hosting.Tests
{
    public class TournamentsApiTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public TournamentsApiTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        private async Task<int> CreateVenueAsync()
        {
            var venueResponse = await _client.PostAsJsonAsync("/api/venues", new VenueRequest
            {
                Name = "Tournament Venue",
                Address = "456 Esports Avenue",
                City = "Split",
                Capacity = 10000,
                IsAvailable = true,
                BookingFrom = new DateTime(2026, 1, 1),
                BookingTo = new DateTime(2026, 12, 31),
                ContactEmail = "tvenue@example.com",
                ContactPhone = "+385111222333"
            });
            var venue = await venueResponse.Content.ReadFromJsonAsync<VenueDto>();
            return venue!.Id;
        }

        [Fact]
        public async Task FullCrudFlow_WorksCorrectly()
        {
            var venueId = await CreateVenueAsync();

            var request = new TournamentRequest
            {
                Name = "Test Tournament",
                Description = "A tournament created for integration testing.",
                Type = TournamentType.Preliminary,
                Format = TournamentFormat.Online,
                Status = TournamentStatus.Upcoming,
                PrizePool = 50000m,
                StartDate = new DateTime(2026, 6, 1),
                EndDate = new DateTime(2026, 6, 10),
                RegistrationDeadline = new DateTime(2026, 5, 1),
                VenueId = venueId
            };

            // Create
            var createResponse = await _client.PostAsJsonAsync("/api/tournaments", request);
            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
            var created = await createResponse.Content.ReadFromJsonAsync<TournamentDto>();
            Assert.NotNull(created);
            Assert.True(created!.Id > 0);
            Assert.Equal(venueId, created.VenueId);
            Assert.Equal("Preliminary", created.Type);
            Assert.NotNull(created.Venue);
            Assert.Equal(venueId, created.Venue!.Id);
            Assert.Empty(created.Teams);
            Assert.Empty(created.Sponsors);

            // Get by id
            var fetched = await _client.GetFromJsonAsync<TournamentDto>($"/api/tournaments/{created.Id}");
            Assert.NotNull(fetched);
            Assert.Equal("Test Tournament", fetched!.Name);

            // Get all
            var list = await _client.GetFromJsonAsync<List<TournamentDto>>("/api/tournaments");
            Assert.NotNull(list);
            Assert.Contains(list!, t => t.Id == created.Id);

            // Update
            var updateRequest = new TournamentRequest
            {
                Name = "Updated Tournament",
                Description = request.Description,
                Type = TournamentType.Final,
                Format = TournamentFormat.Offline,
                Status = TournamentStatus.Ongoing,
                PrizePool = 75000m,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                RegistrationDeadline = request.RegistrationDeadline,
                VenueId = venueId
            };
            var updateResponse = await _client.PutAsJsonAsync($"/api/tournaments/{created.Id}", updateRequest);
            Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

            var updated = await _client.GetFromJsonAsync<TournamentDto>($"/api/tournaments/{created.Id}");
            Assert.Equal("Updated Tournament", updated!.Name);
            Assert.Equal("Final", updated.Type);
            Assert.Equal("Ongoing", updated.Status);

            // Delete
            var deleteResponse = await _client.DeleteAsync($"/api/tournaments/{created.Id}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            var getAfterDelete = await _client.GetAsync($"/api/tournaments/{created.Id}");
            Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);
        }

        [Fact]
        public async Task CreateTournament_NonExistentVenue_ReturnsBadRequest()
        {
            var request = new TournamentRequest
            {
                Name = "Invalid Tournament",
                Description = "Should fail because the venue does not exist.",
                Type = TournamentType.Preliminary,
                Format = TournamentFormat.Online,
                Status = TournamentStatus.Upcoming,
                PrizePool = 1000m,
                StartDate = new DateTime(2026, 6, 1),
                EndDate = new DateTime(2026, 6, 10),
                RegistrationDeadline = new DateTime(2026, 5, 1),
                VenueId = 999999
            };

            var response = await _client.PostAsJsonAsync("/api/tournaments", request);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetTournament_NonExistentId_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/tournaments/999999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateTournament_NonExistentId_ReturnsNotFound()
        {
            var venueId = await CreateVenueAsync();

            var request = new TournamentRequest
            {
                Name = "Ghost Tournament",
                Description = "A tournament that does not exist.",
                Type = TournamentType.Preliminary,
                Format = TournamentFormat.Online,
                Status = TournamentStatus.Upcoming,
                PrizePool = 50000m,
                StartDate = new DateTime(2026, 6, 1),
                EndDate = new DateTime(2026, 6, 10),
                RegistrationDeadline = new DateTime(2026, 5, 1),
                VenueId = venueId
            };

            var response = await _client.PutAsJsonAsync("/api/tournaments/999999", request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteTournament_NonExistentId_ReturnsNotFound()
        {
            var response = await _client.DeleteAsync("/api/tournaments/999999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateTournament_InvalidData_ReturnsBadRequest()
        {
            var venueId = await CreateVenueAsync();

            var request = new TournamentRequest
            {
                Name = "A", // too short (min 2)
                Description = "Hi", // too short (min 5)
                Type = TournamentType.Preliminary,
                Format = TournamentFormat.Online,
                Status = TournamentStatus.Upcoming,
                PrizePool = -100m, // out of range
                StartDate = new DateTime(2026, 6, 1),
                EndDate = new DateTime(2026, 6, 10),
                RegistrationDeadline = new DateTime(2026, 5, 1),
                VenueId = venueId
            };

            var response = await _client.PostAsJsonAsync("/api/tournaments", request);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DocumentUpload_WorksCorrectly()
        {
            var venueId = await CreateVenueAsync();

            var request = new TournamentRequest
            {
                Name = "Document Test Tournament",
                Description = "Tournament for testing document uploads.",
                Type = TournamentType.Preliminary,
                Format = TournamentFormat.Online,
                Status = TournamentStatus.Upcoming,
                PrizePool = 10000m,
                StartDate = new DateTime(2026, 6, 1),
                EndDate = new DateTime(2026, 6, 10),
                RegistrationDeadline = new DateTime(2026, 5, 1),
                VenueId = venueId
            };

            var createResponse = await _client.PostAsJsonAsync("/api/tournaments", request);
            var tournament = await createResponse.Content.ReadFromJsonAsync<TournamentDto>();

            // Upload document
            using var content = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes("Test document content"));
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");
            content.Add(fileContent, "file", "test-document.txt");

            var uploadResponse = await _client.PostAsync($"/api/tournaments/{tournament!.Id}/documents", content);
            Assert.Equal(HttpStatusCode.Created, uploadResponse.StatusCode);

            // Get documents
            var getResponse = await _client.GetAsync($"/api/tournaments/{tournament.Id}/documents");
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

            var jsonString = await getResponse.Content.ReadAsStringAsync();
            using var jsonDoc = JsonDocument.Parse(jsonString);
            var documentsArray = jsonDoc.RootElement;
            Assert.Equal(1, documentsArray.GetArrayLength());

            var documentId = documentsArray[0].GetProperty("id").GetInt32();

            // Delete document
            var deleteResponse = await _client.DeleteAsync($"/api/tournaments/documents/{documentId}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            // Verify deletion
            var getAfterDelete = await _client.GetAsync($"/api/tournaments/{tournament.Id}/documents");
            var jsonStringAfter = await getAfterDelete.Content.ReadAsStringAsync();
            using var jsonDocAfter = JsonDocument.Parse(jsonStringAfter);
            Assert.Equal(0, jsonDocAfter.RootElement.GetArrayLength());
        }

        [Fact]
        public async Task DocumentUpload_NonExistentTournament_ReturnsNotFound()
        {
            using var content = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes("Test content"));
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");
            content.Add(fileContent, "file", "test.txt");

            var response = await _client.PostAsync("/api/tournaments/999999/documents", content);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetDocuments_NonExistentTournament_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/tournaments/999999/documents");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetDocument_NonExistentId_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/tournaments/documents/999999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteDocument_NonExistentId_ReturnsNotFound()
        {
            var response = await _client.DeleteAsync("/api/tournaments/documents/999999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
