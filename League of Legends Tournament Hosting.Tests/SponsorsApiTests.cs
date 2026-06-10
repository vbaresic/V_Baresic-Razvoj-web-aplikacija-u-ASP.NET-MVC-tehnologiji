using System.Net;
using System.Net.Http.Json;
using League_of_Legends_Tournament_Hosting.DTOs;

namespace League_of_Legends_Tournament_Hosting.Tests
{
    public class SponsorsApiTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public SponsorsApiTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task FullCrudFlow_WorksCorrectly()
        {
            var request = new SponsorRequest
            {
                Name = "Test Sponsor",
                Website = "https://sponsor.example.com",
                ContactEmail = "sponsor@example.com",
                ContactPhone = "+385987654321",
                SponsorshipAmount = 10000m,
                ContractStart = new DateTime(2026, 1, 1),
                ContractEnd = new DateTime(2026, 12, 31)
            };

            // Create
            var createResponse = await _client.PostAsJsonAsync("/api/sponsors", request);
            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
            var created = await createResponse.Content.ReadFromJsonAsync<SponsorDto>();
            Assert.NotNull(created);
            Assert.True(created!.Id > 0);

            // Get by id
            var fetched = await _client.GetFromJsonAsync<SponsorDto>($"/api/sponsors/{created.Id}");
            Assert.NotNull(fetched);
            Assert.Equal("Test Sponsor", fetched!.Name);

            // Get all
            var list = await _client.GetFromJsonAsync<List<SponsorDto>>("/api/sponsors");
            Assert.NotNull(list);
            Assert.Contains(list!, s => s.Id == created.Id);

            // Update
            var updateRequest = new SponsorRequest
            {
                Name = "Updated Sponsor",
                Website = request.Website,
                ContactEmail = request.ContactEmail,
                ContactPhone = request.ContactPhone,
                SponsorshipAmount = 25000m,
                ContractStart = request.ContractStart,
                ContractEnd = request.ContractEnd
            };
            var updateResponse = await _client.PutAsJsonAsync($"/api/sponsors/{created.Id}", updateRequest);
            Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

            var updated = await _client.GetFromJsonAsync<SponsorDto>($"/api/sponsors/{created.Id}");
            Assert.Equal("Updated Sponsor", updated!.Name);
            Assert.Equal(25000m, updated.SponsorshipAmount);

            // Delete
            var deleteResponse = await _client.DeleteAsync($"/api/sponsors/{created.Id}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            var getAfterDelete = await _client.GetAsync($"/api/sponsors/{created.Id}");
            Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);
        }

        [Fact]
        public async Task GetSponsor_NonExistentId_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/sponsors/999999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
