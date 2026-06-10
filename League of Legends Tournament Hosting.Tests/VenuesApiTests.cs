using System.Net;
using System.Net.Http.Json;
using League_of_Legends_Tournament_Hosting.DTOs;

namespace League_of_Legends_Tournament_Hosting.Tests
{
    public class VenuesApiTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public VenuesApiTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task FullCrudFlow_WorksCorrectly()
        {
            var request = new VenueRequest
            {
                Name = "Test Arena",
                Address = "123 Esports Street",
                City = "Zagreb",
                Capacity = 5000,
                IsAvailable = true,
                BookingFrom = new DateTime(2026, 1, 1),
                BookingTo = new DateTime(2026, 12, 31),
                ContactEmail = "venue@example.com",
                ContactPhone = "+385123456789"
            };

            // Create
            var createResponse = await _client.PostAsJsonAsync("/api/venues", request);
            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
            var created = await createResponse.Content.ReadFromJsonAsync<VenueDto>();
            Assert.NotNull(created);
            Assert.True(created!.Id > 0);

            // Get by id
            var fetched = await _client.GetFromJsonAsync<VenueDto>($"/api/venues/{created.Id}");
            Assert.NotNull(fetched);
            Assert.Equal("Test Arena", fetched!.Name);

            // Get all (search by city)
            var list = await _client.GetFromJsonAsync<List<VenueDto>>("/api/venues?search=Zagreb");
            Assert.NotNull(list);
            Assert.Contains(list!, v => v.Id == created.Id);

            // Update
            var updateRequest = new VenueRequest
            {
                Name = "Updated Arena",
                Address = request.Address,
                City = request.City,
                Capacity = 8000,
                IsAvailable = false,
                BookingFrom = request.BookingFrom,
                BookingTo = request.BookingTo,
                ContactEmail = request.ContactEmail,
                ContactPhone = request.ContactPhone
            };
            var updateResponse = await _client.PutAsJsonAsync($"/api/venues/{created.Id}", updateRequest);
            Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

            var updated = await _client.GetFromJsonAsync<VenueDto>($"/api/venues/{created.Id}");
            Assert.Equal("Updated Arena", updated!.Name);
            Assert.Equal(8000, updated.Capacity);

            // Delete
            var deleteResponse = await _client.DeleteAsync($"/api/venues/{created.Id}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            var getAfterDelete = await _client.GetAsync($"/api/venues/{created.Id}");
            Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);
        }

        [Fact]
        public async Task GetVenue_NonExistentId_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/venues/999999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateVenue_InvalidData_ReturnsBadRequest()
        {
            var invalidRequest = new VenueRequest
            {
                Name = "A", // too short
                Address = "B", // too short
                City = "Zagreb",
                Capacity = 0, // out of range
                IsAvailable = true,
                BookingFrom = new DateTime(2026, 1, 1),
                BookingTo = new DateTime(2026, 12, 31),
                ContactEmail = "not-an-email", // invalid email
                ContactPhone = "+385123456789"
            };

            var response = await _client.PostAsJsonAsync("/api/venues", invalidRequest);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
