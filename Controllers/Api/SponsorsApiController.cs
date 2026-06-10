using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using League_of_Legends_Tournament_Hosting.Data;
using League_of_Legends_Tournament_Hosting.Models;
using League_of_Legends_Tournament_Hosting.DTOs;

namespace League_of_Legends_Tournament_Hosting.Controllers.Api
{
    [ApiController]
    [Route("api/sponsors")]
    [Authorize]
    public class SponsorsApiController : ControllerBase
    {
        private readonly TournamentDbContext _context;

        public SponsorsApiController(TournamentDbContext context)
        {
            _context = context;
        }

        // GET: api/sponsors?search=...
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<SponsorDto>>> GetSponsors([FromQuery] string? search)
        {
            var query = _context.Sponsors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(s => s.Name.Contains(search));
            }

            var sponsors = await query.ToListAsync();
            return Ok(sponsors.Select(ToDto));
        }

        // GET: api/sponsors/5
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<SponsorDto>> GetSponsor(int id)
        {
            var sponsor = await _context.Sponsors.FindAsync(id);
            if (sponsor == null)
                return NotFound();

            return Ok(ToDto(sponsor));
        }

        // POST: api/sponsors
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<SponsorDto>> CreateSponsor(SponsorRequest request)
        {
            var sponsor = new Sponsor
            {
                Name = request.Name,
                Website = request.Website,
                ContactEmail = request.ContactEmail,
                ContactPhone = request.ContactPhone,
                SponsorshipAmount = request.SponsorshipAmount,
                ContractStart = request.ContractStart,
                ContractEnd = request.ContractEnd
            };

            _context.Sponsors.Add(sponsor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSponsor), new { id = sponsor.Id }, ToDto(sponsor));
        }

        // PUT: api/sponsors/5
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateSponsor(int id, SponsorRequest request)
        {
            var sponsor = await _context.Sponsors.FindAsync(id);
            if (sponsor == null)
                return NotFound();

            sponsor.Name = request.Name;
            sponsor.Website = request.Website;
            sponsor.ContactEmail = request.ContactEmail;
            sponsor.ContactPhone = request.ContactPhone;
            sponsor.SponsorshipAmount = request.SponsorshipAmount;
            sponsor.ContractStart = request.ContractStart;
            sponsor.ContractEnd = request.ContractEnd;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/sponsors/5
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSponsor(int id)
        {
            var sponsor = await _context.Sponsors.FindAsync(id);
            if (sponsor == null)
                return NotFound();

            _context.Sponsors.Remove(sponsor);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private static SponsorDto ToDto(Sponsor sponsor)
        {
            return new SponsorDto
            {
                Id = sponsor.Id,
                Name = sponsor.Name,
                Website = sponsor.Website,
                ContactEmail = sponsor.ContactEmail,
                ContactPhone = sponsor.ContactPhone,
                SponsorshipAmount = sponsor.SponsorshipAmount,
                ContractStart = sponsor.ContractStart,
                ContractEnd = sponsor.ContractEnd
            };
        }
    }
}
