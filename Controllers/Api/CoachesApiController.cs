using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using League_of_Legends_Tournament_Hosting.Data;
using League_of_Legends_Tournament_Hosting.Models;
using League_of_Legends_Tournament_Hosting.DTOs;

namespace League_of_Legends_Tournament_Hosting.Controllers.Api
{
    [ApiController]
    [Route("api/coaches")]
    [Authorize]
    public class CoachesApiController : ControllerBase
    {
        private readonly TournamentDbContext _context;

        public CoachesApiController(TournamentDbContext context)
        {
            _context = context;
        }

        // GET: api/coaches?search=...
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CoachDto>>> GetCoaches([FromQuery] string? search)
        {
            var query = _context.Coaches.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c => c.Name.Contains(search) || c.GamerTag.Contains(search));
            }

            var coaches = await query.ToListAsync();
            return Ok(coaches.Select(ToDto));
        }

        // GET: api/coaches/5
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<CoachDto>> GetCoach(int id)
        {
            var coach = await _context.Coaches.FindAsync(id);
            if (coach == null)
                return NotFound();

            return Ok(ToDto(coach));
        }

        // POST: api/coaches
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<CoachDto>> CreateCoach(CoachRequest request)
        {
            var coach = new Coach
            {
                Name = request.Name,
                GamerTag = request.GamerTag,
                HiredAt = request.HiredAt,
                YearsOfExperience = request.YearsOfExperience
            };

            _context.Coaches.Add(coach);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCoach), new { id = coach.Id }, ToDto(coach));
        }

        // PUT: api/coaches/5
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateCoach(int id, CoachRequest request)
        {
            var coach = await _context.Coaches.FindAsync(id);
            if (coach == null)
                return NotFound();

            coach.Name = request.Name;
            coach.GamerTag = request.GamerTag;
            coach.HiredAt = request.HiredAt;
            coach.YearsOfExperience = request.YearsOfExperience;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/coaches/5
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCoach(int id)
        {
            var coach = await _context.Coaches.FindAsync(id);
            if (coach == null)
                return NotFound();

            _context.Coaches.Remove(coach);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private static CoachDto ToDto(Coach coach)
        {
            return new CoachDto
            {
                Id = coach.Id,
                Name = coach.Name,
                GamerTag = coach.GamerTag,
                HiredAt = coach.HiredAt,
                YearsOfExperience = coach.YearsOfExperience
            };
        }
    }
}
