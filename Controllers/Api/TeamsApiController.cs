using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using League_of_Legends_Tournament_Hosting.Data;
using League_of_Legends_Tournament_Hosting.Models;
using League_of_Legends_Tournament_Hosting.DTOs;

namespace League_of_Legends_Tournament_Hosting.Controllers.Api
{
    [ApiController]
    [Route("api/teams")]
    [Authorize]
    public class TeamsApiController : ControllerBase
    {
        private readonly TournamentDbContext _context;

        public TeamsApiController(TournamentDbContext context)
        {
            _context = context;
        }

        // GET: api/teams?search=...&sortBy=name&descending=false&page=1&pageSize=20
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<TeamDto>>> GetTeams(
            [FromQuery] string? search,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool descending = false,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var query = _context.Teams
                .Include(t => t.Coach)
                .Include(t => t.Manager)
                .Include(t => t.PlayersList)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(t => t.Name.Contains(search));
            }

            query = sortBy?.ToLowerInvariant() switch
            {
                "registeredat" => descending ? query.OrderByDescending(t => t.RegisteredAt) : query.OrderBy(t => t.RegisteredAt),
                "name" => descending ? query.OrderByDescending(t => t.Name) : query.OrderBy(t => t.Name),
                _ => descending ? query.OrderByDescending(t => t.Id) : query.OrderBy(t => t.Id)
            };

            var teams = await query.ApplyPaging(page, pageSize).ToListAsync();
            return Ok(teams.Select(ToDto));
        }

        // GET: api/teams/5
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<TeamDto>> GetTeam(int id)
        {
            var team = await _context.Teams
                .Include(t => t.Coach)
                .Include(t => t.Manager)
                .Include(t => t.PlayersList)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (team == null)
                return NotFound();

            return Ok(ToDto(team));
        }

        // POST: api/teams
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<TeamDto>> CreateTeam(TeamRequest request)
        {
            var coachExists = await _context.Coaches.AnyAsync(c => c.Id == request.CoachId);
            if (!coachExists)
                return BadRequest($"Coach with id {request.CoachId} does not exist.");

            var managerExists = await _context.Managers.AnyAsync(m => m.Id == request.ManagerId);
            if (!managerExists)
                return BadRequest($"Manager with id {request.ManagerId} does not exist.");

            var team = new Team
            {
                Name = request.Name,
                CoachId = request.CoachId,
                ManagerId = request.ManagerId,
                RegisteredAt = request.RegisteredAt,
                IsRosterConfirmed = request.IsRosterConfirmed
            };

            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            await _context.Entry(team).Reference(t => t.Coach).LoadAsync();
            await _context.Entry(team).Reference(t => t.Manager).LoadAsync();

            return CreatedAtAction(nameof(GetTeam), new { id = team.Id }, ToDto(team));
        }

        // PUT: api/teams/5
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateTeam(int id, TeamRequest request)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
                return NotFound();

            var coachExists = await _context.Coaches.AnyAsync(c => c.Id == request.CoachId);
            if (!coachExists)
                return BadRequest($"Coach with id {request.CoachId} does not exist.");

            var managerExists = await _context.Managers.AnyAsync(m => m.Id == request.ManagerId);
            if (!managerExists)
                return BadRequest($"Manager with id {request.ManagerId} does not exist.");

            team.Name = request.Name;
            team.CoachId = request.CoachId;
            team.ManagerId = request.ManagerId;
            team.RegisteredAt = request.RegisteredAt;
            team.IsRosterConfirmed = request.IsRosterConfirmed;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/teams/5
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
                return NotFound();

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private static TeamDto ToDto(Team team)
        {
            return new TeamDto
            {
                Id = team.Id,
                Name = team.Name,
                CoachId = team.CoachId,
                CoachName = team.Coach?.Name ?? string.Empty,
                ManagerId = team.ManagerId,
                ManagerName = team.Manager?.Name ?? string.Empty,
                RegisteredAt = team.RegisteredAt,
                IsRosterConfirmed = team.IsRosterConfirmed,
                PlayerCount = team.PlayersList?.Count ?? 0,
                Coach = team.Coach == null ? null : new CoachSummaryDto
                {
                    Id = team.Coach.Id,
                    Name = team.Coach.Name,
                    GamerTag = team.Coach.GamerTag
                },
                Manager = team.Manager == null ? null : new ManagerSummaryDto
                {
                    Id = team.Manager.Id,
                    Name = team.Manager.Name
                },
                Players = (team.PlayersList ?? new List<Player>()).Select(p => new PlayerSummaryDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    GamerTag = p.GamerTag,
                    Role = p.Role.ToString()
                }).ToList()
            };
        }
    }
}
