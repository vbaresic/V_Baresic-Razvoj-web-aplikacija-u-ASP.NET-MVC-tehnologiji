using League_of_Legends_Tournament_Hosting.Data;
using League_of_Legends_Tournament_Hosting.Models;
using League_of_Legends_Tournament_Hosting.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace League_of_Legends_Tournament_Hosting.Controllers
{
    [Route("sponzor")]
    [Authorize]
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
        [AllowAnonymous]
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

        [Route("kreiraj")]
        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create()
        {
            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Sponsors", Url.Action(nameof(Index))),
                new BreadcrumbItem("Create New", null));

            return View(new Sponsor());
        }

        [Route("kreiraj")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create([Bind("Name,Website,ContactEmail,ContactPhone,SponsorshipAmount,ContractStart,ContractEnd")] Sponsor sponsor)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Add(sponsor);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = sponsor.Id });
            }
            return View(sponsor);
        }

        [Route("uredi/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id)
        {
            var sponsor = await _dbContext.Sponsors
                .FirstOrDefaultAsync(s => s.Id == id);
            
            if (sponsor is null)
            {
                return NotFound();
            }

            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Sponsors", Url.Action(nameof(Index))),
                new BreadcrumbItem(sponsor.Name, Url.Action(nameof(Details), new { id = sponsor.Id })),
                new BreadcrumbItem("Edit", null));

            return View(sponsor);
        }

        [Route("uredi/{id:int}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Website,ContactEmail,ContactPhone,SponsorshipAmount,ContractStart,ContractEnd")] Sponsor sponsor)
        {
            if (id != sponsor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(sponsor);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SponsorExists(sponsor.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Details), new { id = sponsor.Id });
            }
            return View(sponsor);
        }

        [Route("obrisi/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var sponsor = await _dbContext.Sponsors
                .FirstOrDefaultAsync(s => s.Id == id);
            
            if (sponsor is null)
            {
                return NotFound();
            }

            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Sponsors", Url.Action(nameof(Index))),
                new BreadcrumbItem(sponsor.Name, Url.Action(nameof(Details), new { id = sponsor.Id })),
                new BreadcrumbItem("Delete", null));

            return View(sponsor);
        }

        [Route("obrisi/{id:int}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sponsor = await _dbContext.Sponsors.FindAsync(id);
            if (sponsor is not null)
            {
                _dbContext.Sponsors.Remove(sponsor);
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

            var searchResults = await _dbContext.Sponsors
                .AsNoTracking()
                .Where(s => s.Name.Contains(query) || s.Website.Contains(query) || s.ContactEmail.Contains(query))
                .OrderBy(s => s.Name)
                .Select(s => new EntityCardViewModel(
                    s.Name,
                    s.Website,
                    s.ContactPhone,
                    "View Details",
                    Url.Action(nameof(Details), new { id = s.Id }) ?? "#"))
                .ToListAsync();

            return Json(searchResults);
        }

        private bool SponsorExists(int id)
        {
            return _dbContext.Sponsors.Any(e => e.Id == id);
        }
    }
}