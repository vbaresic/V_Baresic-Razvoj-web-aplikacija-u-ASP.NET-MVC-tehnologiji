using League_of_Legends_Tournament_Hosting.Models;
using League_of_Legends_Tournament_Hosting.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace League_of_Legends_Tournament_Hosting.Controllers
{
    public class TournamentsController : AppControllerBase
    {
        public IActionResult Index()
        {
            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Tournaments", null));

            var cards = MockRepository.Tournaments
                .OrderBy(tournament => tournament.StartDate)
                .Select(tournament =>
                {
                    var stageNumber = tournament.Type == TournamentType.Preliminary ? 1 :
                        tournament.Type == TournamentType.Quarterfinal || tournament.Type == TournamentType.Semifinal ? 2 :
                        3;

                    var teamPreviewNames = tournament.Teams
                        .Select(team => team.Name)
                        .Take(2)
                        .ToList();

                    var teamPreviewLinks = tournament.Teams
                        .Take(2)
                        .Select(team => new TournamentTeamPreviewViewModel(
                            team.Name,
                            Url.Action("Details", "Teams", new { id = team.Id }) ?? "#"))
                        .ToList();

                    var additionalTeamCount = Math.Max(0, tournament.Teams.Count - teamPreviewNames.Count);

                    var teamsSummary = tournament.Teams.Count switch
                    {
                        0 => $"0 / {Tournament.MaximumTeamsCount} teams",
                        _ when tournament.Teams.Count >= Tournament.MaximumTeamsCount => $"{Tournament.MaximumTeamsCount} / {Tournament.MaximumTeamsCount} teams (Full)",
                        _ => $"{tournament.Teams.Count} / {Tournament.MaximumTeamsCount} teams"
                    };

                    var registrationLabel = tournament.Status switch
                    {
                        TournamentStatus.Ongoing => "In Progress",
                        TournamentStatus.Completed => "Completed",
                        TournamentStatus.Cancelled => "Cancelled",
                        _ => tournament.RegistrationDeadline >= DateTime.UtcNow ? "Registration" : "Upcoming"
                    };

                    var daysUntilStart = (tournament.StartDate.Date - DateTime.UtcNow.Date).Days;

                    var countdownLabel = daysUntilStart switch
                    {
                        <= 0 => "Starts soon",
                        1 => "Starts in 1 day",
                        _ => $"Starts in {daysUntilStart} days"
                    };

                    var statusStageMessage = tournament.Status switch
                    {
                        TournamentStatus.Ongoing => $"LIVE • {tournament.Type} Stage",
                        TournamentStatus.Completed => $"Completed • {tournament.Type}",
                        TournamentStatus.Cancelled => $"Cancelled • {tournament.Type}",
                        _ when tournament.Type == TournamentType.Final => $"Final • {countdownLabel}",
                        _ => $"Upcoming • {tournament.Type}"
                    };

                    var competitionStateLabel = tournament.Status switch
                    {
                        TournamentStatus.Ongoing => stageNumber switch
                        {
                            1 => "Registration 25%",
                            2 => "Semifinal Match 1 of 2",
                            _ => "Final Match 1 of 1"
                        },
                        TournamentStatus.Completed => "Completed",
                        TournamentStatus.Cancelled => "Cancelled",
                        _ => countdownLabel
                    };

                    return new TournamentSpotlightViewModel(
                        tournament.Name,
                        tournament.Description,
                        tournament.Type,
                        tournament.Status,
                        tournament.Type.ToString(),
                        stageNumber,
                        $"{tournament.StartDate:dd MMM yyyy} - {tournament.EndDate:dd MMM yyyy}",
                        tournament.Venue.Name,
                        teamsSummary,
                        Url.Action(nameof(Details), new { id = tournament.Id }) ?? "#",
                        "View Details",
                        venueLinkUrl: Url.Action("Details", "Venues", new { id = tournament.Venue.Id }) ?? "#",
                        prizePoolLabel: $"Prize pool: {tournament.PrizePool:C}",
                        teamNamesLabel: "Teams",
                        registrationLabel: registrationLabel,
                        isLiveNow: tournament.Status == TournamentStatus.Ongoing,
                        teamCapacityLabel: teamsSummary,
                        detailsAriaLabel: $"Open {tournament.Name} details page",
                        statusStageMessage: statusStageMessage,
                        competitionStateLabel: competitionStateLabel,
                        showProgressBar: tournament.Status == TournamentStatus.Ongoing,
                        competitionAssistiveLabel: $"{tournament.Name} competition state",
                        teamPreviewNames: teamPreviewNames,
                        additionalTeamCount: additionalTeamCount,
                        teamPreviewLinks: teamPreviewLinks);
                })
                .ToList();

            ViewData["PageTitle"] = "Tournaments";
            ViewData["EntityCount"] = cards.Count;
            return View(cards);
        }

        public IActionResult Details(int id)
        {
            var tournament = MockRepository.GetTournament(id);
            if (tournament is null)
            {
                return NotFound();
            }

            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Tournaments", Url.Action(nameof(Index))),
                new BreadcrumbItem(tournament.Name, null));

            return View(tournament);
        }
    }
}