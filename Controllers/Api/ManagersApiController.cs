using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using League_of_Legends_Tournament_Hosting.Data;
using League_of_Legends_Tournament_Hosting.Models;
using League_of_Legends_Tournament_Hosting.DTOs;

namespace League_of_Legends_Tournament_Hosting.Controllers.Api
{
    [ApiController]
    [Route("api/managers")]
    [Authorize]
    public class ManagersApiController : ControllerBase
    {
        private readonly TournamentDbContext _context;

        public ManagersApiController(TournamentDbContext context)
        {
            _context = context;
        }

        // GET: api/managers?search=...
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ManagerDto>>> GetManagers([FromQuery] string? search)
        {
            var query = _context.Managers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(m => m.Name.Contains(search));
            }

            var managers = await query.ToListAsync();
            return Ok(managers.Select(ToDto));
        }

        // GET: api/managers/5
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<ManagerDto>> GetManager(int id)
        {
            var manager = await _context.Managers.FindAsync(id);
            if (manager == null)
                return NotFound();

            return Ok(ToDto(manager));
        }

        // POST: api/managers
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ManagerDto>> CreateManager(ManagerRequest request)
        {
            var manager = new Manager
            {
                Name = request.Name,
                HiredAt = request.HiredAt,
                YearsOfExperience = request.YearsOfExperience
            };

            _context.Managers.Add(manager);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetManager), new { id = manager.Id }, ToDto(manager));
        }

        // PUT: api/managers/5
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateManager(int id, ManagerRequest request)
        {
            var manager = await _context.Managers.FindAsync(id);
            if (manager == null)
                return NotFound();

            manager.Name = request.Name;
            manager.HiredAt = request.HiredAt;
            manager.YearsOfExperience = request.YearsOfExperience;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/managers/5
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteManager(int id)
        {
            var manager = await _context.Managers.FindAsync(id);
            if (manager == null)
                return NotFound();

            _context.Managers.Remove(manager);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private static ManagerDto ToDto(Manager manager)
        {
            return new ManagerDto
            {
                Id = manager.Id,
                Name = manager.Name,
                HiredAt = manager.HiredAt,
                YearsOfExperience = manager.YearsOfExperience
            };
        }
    }
}
