using League_of_Legends_Tournament_Hosting.Data;
using League_of_Legends_Tournament_Hosting.Models;
using League_of_Legends_Tournament_Hosting.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace League_of_Legends_Tournament_Hosting.Controllers
{
    [Route("sponzor")]
    public class SponsorsController : AppControllerBase
    {
        private readonly TournamentDbContext _dbContext;

        public SponsorsController(TournamentDbContext dbContext)
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
                new BreadcrumbItem("Sponsors", null));

            var cards = await _dbContext.Sponsors
                .AsNoTracking()
                .OrderBy(sponsor => sponsor.Name)
                .Select(sponsor => new EntityCardViewModel(
                    sponsor.Name,
                    sponsor.Website,
                    sponsor.ContactPhone,
                    "View Details",
                    Url.Action(nameof(Details), new { id = sponsor.Id }) ?? "#"))
                .ToListAsync();

            ViewData["PageTitle"] = "Sponsors";
            ViewData["EntityCount"] = cards.Count;
            return View(cards);
        }

        [Route("detalji/{id:int}")]
        [Route("info/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var sponsor = await _dbContext.Sponsors
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
            
            if (sponsor is null)
            {
                return NotFound();
            }

            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Sponsors", Url.Action(nameof(Index))),
                new BreadcrumbItem(sponsor.Name, null));

            ViewData["SponsorTournaments"] = await _dbContext.Tournaments
                .AsNoTracking()
                .Where(t => t.SponsorsList.Any(s => s.Id == sponsor.Id))
                .ToListAsync();
            
            return View(sponsor);
        }
    }
}