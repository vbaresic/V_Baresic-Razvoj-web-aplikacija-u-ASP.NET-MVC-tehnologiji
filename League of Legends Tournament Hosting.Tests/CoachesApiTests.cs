using System.Net;
using System.Net.Http.Json;
using League_of_Legends_Tournament_Hosting.DTOs;

namespace League_of_Legends_Tournament_Hosting.Tests
{
    public class CoachesApiTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public CoachesApiTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task FullCrudFlow_WorksCorrectly()
        {
            var request = new CoachRequest
            {
                Name = "Test Coach",
                GamerTag = "TestCoachTag",
                HiredAt = new DateTime(2024, 1, 1),
                YearsOfExperience = 5
            };

            // Create
            var createResponse = await _client.PostAsJsonAsync("/api/coaches", request);
            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
            var created = await createResponse.Content.ReadFromJsonAsync<CoachDto>();
            Assert.NotNull(created);
            Assert.True(created!.Id > 0);
            Assert.Equal(request.Name, created.Name);

            // Get by id
            var getResponse = await _client.GetAsync($"/api/coaches/{created.Id}");
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            var fetched = await getResponse.Content.ReadFromJsonAsync<CoachDto>();
            Assert.NotNull(fetched);
            Assert.Equal(created.Id, fetched!.Id);

            // Get all
            var listResponse = await _client.GetAsync("/api/coaches");
            Assert.Equal(HttpStatusCode.OK, listResponse.StatusCode);
            var list = await listResponse.Content.ReadFromJsonAsync<List<CoachDto>>();
            Assert.NotNull(list);
            Assert.Contains(list!, c => c.Id == created.Id);

            // Update
            var updateRequest = new CoachRequest
            {
                Name = "Updated Coach",
                GamerTag = "UpdatedTag",
                HiredAt = request.HiredAt,
                YearsOfExperience = 7
            };
            var updateResponse = await _client.PutAsJsonAsync($"/api/coaches/{created.Id}", updateRequest);
            Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

            var getAfterUpdate = await _client.GetFromJsonAsync<CoachDto>($"/api/coaches/{created.Id}");
            Assert.NotNull(getAfterUpdate);
            Assert.Equal("Updated Coach", getAfterUpdate!.Name);
            Assert.Equal(7, getAfterUpdate.YearsOfExperience);

            // Delete
            var deleteResponse = await _client.DeleteAsync($"/api/coaches/{created.Id}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            // Get after delete -> 404
            var getAfterDelete = await _client.GetAsync($"/api/coaches/{created.Id}");
            Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);
        }

        [Fact]
        public async Task GetCoach_NonExistentId_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/coaches/999999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateCoach_InvalidData_ReturnsBadRequest()
        {
            var invalidRequest = new CoachRequest
            {
                Name = "A", // too short
                GamerTag = "B", // too short
                HiredAt = new DateTime(2024, 1, 1),
                YearsOfExperience = -1 // out of range
            };

            var response = await _client.PostAsJsonAsync("/api/coaches", invalidRequest);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
