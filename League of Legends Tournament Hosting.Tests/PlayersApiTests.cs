using System.Net;
using System.Net.Http.Json;
using League_of_Legends_Tournament_Hosting.DTOs;
using League_of_Legends_Tournament_Hosting.Models;

namespace League_of_Legends_Tournament_Hosting.Tests
{
    public class PlayersApiTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public PlayersApiTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task FullCrudFlow_WorksCorrectly()
        {
            var request = new PlayerRequest
            {
                Name = "Test Player",
                GamerTag = "TestGamerTag",
                Role = PlayerRole.Player,
                PreferredPosition = Position.MidLane,
                SecondaryPosition = Position.TopLane,
                JoinedAt = new DateTime(2025, 1, 1),
                SummonerName = "TestSummoner",
                RiotTag = "EUW1",
                Region = Region.EUW,
                LeagueTier = LeagueTier.Gold
            };

            // Create
            var createResponse = await _client.PostAsJsonAsync("/api/players", request);
            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
            var created = await createResponse.Content.ReadFromJsonAsync<PlayerDto>();
            Assert.NotNull(created);
            Assert.True(created!.Id > 0);
            Assert.Equal("EUW", created.AccountInformation.Region);
            Assert.Equal("Gold", created.AccountInformation.LeagueTier);

            // Get by id
            var fetched = await _client.GetFromJsonAsync<PlayerDto>($"/api/players/{created.Id}");
            Assert.NotNull(fetched);
            Assert.Equal("Test Player", fetched!.Name);

            // Get all
            var list = await _client.GetFromJsonAsync<List<PlayerDto>>("/api/players");
            Assert.NotNull(list);
            Assert.Contains(list!, p => p.Id == created.Id);

            // Update
            var updateRequest = new PlayerRequest
            {
                Name = "Updated Player",
                GamerTag = "UpdatedTag",
                Role = PlayerRole.TeamCaptain,
                PreferredPosition = Position.ADC,
                SecondaryPosition = Position.Support,
                JoinedAt = request.JoinedAt,
                SummonerName = "UpdatedSummoner",
                RiotTag = "EUW2",
                Region = Region.EUNE,
                LeagueTier = LeagueTier.Platinum
            };
            var updateResponse = await _client.PutAsJsonAsync($"/api/players/{created.Id}", updateRequest);
            Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

            var updated = await _client.GetFromJsonAsync<PlayerDto>($"/api/players/{created.Id}");
            Assert.Equal("Updated Player", updated!.Name);
            Assert.Equal("TeamCaptain", updated.Role);
            Assert.Equal("EUNE", updated.AccountInformation.Region);

            // Delete
            var deleteResponse = await _client.DeleteAsync($"/api/players/{created.Id}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            var getAfterDelete = await _client.GetAsync($"/api/players/{created.Id}");
            Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);
        }

        [Fact]
        public async Task GetPlayer_NonExistentId_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/players/999999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
