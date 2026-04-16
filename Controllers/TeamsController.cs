using League_of_Legends_Tournament_Hosting.Models;
using League_of_Legends_Tournament_Hosting.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace League_of_Legends_Tournament_Hosting.Controllers
{
    public class TeamsController : AppControllerBase
    {
        public IActionResult Index()
        {
            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Teams", null));

            var cards = MockRepository.Teams
                .OrderBy(team => team.Name)
                .Select(team => new EntityCardViewModel(
                    team.Name,
                    $"Coach: {team.Coach.Name}",
                    $"{team.Players.Count} players · Manager: {team.Manager.Name} · {(team.IsRosterConfirmed ? "Roster confirmed" : "Roster open")}",
                    "Open Team",
                    Url.Action(nameof(Details), new { id = team.Id }) ?? "#"))
                .ToList();

            ViewData["PageTitle"] = "Teams";
            ViewData["EntityCount"] = cards.Count;
            return View(cards);
        }

        public IActionResult Details(int id)
        {
            var team = MockRepository.GetTeam(id);
            if (team is null)
            {
                return NotFound();
            }

            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Teams", Url.Action(nameof(Index))),
                new BreadcrumbItem(team.Name, null));

            ViewData["RelatedTournaments"] = MockRepository.GetTournamentsForTeam(team.Id).ToList();
            return View(team);
        }
    }
}