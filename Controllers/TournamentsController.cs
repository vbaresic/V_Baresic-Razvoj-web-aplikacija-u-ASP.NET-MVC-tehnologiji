using League_of_Legends_Tournament_Hosting.Data;
using League_of_Legends_Tournament_Hosting.Models;
using League_of_Legends_Tournament_Hosting.Services;
using League_of_Legends_Tournament_Hosting.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace League_of_Legends_Tournament_Hosting.Controllers
{
    [Route("turnir")]
    [Authorize]
    public class TournamentsController : AppControllerBase
    {
        private readonly TournamentDbContext _dbContext;
        private readonly IWebHostEnvironment _environment;

        public TournamentsController(TournamentDbContext dbContext, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _environment = environment;
        }

        [Route("")]
        [Route("pregled")]
        [Route("lista")]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Tournaments", null));

            var tournaments = await _dbContext.Tournaments
                .Include(t => t.Venue)
                .Include(t => t.TeamsList)
                .Include(t => t.SponsorsList)
                .OrderBy(tournament => tournament.StartDate)
                .ToListAsync();

            var cards = tournaments
                .Select(tournament =>
                {
                    var stageNumber = tournament.Type == TournamentType.Preliminary ? 1 :
                        tournament.Type == TournamentType.Quarterfinal || tournament.Type == TournamentType.Semifinal ? 2 :
                        3;

                    var teamPreviewNames = tournament.TeamsList
                        .Select(team => team.Name)
                        .Take(2)
                        .ToList();

                    var teamPreviewLinks = tournament.TeamsList
                        .Take(2)
                        .Select(team => new TournamentTeamPreviewViewModel(
                            team.Name,
                            Url.Action("Details", "Teams", new { id = team.Id }) ?? "#"))
                        .ToList();

                    var additionalTeamCount = Math.Max(0, tournament.TeamsList.Count - teamPreviewNames.Count);

                    var teamsSummary = tournament.TeamsList.Count switch
                    {
                        0 => $"0 / {Tournament.MaximumTeamsCount} teams",
                        _ when tournament.TeamsList.Count >= Tournament.MaximumTeamsCount => $"{Tournament.MaximumTeamsCount} / {Tournament.MaximumTeamsCount} teams (Full)",
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

        [Route("detalji/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var tournament = await _dbContext.Tournaments
                .Include(t => t.Venue)
                .Include(t => t.TeamsList)
                .Include(t => t.SponsorsList)
                .FirstOrDefaultAsync(t => t.Id == id);

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

        [Route("kreiraj")]
        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create()
        {
            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Tournaments", Url.Action(nameof(Index))),
                new BreadcrumbItem("Create New", null));

            ViewBag.TournamentTypes   = Enum.GetValues<TournamentType>();
            ViewBag.TournamentFormats = Enum.GetValues<TournamentFormat>();
            ViewBag.TournamentStatuses = Enum.GetValues<TournamentStatus>();

            return View(new Tournament());
        }

        [Route("kreiraj")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create([Bind("Name,Description,Type,Format,Status,PrizePool,StartDate,EndDate,RegistrationDeadline,VenueId")] Tournament tournament)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Add(tournament);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = tournament.Id });
            }

            ViewBag.TournamentTypes   = Enum.GetValues<TournamentType>();
            ViewBag.TournamentFormats = Enum.GetValues<TournamentFormat>();
            ViewBag.TournamentStatuses = Enum.GetValues<TournamentStatus>();
            return View(tournament);
        }

        [Route("uredi/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id)
        {
            var tournament = await _dbContext.Tournaments
                .Include(t => t.Venue)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tournament is null)
            {
                return NotFound();
            }

            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Tournaments", Url.Action(nameof(Index))),
                new BreadcrumbItem(tournament.Name, Url.Action(nameof(Details), new { id = tournament.Id })),
                new BreadcrumbItem("Edit", null));

            ViewBag.TournamentTypes   = Enum.GetValues<TournamentType>();
            ViewBag.TournamentFormats = Enum.GetValues<TournamentFormat>();
            ViewBag.TournamentStatuses = Enum.GetValues<TournamentStatus>();

            return View(tournament);
        }

        [Route("uredi/{id:int}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Type,Format,Status,PrizePool,StartDate,EndDate,RegistrationDeadline,VenueId")] Tournament tournament)
        {
            if (id != tournament.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(tournament);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TournamentExists(tournament.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Details), new { id = tournament.Id });
            }

            tournament.Venue = await _dbContext.Venues.FindAsync(tournament.VenueId) ?? new Venue();
            ViewBag.TournamentTypes   = Enum.GetValues<TournamentType>();
            ViewBag.TournamentFormats = Enum.GetValues<TournamentFormat>();
            ViewBag.TournamentStatuses = Enum.GetValues<TournamentStatus>();
            return View(tournament);
        }

        [Route("obrisi/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var tournament = await _dbContext.Tournaments
                .Include(t => t.Venue)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tournament is null)
            {
                return NotFound();
            }

            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Tournaments", Url.Action(nameof(Index))),
                new BreadcrumbItem(tournament.Name, Url.Action(nameof(Details), new { id = tournament.Id })),
                new BreadcrumbItem("Delete", null));

            return View(tournament);
        }

        [Route("obrisi/{id:int}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tournament = await _dbContext.Tournaments.FindAsync(id);
            if (tournament is not null)
            {
                _dbContext.Tournaments.Remove(tournament);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [Route("pretraga")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest();
            }

            var searchResults = await _dbContext.Tournaments
                .AsNoTracking()
                .Include(t => t.Venue)
                .Include(t => t.TeamsList)
                .Where(t => t.Name.Contains(query) || t.Description.Contains(query) || t.Venue.Name.Contains(query))
                .OrderBy(t => t.StartDate)
                .Select(t => new EntityCardViewModel(
                    t.Name,
                    t.Venue.Name,
                    $"{t.Status} · {t.StartDate:dd MMM yyyy} — {t.EndDate:dd MMM yyyy} · Prize: {t.PrizePool:C}",
                    "View Details",
                    Url.Action(nameof(Details), new { id = t.Id }) ?? "#"))
                .ToListAsync();

            return Json(searchResults);
        }

        [Route("dokumenti/{id:int}/upload")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UploadDocument(int id, IFormFile file)
        {
            var tournament = await _dbContext.Tournaments.FindAsync(id);
            if (tournament is null)
            {
                return NotFound();
            }

            if (file is null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            if (!TournamentDocumentValidator.IsValid(file, out var validationError))
            {
                return BadRequest(validationError);
            }

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "tournaments", id.ToString());
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var document = new TournamentDocument
            {
                TournamentId = id,
                FileName = file.FileName,
                FilePath = $"/uploads/tournaments/{id}/{uniqueFileName}",
                ContentType = file.ContentType,
                FileSize = file.Length,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.TournamentDocuments.Add(document);
            await _dbContext.SaveChangesAsync();

            return Ok(new { document.Id, document.FileName, document.FilePath, document.FileSize });
        }

        [Route("dokumenti/{id:int}")]
        [HttpGet]
        public async Task<IActionResult> GetDocuments(int id)
        {
            var documents = await _dbContext.TournamentDocuments
                .AsNoTracking()
                .Where(d => d.TournamentId == id)
                .OrderByDescending(d => d.CreatedAt)
                .Select(d => new { d.Id, d.FileName, d.FilePath, d.FileSize, d.CreatedAt })
                .ToListAsync();

            return Json(documents);
        }

        [Route("dokumenti/obrisi/{documentId:int}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteDocument(int documentId)
        {
            var document = await _dbContext.TournamentDocuments.FindAsync(documentId);
            if (document is null)
            {
                return NotFound();
            }

            var fullPath = Path.Combine(_environment.WebRootPath, document.FilePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            _dbContext.TournamentDocuments.Remove(document);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        private bool TournamentExists(int id)
        {
            return _dbContext.Tournaments.Any(e => e.Id == id);
        }
    }
}