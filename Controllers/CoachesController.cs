using League_of_Legends_Tournament_Hosting.Data;
using League_of_Legends_Tournament_Hosting.Models;
using League_of_Legends_Tournament_Hosting.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace League_of_Legends_Tournament_Hosting.Controllers
{
    [Route("trener")]
    [Authorize]
    public class CoachesController : AppControllerBase
    {
        private readonly TournamentDbContext _dbContext;

        public CoachesController(TournamentDbContext dbContext)
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
                new BreadcrumbItem("Coaches", null));

            var cards = await _dbContext.Coaches
                .AsNoTracking()
                .OrderBy(coach => coach.Name)
                .Select(coach => new EntityCardViewModel(
                    coach.Name,
                    coach.GamerTag,
                    $"{coach.YearsOfExperience} years of experience · Hired {coach.HiredAt:dd MMM yyyy}",
                    "View Details",
                    Url.Action(nameof(Details), new { id = coach.Id }) ?? "#"))
                .ToListAsync();

            ViewData["PageTitle"] = "Coaches";
            ViewData["EntityCount"] = cards.Count;
            return View(cards);
        }

        [Route("detalji/{id:int}")]
        [Route("profil/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var coach = await _dbContext.Coaches
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
            
            if (coach is null)
            {
                return NotFound();
            }

            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Coaches", Url.Action(nameof(Index))),
                new BreadcrumbItem(coach.Name, null));

            var coachTeam = await _dbContext.Teams
                .AsNoTracking()
                .FirstOrDefaultAsync(team => team.CoachId == coach.Id);
            
            ViewData["CoachTeam"] = coachTeam;
            return View(coach);
        }

        [Route("kreiraj")]
        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create()
        {
            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Coaches", Url.Action(nameof(Index))),
                new BreadcrumbItem("Create New", null));

            return View(new Coach());
        }

        [Route("kreiraj")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create([Bind("Name,GamerTag,HiredAt,YearsOfExperience")] Coach coach)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Add(coach);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = coach.Id });
            }
            return View(coach);
        }

        [Route("uredi/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id)
        {
            var coach = await _dbContext.Coaches
                .FirstOrDefaultAsync(c => c.Id == id);
            
            if (coach is null)
            {
                return NotFound();
            }

            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Coaches", Url.Action(nameof(Index))),
                new BreadcrumbItem(coach.Name, Url.Action(nameof(Details), new { id = coach.Id })),
                new BreadcrumbItem("Edit", null));

            return View(coach);
        }

        [Route("uredi/{id:int}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,GamerTag,HiredAt,YearsOfExperience")] Coach coach)
        {
            if (id != coach.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(coach);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoachExists(coach.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Details), new { id = coach.Id });
            }
            return View(coach);
        }

        [Route("obrisi/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var coach = await _dbContext.Coaches
                .FirstOrDefaultAsync(c => c.Id == id);
            
            if (coach is null)
            {
                return NotFound();
            }

            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Coaches", Url.Action(nameof(Index))),
                new BreadcrumbItem(coach.Name, Url.Action(nameof(Details), new { id = coach.Id })),
                new BreadcrumbItem("Delete", null));

            return View(coach);
        }

        [Route("obrisi/{id:int}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coach = await _dbContext.Coaches.FindAsync(id);
            if (coach is not null)
            {
                _dbContext.Coaches.Remove(coach);
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

            var searchResults = await _dbContext.Coaches
                .AsNoTracking()
                .Where(c => c.Name.Contains(query) || c.GamerTag.Contains(query))
                .OrderBy(c => c.Name)
                .Select(c => new EntityCardViewModel(
                    c.Name,
                    c.GamerTag,
                    $"{c.YearsOfExperience} years of experience · Hired {c.HiredAt:dd MMM yyyy}",
                    "View Details",
                    Url.Action(nameof(Details), new { id = c.Id }) ?? "#"))
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

            var results = await _dbContext.Coaches
                .AsNoTracking()
                .Where(c => c.Name.Contains(query) || c.GamerTag.Contains(query))
                .OrderBy(c => c.Name)
                .Take(10)
                .Select(c => new { id = c.Id, label = $"{c.Name} ({c.GamerTag})" })
                .ToListAsync();

            return Json(results);
        }

        private bool CoachExists(int id)
        {
            return _dbContext.Coaches.Any(e => e.Id == id);
        }
    }
}