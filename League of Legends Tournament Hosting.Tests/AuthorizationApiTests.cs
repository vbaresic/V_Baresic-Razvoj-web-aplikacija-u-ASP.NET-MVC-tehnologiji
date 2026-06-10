using System.Net;
using System.Net.Http.Json;
using League_of_Legends_Tournament_Hosting.DTOs;

namespace League_of_Legends_Tournament_Hosting.Tests
{
    public class AuthorizationApiTests : IClassFixture<NoRoleWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public AuthorizationApiTests(NoRoleWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateCoach_AsUserWithoutRequiredRole_ReturnsForbidden()
        {
            var request = new CoachRequest
            {
                Name = "Forbidden Coach",
                GamerTag = "ForbiddenTag",
                HiredAt = new DateTime(2024, 1, 1),
                YearsOfExperience = 5
            };

            var response = await _client.PostAsJsonAsync("/api/coaches", request);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
