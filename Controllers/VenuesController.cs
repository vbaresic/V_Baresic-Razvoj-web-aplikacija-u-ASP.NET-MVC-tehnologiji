using League_of_Legends_Tournament_Hosting.Models;
using League_of_Legends_Tournament_Hosting.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace League_of_Legends_Tournament_Hosting.Controllers
{
    public class VenuesController : AppControllerBase
    {
        public IActionResult Index()
        {
            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Venues", null));

            var cards = MockRepository.Venues
                .OrderBy(venue => venue.City)
                .ThenBy(venue => venue.Name)
                .Select(venue => new EntityCardViewModel(
                    venue.Name,
                    venue.City,
                    $"Capacity {venue.Capacity} · Available: {(venue.IsAvailable ? "Yes" : "No")}",
                    "View Details",
                    Url.Action(nameof(Details), new { id = venue.Id }) ?? "#"))
                .ToList();

            ViewData["PageTitle"] = "Venues";
            ViewData["EntityCount"] = cards.Count;
            return View(cards);
        }

        public IActionResult Details(int id)
        {
            var venue = MockRepository.GetVenue(id);
            if (venue is null)
            {
                return NotFound();
            }

            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Venues", Url.Action(nameof(Index))),
                new BreadcrumbItem(venue.Name, null));

            ViewData["VenueTournaments"] = MockRepository.GetTournamentsForVenue(venue.Id).ToList();
            return View(venue);
        }
    }
}