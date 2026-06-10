using League_of_Legends_Tournament_Hosting.Data;
using League_of_Legends_Tournament_Hosting.Models;
using League_of_Legends_Tournament_Hosting.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace League_of_Legends_Tournament_Hosting.Controllers
{
    [Route("tim")]
    [Authorize]
    public class TeamsController : AppControllerBase
    {
        private readonly TournamentDbContext _dbContext;

        public TeamsController(TournamentDbContext dbContext)
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
                new BreadcrumbItem("Teams", null));

            var teams = await _dbContext.Teams
                .Include(t => t.Coach)
                .Include(t => t.Manager)
                .Include(t => t.PlayersList)
                .OrderBy(team => team.Name)
                .ToListAsync();

            var cards = teams
                .Select(team => new EntityCardViewModel(
                    team.Name,
                    $"Coach: {team.Coach.Name}",
                    $"{team.PlayersList.Count} players · Manager: {team.Manager.Name} · {(team.IsRosterConfirmed ? "Roster confirmed" : "Roster open")}",
                    "Open Team",
                    Url.Action(nameof(Details), new { id = team.Id }) ?? "#"))
                .ToList();

            ViewData["PageTitle"] = "Teams";
            ViewData["EntityCount"] = cards.Count;
            return View(cards);
        }

        [Route("detalji/{id:int}")]
        [Route("profil/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var team = await _dbContext.Teams
                .Include(t => t.Coach)
                .Include(t => t.Manager)
                .Include(t => t.PlayersList)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (team is null)
            {
                return NotFound();
            }

            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Teams", Url.Action(nameof(Index))),
                new BreadcrumbItem(team.Name, null));

            var relatedTournaments = await _dbContext.Tournaments
                .Where(t => t.TeamsList.Any(tm => tm.Id == team.Id))
                .ToListAsync();

            ViewData["RelatedTournaments"] = relatedTournaments;
            return View(team);
        }

        [Route("kreiraj")]
        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create()
        {
            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Teams", Url.Action(nameof(Index))),
                new BreadcrumbItem("Create New", null));

            return View(new Team());
        }

        [Route("kreiraj")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create([Bind("Name,CoachId,ManagerId,RegisteredAt")] Team team)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Add(team);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = team.Id });
            }
            return View(team);
        }

        [Route("uredi/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id)
        {
            var team = await _dbContext.Teams
                .Include(t => t.Coach)
                .Include(t => t.Manager)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (team is null)
            {
                return NotFound();
            }

            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Teams", Url.Action(nameof(Index))),
                new BreadcrumbItem(team.Name, Url.Action(nameof(Details), new { id = team.Id })),
                new BreadcrumbItem("Edit", null));

            return View(team);
        }

        [Route("uredi/{id:int}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CoachId,ManagerId,RegisteredAt,IsRosterConfirmed")] Team team)
        {
            if (id != team.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(team);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamExists(team.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Details), new { id = team.Id });
            }

            team.Coach = await _dbContext.Coaches.FindAsync(team.CoachId) ?? new Coach();
            team.Manager = await _dbContext.Managers.FindAsync(team.ManagerId) ?? new Manager();
            return View(team);
        }

        [Route("obrisi/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var team = await _dbContext.Teams
                .Include(t => t.Coach)
                .Include(t => t.Manager)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (team is null)
            {
                return NotFound();
            }

            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Teams", Url.Action(nameof(Index))),
                new BreadcrumbItem(team.Name, Url.Action(nameof(Details), new { id = team.Id })),
                new BreadcrumbItem("Delete", null));

            return View(team);
        }

        [Route("obrisi/{id:int}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var team = await _dbContext.Teams.FindAsync(id);
            if (team is not null)
            {
                _dbContext.Teams.Remove(team);
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

            var searchResults = await _dbContext.Teams
                .AsNoTracking()
                .Include(t => t.Coach)
                .Include(t => t.Manager)
                .Include(t => t.PlayersList)
                .Where(t => t.Name.Contains(query) || t.Coach.Name.Contains(query) || t.Manager.Name.Contains(query))
                .OrderBy(t => t.Name)
                .Select(t => new EntityCardViewModel(
                    t.Name,
                    $"Coach: {t.Coach.Name}",
                    $"{t.PlayersList.Count} players · Manager: {t.Manager.Name} · {(t.IsRosterConfirmed ? "Roster confirmed" : "Roster open")}",
                    "Open Team",
                    Url.Action(nameof(Details), new { id = t.Id }) ?? "#"))
                .ToListAsync();

            return Json(searchResults);
        }

        private bool TeamExists(int id)
        {
            return _dbContext.Teams.Any(e => e.Id == id);
        }
    }
}