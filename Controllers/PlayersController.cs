using League_of_Legends_Tournament_Hosting.Data;
using League_of_Legends_Tournament_Hosting.Models;
using League_of_Legends_Tournament_Hosting.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace League_of_Legends_Tournament_Hosting.Controllers
{
    [Route("igrac")]
    [Authorize]
    public class PlayersController : AppControllerBase
    {
        private readonly TournamentDbContext _dbContext;

        public PlayersController(TournamentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Route("")]
        [Route("pregled")]
        [Route("red")]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Players", null));

            var players = await _dbContext.Players
                .OrderBy(player => player.GamerTag)
                .ToListAsync();

            var cards = players
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

        [Route("detalji/{id:int}")]
        [Route("profil/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var player = await _dbContext.Players
                .Include(p => p.Teams)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (player is null)
            {
                return NotFound();
            }

            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Players", Url.Action(nameof(Index))),
                new BreadcrumbItem(player.GamerTag, null));

            // Get the player's teams
            var playerTeams = player.Teams?.FirstOrDefault();
            ViewData["PlayerTeam"] = playerTeams;
            return View(player);
        }

        [HttpGet]
        [Route("kreiraj")]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create()
        {
            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Players", Url.Action(nameof(Index))),
                new BreadcrumbItem("Create", null));
            return View();
        }

        [HttpPost]
        [Route("kreiraj")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create([Bind("Name,GamerTag,Role,PreferredPosition,SecondaryPosition,JoinedAt,AccountInformation")] Player player)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Add(player);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(player);
        }

        [HttpGet]
        [Route("uredi/{id:int}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id)
        {
            var player = await _dbContext.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }
            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Players", Url.Action(nameof(Index))),
                new BreadcrumbItem("Edit", null));
            return View(player);
        }

        [HttpPost]
        [Route("uredi/{id:int}")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,GamerTag,Role,PreferredPosition,SecondaryPosition,JoinedAt,AccountInformation")] Player player)
        {
            if (id != player.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(player);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerExists(player.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(player);
        }

        [Route("obrisi/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var player = await _dbContext.Players
                .FirstOrDefaultAsync(p => p.Id == id);
            
            if (player is null)
            {
                return NotFound();
            }

            SetBreadcrumbs(
                new BreadcrumbItem("Home", Url.Action("Index", "Home")),
                new BreadcrumbItem("Players", Url.Action(nameof(Index))),
                new BreadcrumbItem(player.GamerTag, Url.Action(nameof(Details), new { id = player.Id })),
                new BreadcrumbItem("Delete", null));

            return View(player);
        }

        [Route("obrisi/{id:int}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _dbContext.Players.FindAsync(id);
            if (player is not null)
            {
                _dbContext.Players.Remove(player);
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

            var searchResults = await _dbContext.Players
                .AsNoTracking()
                .Where(p => p.GamerTag.Contains(query) || p.Name.Contains(query))
                .OrderBy(p => p.GamerTag)
                .Select(p => new EntityCardViewModel(
                    p.GamerTag,
                    p.Name,
                    $"{p.Role}|{p.AccountInformation.LeagueTier}|{p.PreferredPosition} / {p.SecondaryPosition}",
                    "View Details",
                    Url.Action(nameof(Details), new { id = p.Id }) ?? "#"))
                .ToListAsync();

            return Json(searchResults);
        }

        private bool PlayerExists(int id)
        {
            return _dbContext.Players.Any(e => e.Id == id);
        }
    }
}