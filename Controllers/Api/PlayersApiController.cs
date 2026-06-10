using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using League_of_Legends_Tournament_Hosting.Data;
using League_of_Legends_Tournament_Hosting.Models;
using League_of_Legends_Tournament_Hosting.DTOs;

namespace League_of_Legends_Tournament_Hosting.Controllers.Api
{
    [ApiController]
    [Route("api/players")]
    [Authorize]
    public class PlayersApiController : ControllerBase
    {
        private readonly TournamentDbContext _context;

        public PlayersApiController(TournamentDbContext context)
        {
            _context = context;
        }

        // GET: api/players?search=...
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PlayerDto>>> GetPlayers([FromQuery] string? search)
        {
            var query = _context.Players.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p => p.Name.Contains(search) || p.GamerTag.Contains(search));
            }

            var players = await query.ToListAsync();
            return Ok(players.Select(ToDto));
        }

        // GET: api/players/5
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<PlayerDto>> GetPlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
                return NotFound();

            return Ok(ToDto(player));
        }

        // POST: api/players
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<PlayerDto>> CreatePlayer(PlayerRequest request)
        {
            var player = new Player
            {
                Name = request.Name,
                GamerTag = request.GamerTag,
                Role = request.Role,
                PreferredPosition = request.PreferredPosition,
                SecondaryPosition = request.SecondaryPosition,
                JoinedAt = request.JoinedAt,
                AccountInformation = new AccountInformation
                {
                    SummonerName = request.SummonerName,
                    RiotTag = request.RiotTag,
                    Region = request.Region,
                    LeagueTier = request.LeagueTier
                }
            };

            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlayer), new { id = player.Id }, ToDto(player));
        }

        // PUT: api/players/5
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdatePlayer(int id, PlayerRequest request)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
                return NotFound();

            player.Name = request.Name;
            player.GamerTag = request.GamerTag;
            player.Role = request.Role;
            player.PreferredPosition = request.PreferredPosition;
            player.SecondaryPosition = request.SecondaryPosition;
            player.JoinedAt = request.JoinedAt;
            player.AccountInformation = new AccountInformation
            {
                SummonerName = request.SummonerName,
                RiotTag = request.RiotTag,
                Region = request.Region,
                LeagueTier = request.LeagueTier
            };

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/players/5
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
                return NotFound();

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private static PlayerDto ToDto(Player player)
        {
            return new PlayerDto
            {
                Id = player.Id,
                Name = player.Name,
                GamerTag = player.GamerTag,
                Role = player.Role.ToString(),
                PreferredPosition = player.PreferredPosition.ToString(),
                SecondaryPosition = player.SecondaryPosition.ToString(),
                JoinedAt = player.JoinedAt,
                AccountInformation = new AccountInformationDto
                {
                    SummonerName = player.AccountInformation?.SummonerName ?? string.Empty,
                    RiotTag = player.AccountInformation?.RiotTag ?? string.Empty,
                    Region = player.AccountInformation?.Region.ToString() ?? string.Empty,
                    LeagueTier = player.AccountInformation?.LeagueTier.ToString() ?? string.Empty
                }
            };
        }
    }
}
