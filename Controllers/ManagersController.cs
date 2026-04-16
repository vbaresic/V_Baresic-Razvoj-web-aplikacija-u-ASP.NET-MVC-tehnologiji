using League_of_Legends_Tournament_Hosting.Models;
using League_of_Legends_Tournament_Hosting.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace League_of_Legends_Tournament_Hosting.Controllers
{
    public class ManagersController : AppControllerBase
    {
        public IActionResult Index()
        {
            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Managers", null));

            var cards = MockRepository.Managers
                .OrderBy(manager => manager.Name)
                .Select(manager => new EntityCardViewModel(
                    manager.Name,
                    "Team manager",
                    $"{manager.YearsOfExperience} years of experience · Hired {manager.HiredAt:dd MMM yyyy}",
                    "View Details",
                    Url.Action(nameof(Details), new { id = manager.Id }) ?? "#"))
                .ToList();

            ViewData["PageTitle"] = "Managers";
            ViewData["EntityCount"] = cards.Count;
            return View(cards);
        }

        public IActionResult Details(int id)
        {
            var manager = MockRepository.GetManager(id);
            if (manager is null)
            {
                return NotFound();
            }

            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Managers", Url.Action(nameof(Index))),
                new BreadcrumbItem(manager.Name, null));

            ViewData["ManagerTeam"] = MockRepository.Teams.FirstOrDefault(team => team.Manager.Id == manager.Id);
            return View(manager);
        }
    }
}