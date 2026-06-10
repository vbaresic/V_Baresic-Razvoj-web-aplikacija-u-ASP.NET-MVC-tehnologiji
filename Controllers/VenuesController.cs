using League_of_Legends_Tournament_Hosting.Data;
using League_of_Legends_Tournament_Hosting.Models;
using League_of_Legends_Tournament_Hosting.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace League_of_Legends_Tournament_Hosting.Controllers
{
    [Route("mjesto")]
    [Authorize]
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
        [AllowAnonymous]
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

        [Route("kreiraj")]
        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create()
        {
            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Venues", Url.Action(nameof(Index))),
                new BreadcrumbItem("Create New", null));

            return View(new Venue());
        }

        [Route("kreiraj")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create([Bind("Name,City,Address,Capacity,IsAvailable")] Venue venue)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Add(venue);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = venue.Id });
            }
            return View(venue);
        }

        [Route("uredi/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id)
        {
            var venue = await _dbContext.Venues
                .FirstOrDefaultAsync(v => v.Id == id);
            
            if (venue is null)
            {
                return NotFound();
            }

            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Venues", Url.Action(nameof(Index))),
                new BreadcrumbItem(venue.Name, Url.Action(nameof(Details), new { id = venue.Id })),
                new BreadcrumbItem("Edit", null));

            return View(venue);
        }

        [Route("uredi/{id:int}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,City,Address,Capacity,IsAvailable")] Venue venue)
        {
            if (id != venue.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(venue);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenueExists(venue.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Details), new { id = venue.Id });
            }
            return View(venue);
        }

        [Route("obrisi/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var venue = await _dbContext.Venues
                .FirstOrDefaultAsync(v => v.Id == id);
            
            if (venue is null)
            {
                return NotFound();
            }

            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Venues", Url.Action(nameof(Index))),
                new BreadcrumbItem(venue.Name, Url.Action(nameof(Details), new { id = venue.Id })),
                new BreadcrumbItem("Delete", null));

            return View(venue);
        }

        [Route("obrisi/{id:int}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venue = await _dbContext.Venues.FindAsync(id);
            if (venue is not null)
            {
                _dbContext.Venues.Remove(venue);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [Route("pretraga")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest();
            }

            var searchResults = await _dbContext.Venues
                .AsNoTracking()
                .Where(v => v.Name.Contains(query) || v.City.Contains(query))
                .OrderBy(v => v.City)
                .ThenBy(v => v.Name)
                .Select(v => new EntityCardViewModel(
                    v.Name,
                    v.City,
                    $"Capacity {v.Capacity} · Available: {(v.IsAvailable ? "Yes" : "No")}",
                    "View Details",
                    Url.Action(nameof(Details), new { id = v.Id }) ?? "#"))
                .ToListAsync();

            return Json(searchResults);
        }

        [Route("autocomplete")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Autocomplete(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Json(Array.Empty<object>());

            var results = await _dbContext.Venues
                .AsNoTracking()
                .Where(v => v.Name.Contains(query) || v.City.Contains(query))
                .OrderBy(v => v.Name)
                .Take(10)
                .Select(v => new { id = v.Id, label = $"{v.Name} — {v.City}" })
                .ToListAsync();

            return Json(results);
        }

        private bool VenueExists(int id)
        {
            return _dbContext.Venues.Any(e => e.Id == id);
        }
    }
}