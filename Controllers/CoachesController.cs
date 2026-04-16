using League_of_Legends_Tournament_Hosting.Models;
using League_of_Legends_Tournament_Hosting.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace League_of_Legends_Tournament_Hosting.Controllers
{
    public class CoachesController : AppControllerBase
    {
        public IActionResult Index()
        {
            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Coaches", null));

            var cards = MockRepository.Coaches
                .OrderBy(coach => coach.Name)
                .Select(coach => new EntityCardViewModel(
                    coach.Name,
                    coach.GamerTag,
                    $"{coach.YearsOfExperience} years of experience · Hired {coach.HiredAt:dd MMM yyyy}",
                    "View Details",
                    Url.Action(nameof(Details), new { id = coach.Id }) ?? "#"))
                .ToList();

            ViewData["PageTitle"] = "Coaches";
            ViewData["EntityCount"] = cards.Count;
            return View(cards);
        }

        public IActionResult Details(int id)
        {
            var coach = MockRepository.GetCoach(id);
            if (coach is null)
            {
                return NotFound();
            }

            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Coaches", Url.Action(nameof(Index))),
                new BreadcrumbItem(coach.Name, null));

            ViewData["CoachTeam"] = MockRepository.Teams.FirstOrDefault(team => team.Coach.Id == coach.Id);
            return View(coach);
        }
    }
}