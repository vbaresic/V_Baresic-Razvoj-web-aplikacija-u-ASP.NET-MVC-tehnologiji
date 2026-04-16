using League_of_Legends_Tournament_Hosting.Models;
using League_of_Legends_Tournament_Hosting.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace League_of_Legends_Tournament_Hosting.Controllers
{
    public class SponsorsController : AppControllerBase
    {
        public IActionResult Index()
        {
            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Sponsors", null));

            var cards = MockRepository.Sponsors
                .OrderBy(sponsor => sponsor.Name)
                .Select(sponsor => new EntityCardViewModel(
                    sponsor.Name,
                    sponsor.Website,
                    sponsor.ContactPhone,
                    "View Details",
                    Url.Action(nameof(Details), new { id = sponsor.Id }) ?? "#"))
                .ToList();

            ViewData["PageTitle"] = "Sponsors";
            ViewData["EntityCount"] = cards.Count;
            return View(cards);
        }

        public IActionResult Details(int id)
        {
            var sponsor = MockRepository.GetSponsor(id);
            if (sponsor is null)
            {
                return NotFound();
            }

            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Sponsors", Url.Action(nameof(Index))),
                new BreadcrumbItem(sponsor.Name, null));

            ViewData["SponsorTournaments"] = MockRepository.GetTournamentsForSponsor(sponsor.Id).ToList();
            return View(sponsor);
        }
    }
}