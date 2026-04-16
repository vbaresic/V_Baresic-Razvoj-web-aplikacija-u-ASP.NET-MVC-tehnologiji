using League_of_Legends_Tournament_Hosting.Models;
using League_of_Legends_Tournament_Hosting.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace League_of_Legends_Tournament_Hosting.Controllers
{
    public class HomeController : AppControllerBase
    {
        public IActionResult Index()
        {
            SetBreadcrumbs(new BreadcrumbItem("Home", null));

            var stats = new List<HomeStatViewModel>
            {
                new(MockRepository.Teams.Count.ToString(), "Teams", "Confirmed rosters ready for bracket play"),
                new(MockRepository.Players.Count.ToString(), "Players", "All rostered players across the league"),
                new(MockRepository.Tournaments.Count.ToString(), "Tournaments", "Prelim, semifinal, and final stages"),
                new($"{MockRepository.Tournaments.Sum(tournament => tournament.PrizePool):C}", "Prize Pool", "Total prize money across all events")
            };

            var highlights = MockRepository.Tournaments
                .OrderBy(tournament => tournament.StartDate)
                .Select((tournament, index) => new TournamentSpotlightViewModel(
                    tournament.Name,
                    tournament.Description,
                    tournament.Type,
                    tournament.Status,
                    tournament.Type.ToString(),
                    index + 1,
                    $"{tournament.StartDate:dd MMM} - {tournament.EndDate:dd MMM}",
                    tournament.Venue.Name,
                    $"{tournament.Teams.Count} teams registered",
                    Url.Action("Details", "Tournaments", new { id = tournament.Id }) ?? "#",
                    "View Details"))
                .Take(3)
                .ToList();

            var topCompetitors = MockRepository.Teams
                .OrderByDescending(team => team.ConfirmedPlayers.Count)
                .ThenByDescending(team => team.RegisteredAt)
                .ThenBy(team => team.Name)
                .Select((team, index) => new HomeSpotlightCardViewModel(
                    team.Name,
                    $"Rank #{index + 1}",
                    $"Led by {team.Coach.Name} with captain {team.ConfirmedPlayers.FirstOrDefault(player => player.Role == PlayerRole.TeamCaptain)?.GamerTag ?? "TBD"}.",
                    index == 0 ? "Top contender" : "Contender",
                    "purple",
                    "crown",
                    index == 0 ? "Challenge Team" : "View Team",
                    Url.Action("Details", "Teams", new { id = team.Id }) ?? "#",
                    isFeatured: index == 0,
                    metricLabel: "Confirmed roster",
                    metricValue: $"{team.ConfirmedPlayers.Count}/{Team.MaximumPlayersCount}",
                    progressValue: team.ConfirmedPlayers.Count * 100 / Team.MaximumPlayersCount,
                    progressLabel: "Roster strength",
                    progressCaption: index == 0 ? "Featured squad in the bracket" : "Locked and tournament-ready"))
                .Take(3)
                .ToList();

            var recruitingTeams = MockRepository.Teams
                .Where(team => team.Players.Count < Team.MaximumPlayersCount)
                .OrderBy(team => team.Players.Count)
                .ThenBy(team => team.Name)
                .Select((team, index) => new HomeSpotlightCardViewModel(
                    team.Name,
                    $"{Team.MaximumPlayersCount - team.Players.Count} slots open",
                    "Find yourself a team to win with and lock your place in upcoming qualifiers.",
                    "recruiting",
                    "green",
                    "plus",
                    index == 0 ? "Join This Team" : "See Roster",
                    Url.Action("Details", "Teams", new { id = team.Id }) ?? "#",
                    isFeatured: index == 0,
                    metricLabel: "Urgency",
                    metricValue: $"{Team.MaximumPlayersCount - team.Players.Count} slots left",
                    progressValue: team.Players.Count * 100 / Team.MaximumPlayersCount,
                    progressLabel: $"{team.Players.Count}/{Team.MaximumPlayersCount} roster filled",
                    progressCaption: team.Players.Count == Team.MaximumPlayersCount - 1 ? "One player away from full roster" : "Open slots still available"))
                .Take(3)
                .ToList();

            var playersLookingForTeam = MockRepository.Players
                .OrderByDescending(player => player.AccountInformation.LeagueTier)
                .ThenBy(player => player.GamerTag)
                .Select(player => new HomeSpotlightCardViewModel(
                    player.GamerTag,
                    $"{player.PreferredPosition} · {player.AccountInformation.LeagueTier}",
                    "Not happy with current recruiting teams? Build one from the ground up.",
                    "free agent",
                    "orange",
                    "user",
                    "Browse Players",
                    Url.Action("Details", "Players", new { id = player.Id }) ?? "#",
                    metricLabel: "Open to offers",
                    metricValue: player.AccountInformation.LeagueTier.ToString(),
                    progressCaption: "Looking for the right squad"))
                .Take(3)
                .ToList();

            var model = new HomeIndexViewModel(stats, highlights, topCompetitors, recruitingTeams, playersLookingForTeam);
            ViewData["PageTitle"] = "Tournament Control Room";
            return View(model);
        }

        public IActionResult Privacy()
        {
            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index")),
                new BreadcrumbItem("Privacy", null));

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
