using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using League_of_Legends_Tournament_Hosting.Data;
using League_of_Legends_Tournament_Hosting.Models;
using League_of_Legends_Tournament_Hosting.DTOs;

namespace League_of_Legends_Tournament_Hosting.Controllers.Api
{
    [ApiController]
    [Route("api/venues")]
    [Authorize]
    public class VenuesApiController : ControllerBase
    {
        private readonly TournamentDbContext _context;

        public VenuesApiController(TournamentDbContext context)
        {
            _context = context;
        }

        // GET: api/venues?search=...&sortBy=name&descending=false&page=1&pageSize=20
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<VenueDto>>> GetVenues(
            [FromQuery] string? search,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool descending = false,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var query = _context.Venues.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(v => v.Name.Contains(search) || v.City.Contains(search));
            }

            query = sortBy?.ToLowerInvariant() switch
            {
                "city" => descending ? query.OrderByDescending(v => v.City) : query.OrderBy(v => v.City),
                "capacity" => descending ? query.OrderByDescending(v => v.Capacity) : query.OrderBy(v => v.Capacity),
                "name" => descending ? query.OrderByDescending(v => v.Name) : query.OrderBy(v => v.Name),
                _ => descending ? query.OrderByDescending(v => v.Id) : query.OrderBy(v => v.Id)
            };

            var venues = await query.ApplyPaging(page, pageSize).ToListAsync();
            return Ok(venues.Select(ToDto));
        }

        // GET: api/venues/5
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<VenueDto>> GetVenue(int id)
        {
            var venue = await _context.Venues.FindAsync(id);
            if (venue == null)
                return NotFound();

            return Ok(ToDto(venue));
        }

        // POST: api/venues
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<VenueDto>> CreateVenue(VenueRequest request)
        {
            var venue = new Venue
            {
                Name = request.Name,
                Address = request.Address,
                City = request.City,
                Capacity = request.Capacity,
                IsAvailable = request.IsAvailable,
                BookingFrom = request.BookingFrom,
                BookingTo = request.BookingTo,
                ContactEmail = request.ContactEmail,
                ContactPhone = request.ContactPhone
            };

            _context.Venues.Add(venue);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVenue), new { id = venue.Id }, ToDto(venue));
        }

        // PUT: api/venues/5
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateVenue(int id, VenueRequest request)
        {
            var venue = await _context.Venues.FindAsync(id);
            if (venue == null)
                return NotFound();

            venue.Name = request.Name;
            venue.Address = request.Address;
            venue.City = request.City;
            venue.Capacity = request.Capacity;
            venue.IsAvailable = request.IsAvailable;
            venue.BookingFrom = request.BookingFrom;
            venue.BookingTo = request.BookingTo;
            venue.ContactEmail = request.ContactEmail;
            venue.ContactPhone = request.ContactPhone;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/venues/5
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteVenue(int id)
        {
            var venue = await _context.Venues.FindAsync(id);
            if (venue == null)
                return NotFound();

            _context.Venues.Remove(venue);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private static VenueDto ToDto(Venue venue)
        {
            return new VenueDto
            {
                Id = venue.Id,
                Name = venue.Name,
                Address = venue.Address,
                City = venue.City,
                Capacity = venue.Capacity,
                IsAvailable = venue.IsAvailable,
                BookingFrom = venue.BookingFrom,
                BookingTo = venue.BookingTo,
                ContactEmail = venue.ContactEmail,
                ContactPhone = venue.ContactPhone
            };
        }
    }
}
