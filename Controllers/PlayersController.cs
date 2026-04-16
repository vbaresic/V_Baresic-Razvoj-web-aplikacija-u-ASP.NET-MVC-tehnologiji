using League_of_Legends_Tournament_Hosting.Models;
using League_of_Legends_Tournament_Hosting.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace League_of_Legends_Tournament_Hosting.Controllers
{
    public class PlayersController : AppControllerBase
    {
        public IActionResult Index()
        {
            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Players", null));

            var cards = MockRepository.Players
                .OrderBy(player => player.GamerTag)
                .Select(player => new EntityCardViewModel(
                    player.GamerTag,
                    player.Name,
                    $"{player.Role}|{player.AccountInformation.LeagueTier}|{player.PreferredPosition} / {player.SecondaryPosition}",
                    "View Details",
                    Url.Action(nameof(Details), new { id = player.Id }) ?? "#"))
                .ToList();

            ViewData["PageTitle"] = "Players";
            ViewData["EntityCount"] = cards.Count;
            return View(cards);
        }

        public IActionResult Details(int id)
        {
            var player = MockRepository.GetPlayer(id);
            if (player is null)
            {
                return NotFound();
            }

            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Players", Url.Action(nameof(Index))),
                new BreadcrumbItem(player.GamerTag, null));

            ViewData["PlayerTeam"] = MockRepository.GetTeamForPlayer(player.Id);
            return View(player);
        }
    }
}