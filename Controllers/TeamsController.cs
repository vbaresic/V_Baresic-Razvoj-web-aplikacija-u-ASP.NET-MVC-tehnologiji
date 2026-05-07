using League_of_Legends_Tournament_Hosting.Data;
using League_of_Legends_Tournament_Hosting.Models;
using League_of_Legends_Tournament_Hosting.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace League_of_Legends_Tournament_Hosting.Controllers
{
    [Route("tim")]
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

            // Get tournaments where this team is registered
            var relatedTournaments = await _dbContext.Tournaments
                .Where(t => t.TeamsList.Any(tm => tm.Id == team.Id))
                .ToListAsync();

            ViewData["RelatedTournaments"] = relatedTournaments;
            return View(team);
        }
    }
}