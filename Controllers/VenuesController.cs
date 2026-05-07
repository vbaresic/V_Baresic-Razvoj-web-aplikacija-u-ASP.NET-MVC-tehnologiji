using League_of_Legends_Tournament_Hosting.Data;
using League_of_Legends_Tournament_Hosting.Models;
using League_of_Legends_Tournament_Hosting.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace League_of_Legends_Tournament_Hosting.Controllers
{
    [Route("mjesto")]
    public class VenuesController : AppControllerBase
    {
        private readonly TournamentDbContext _dbContext;

        public VenuesController(TournamentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Route("")]
        [Route("pregled")]
        [Route("sve")]
        public async Task<IActionResult> Index()
        {
            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Venues", null));

            var cards = await _dbContext.Venues
                .AsNoTracking()
                .OrderBy(venue => venue.City)
                .ThenBy(venue => venue.Name)
                .Select(venue => new EntityCardViewModel(
                    venue.Name,
                    venue.City,
                    $"Capacity {venue.Capacity} · Available: {(venue.IsAvailable ? "Yes" : "No")}",
                    "View Details",
                    Url.Action(nameof(Details), new { id = venue.Id }) ?? "#"))
                .ToListAsync();

            ViewData["PageTitle"] = "Venues";
            ViewData["EntityCount"] = cards.Count;
            return View(cards);
        }

        [Route("detalji/{id:int}")]
        [Route("info/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var venue = await _dbContext.Venues
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == id);
            
            if (venue is null)
            {
                return NotFound();
            }

            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Venues", Url.Action(nameof(Index))),
                new BreadcrumbItem(venue.Name, null));

            ViewData["VenueTournaments"] = await _dbContext.Tournaments
                .AsNoTracking()
                .Where(t => t.VenueId == venue.Id)
                .ToListAsync();
            
            return View(venue);
        }
    }
}