using League_of_Legends_Tournament_Hosting.Data;
using League_of_Legends_Tournament_Hosting.Models;
using League_of_Legends_Tournament_Hosting.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace League_of_Legends_Tournament_Hosting.Controllers
{
    [Route("trener")]
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
    }
}