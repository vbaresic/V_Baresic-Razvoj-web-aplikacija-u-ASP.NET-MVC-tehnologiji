using System.Net;
using System.Net.Http.Json;
using League_of_Legends_Tournament_Hosting.DTOs;

namespace League_of_Legends_Tournament_Hosting.Tests
{
    public class ManagersApiTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ManagersApiTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task FullCrudFlow_WorksCorrectly()
        {
            var request = new ManagerRequest
            {
                Name = "Test Manager",
                HiredAt = new DateTime(2023, 5, 1),
                YearsOfExperience = 3
            };

            // Create
            var createResponse = await _client.PostAsJsonAsync("/api/managers", request);
            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
            var created = await createResponse.Content.ReadFromJsonAsync<ManagerDto>();
            Assert.NotNull(created);
            Assert.True(created!.Id > 0);

            // Get by id
            var fetched = await _client.GetFromJsonAsync<ManagerDto>($"/api/managers/{created.Id}");
            Assert.NotNull(fetched);
            Assert.Equal("Test Manager", fetched!.Name);

            // Get all
            var list = await _client.GetFromJsonAsync<List<ManagerDto>>("/api/managers");
            Assert.NotNull(list);
            Assert.Contains(list!, m => m.Id == created.Id);

            // Update
            var updateRequest = new ManagerRequest
            {
                Name = "Updated Manager",
                HiredAt = request.HiredAt,
                YearsOfExperience = 8
            };
            var updateResponse = await _client.PutAsJsonAsync($"/api/managers/{created.Id}", updateRequest);
            Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

            var updated = await _client.GetFromJsonAsync<ManagerDto>($"/api/managers/{created.Id}");
            Assert.Equal("Updated Manager", updated!.Name);
            Assert.Equal(8, updated.YearsOfExperience);

            // Delete
            var deleteResponse = await _client.DeleteAsync($"/api/managers/{created.Id}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            var getAfterDelete = await _client.GetAsync($"/api/managers/{created.Id}");
            Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);
        }

        [Fact]
        public async Task GetManager_NonExistentId_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/managers/999999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateManager_InvalidData_ReturnsBadRequest()
        {
            var invalidRequest = new ManagerRequest
            {
                Name = "A", // too short
                HiredAt = new DateTime(2023, 5, 1),
                YearsOfExperience = -1 // out of range
            };

            var response = await _client.PostAsJsonAsync("/api/managers", invalidRequest);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
