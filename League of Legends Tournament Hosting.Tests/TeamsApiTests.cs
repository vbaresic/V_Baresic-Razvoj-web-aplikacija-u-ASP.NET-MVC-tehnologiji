using System.Net;
using System.Net.Http.Json;
using League_of_Legends_Tournament_Hosting.DTOs;

namespace League_of_Legends_Tournament_Hosting.Tests
{
    public class TeamsApiTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public TeamsApiTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        private async Task<(int CoachId, int ManagerId)> CreatePrerequisitesAsync()
        {
            var coachResponse = await _client.PostAsJsonAsync("/api/coaches", new CoachRequest
            {
                Name = "Team Coach",
                GamerTag = "TeamCoachTag",
                HiredAt = new DateTime(2024, 1, 1),
                YearsOfExperience = 4
            });
            var coach = await coachResponse.Content.ReadFromJsonAsync<CoachDto>();

            var managerResponse = await _client.PostAsJsonAsync("/api/managers", new ManagerRequest
            {
                Name = "Team Manager",
                HiredAt = new DateTime(2024, 1, 1),
                YearsOfExperience = 2
            });
            var manager = await managerResponse.Content.ReadFromJsonAsync<ManagerDto>();

            return (coach!.Id, manager!.Id);
        }

        [Fact]
        public async Task FullCrudFlow_WorksCorrectly()
        {
            var (coachId, managerId) = await CreatePrerequisitesAsync();

            var request = new TeamRequest
            {
                Name = "Test Team",
                CoachId = coachId,
                ManagerId = managerId,
                RegisteredAt = new DateTime(2025, 1, 1),
                IsRosterConfirmed = false
            };

            // Create
            var createResponse = await _client.PostAsJsonAsync("/api/teams", request);
            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
            var created = await createResponse.Content.ReadFromJsonAsync<TeamDto>();
            Assert.NotNull(created);
            Assert.True(created!.Id > 0);
            Assert.Equal(coachId, created.CoachId);
            Assert.Equal(managerId, created.ManagerId);

            // Get by id
            var fetched = await _client.GetFromJsonAsync<TeamDto>($"/api/teams/{created.Id}");
            Assert.NotNull(fetched);
            Assert.Equal("Test Team", fetched!.Name);

            // Get all
            var list = await _client.GetFromJsonAsync<List<TeamDto>>("/api/teams");
            Assert.NotNull(list);
            Assert.Contains(list!, t => t.Id == created.Id);

            // Update
            var updateRequest = new TeamRequest
            {
                Name = "Updated Team",
                CoachId = coachId,
                ManagerId = managerId,
                RegisteredAt = request.RegisteredAt,
                IsRosterConfirmed = true
            };
            var updateResponse = await _client.PutAsJsonAsync($"/api/teams/{created.Id}", updateRequest);
            Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

            var updated = await _client.GetFromJsonAsync<TeamDto>($"/api/teams/{created.Id}");
            Assert.Equal("Updated Team", updated!.Name);
            Assert.True(updated.IsRosterConfirmed);

            // Delete
            var deleteResponse = await _client.DeleteAsync($"/api/teams/{created.Id}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            var getAfterDelete = await _client.GetAsync($"/api/teams/{created.Id}");
            Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);
        }

        [Fact]
        public async Task CreateTeam_NonExistentCoach_ReturnsBadRequest()
        {
            var (_, managerId) = await CreatePrerequisitesAsync();

            var request = new TeamRequest
            {
                Name = "Invalid Team",
                CoachId = 999999,
                ManagerId = managerId,
                RegisteredAt = new DateTime(2025, 1, 1),
                IsRosterConfirmed = false
            };

            var response = await _client.PostAsJsonAsync("/api/teams", request);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetTeam_NonExistentId_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/teams/999999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
