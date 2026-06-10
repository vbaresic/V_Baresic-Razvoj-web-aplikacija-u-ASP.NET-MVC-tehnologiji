using League_of_Legends_Tournament_Hosting.Data;
using League_of_Legends_Tournament_Hosting.Models;
using League_of_Legends_Tournament_Hosting.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace League_of_Legends_Tournament_Hosting.Controllers
{
    [Route("menadzer")]
    [Authorize]
    public class ManagersController : AppControllerBase
    {
        private readonly TournamentDbContext _dbContext;

        public ManagersController(TournamentDbContext dbContext)
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
                new BreadcrumbItem("Managers", null));

            var cards = await _dbContext.Managers
                .AsNoTracking()
                .OrderBy(manager => manager.Name)
                .Select(manager => new EntityCardViewModel(
                    manager.Name,
                    "Team manager",
                    $"{manager.YearsOfExperience} years of experience · Hired {manager.HiredAt:dd MMM yyyy}",
                    "View Details",
                    Url.Action(nameof(Details), new { id = manager.Id }) ?? "#"))
                .ToListAsync();

            ViewData["PageTitle"] = "Managers";
            ViewData["EntityCount"] = cards.Count;
            return View(cards);
        }

        [Route("detalji/{id:int}")]
        [Route("profil/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var manager = await _dbContext.Managers
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (manager is null)
            {
                return NotFound();
            }

            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Managers", Url.Action(nameof(Index))),
                new BreadcrumbItem(manager.Name, null));

            var managerTeam = await _dbContext.Teams
                .AsNoTracking()
                .FirstOrDefaultAsync(team => team.ManagerId == manager.Id);
            
            ViewData["ManagerTeam"] = managerTeam;
            return View(manager);
        }

        [Route("kreiraj")]
        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create()
        {
            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Managers", Url.Action(nameof(Index))),
                new BreadcrumbItem("Create New", null));

            return View(new Manager());
        }

        [Route("kreiraj")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create([Bind("Name,HiredAt,YearsOfExperience")] Manager manager)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Add(manager);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = manager.Id });
            }
            return View(manager);
        }

        [Route("uredi/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id)
        {
            var manager = await _dbContext.Managers
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (manager is null)
            {
                return NotFound();
            }

            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Managers", Url.Action(nameof(Index))),
                new BreadcrumbItem(manager.Name, Url.Action(nameof(Details), new { id = manager.Id })),
                new BreadcrumbItem("Edit", null));

            return View(manager);
        }

        [Route("uredi/{id:int}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,HiredAt,YearsOfExperience")] Manager manager)
        {
            if (id != manager.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(manager);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManagerExists(manager.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Details), new { id = manager.Id });
            }
            return View(manager);
        }

        [Route("obrisi/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var manager = await _dbContext.Managers
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (manager is null)
            {
                return NotFound();
            }

            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Managers", Url.Action(nameof(Index))),
                new BreadcrumbItem(manager.Name, Url.Action(nameof(Details), new { id = manager.Id })),
                new BreadcrumbItem("Delete", null));

            return View(manager);
        }

        [Route("obrisi/{id:int}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var manager = await _dbContext.Managers.FindAsync(id);
            if (manager is not null)
            {
                _dbContext.Managers.Remove(manager);
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

            var searchResults = await _dbContext.Managers
                .AsNoTracking()
                .Where(m => m.Name.Contains(query))
                .OrderBy(m => m.Name)
                .Select(m => new EntityCardViewModel(
                    m.Name,
                    "Team manager",
                    $"{m.YearsOfExperience} years of experience · Hired {m.HiredAt:dd MMM yyyy}",
                    "View Details",
                    Url.Action(nameof(Details), new { id = m.Id }) ?? "#"))
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

            var results = await _dbContext.Managers
                .AsNoTracking()
                .Where(m => m.Name.Contains(query))
                .OrderBy(m => m.Name)
                .Take(10)
                .Select(m => new { id = m.Id, label = m.Name })
                .ToListAsync();

            return Json(results);
        }

        private bool ManagerExists(int id)
        {
            return _dbContext.Managers.Any(e => e.Id == id);
        }
    }
}