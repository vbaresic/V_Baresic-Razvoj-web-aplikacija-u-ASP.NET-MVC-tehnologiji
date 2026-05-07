using Microsoft.EntityFrameworkCore;
using League_of_Legends_Tournament_Hosting.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register EF Core DbContext
builder.Services.AddDbContext<TournamentDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("TournamentDbContext"),
        opt => opt.MigrationsAssembly("League of Legends Tournament Hosting")));

var app = builder.Build();

// Apply migrations and seed data on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TournamentDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
