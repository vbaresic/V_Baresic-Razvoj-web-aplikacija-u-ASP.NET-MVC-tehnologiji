using League_of_Legends_Tournament_Hosting.Data;
using League_of_Legends_Tournament_Hosting.Models;
using League_of_Legends_Tournament_Hosting.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace League_of_Legends_Tournament_Hosting.Controllers
{
    [Route("menadzer")]
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
    }
}