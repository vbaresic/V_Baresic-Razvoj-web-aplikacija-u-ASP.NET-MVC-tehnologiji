using League_of_Legends_Tournament_Hosting.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace League_of_Legends_Tournament_Hosting.Tests
{
    public class NoRoleWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly string _databaseName = $"TestDb_{Guid.NewGuid()}";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptorsToRemove = services.Where(d =>
                    d.ServiceType == typeof(DbContextOptions<TournamentDbContext>) ||
                    d.ServiceType == typeof(DbContextOptions) ||
                    (d.ServiceType.IsGenericType && d.ServiceType.GetGenericTypeDefinition().Name.Contains("IDbContextOptionsConfiguration"))
                ).ToList();

                foreach (var descriptor in descriptorsToRemove)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<TournamentDbContext>(options =>
                {
                    options.UseInMemoryDatabase(_databaseName);
                });

                services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = NoRoleAuthHandler.AuthenticationScheme;
                        options.DefaultChallengeScheme = NoRoleAuthHandler.AuthenticationScheme;
                        options.DefaultScheme = NoRoleAuthHandler.AuthenticationScheme;
                    })
                    .AddScheme<AuthenticationSchemeOptions, NoRoleAuthHandler>(
                        NoRoleAuthHandler.AuthenticationScheme, options => { });
            });
        }
    }
}
