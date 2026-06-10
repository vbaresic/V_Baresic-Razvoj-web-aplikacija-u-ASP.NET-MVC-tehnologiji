using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using League_of_Legends_Tournament_Hosting.Data;
using League_of_Legends_Tournament_Hosting.Models;
using League_of_Legends_Tournament_Hosting.DTOs;
using League_of_Legends_Tournament_Hosting.Services;

namespace League_of_Legends_Tournament_Hosting.Controllers.Api
{
    [ApiController]
    [Route("api/tournaments")]
    [Authorize]
    public class TournamentsApiController : ControllerBase
    {
        private readonly TournamentDbContext _context;

        public TournamentsApiController(TournamentDbContext context)
        {
            _context = context;
        }

        // GET: api/tournaments?search=...
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournaments([FromQuery] string? search)
        {
            var query = _context.Tournaments
                .Include(t => t.Venue)
                .Include(t => t.TeamsList)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(t => t.Name.Contains(search));
            }

            var tournaments = await query.ToListAsync();
            return Ok(tournaments.Select(ToDto));
        }

        // GET: api/tournaments/5
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<TournamentDto>> GetTournament(int id)
        {
            var tournament = await _context.Tournaments
                .Include(t => t.Venue)
                .Include(t => t.TeamsList)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tournament == null)
                return NotFound();

            return Ok(ToDto(tournament));
        }

        // POST: api/tournaments
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<TournamentDto>> CreateTournament(TournamentRequest request)
        {
            var venueExists = await _context.Venues.AnyAsync(v => v.Id == request.VenueId);
            if (!venueExists)
                return BadRequest($"Venue with id {request.VenueId} does not exist.");

            var tournament = new Tournament
            {
                Name = request.Name,
                Description = request.Description,
                Type = request.Type,
                Format = request.Format,
                Status = request.Status,
                PrizePool = request.PrizePool,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                RegistrationDeadline = request.RegistrationDeadline,
                VenueId = request.VenueId
            };

            _context.Tournaments.Add(tournament);
            await _context.SaveChangesAsync();

            await _context.Entry(tournament).Reference(t => t.Venue).LoadAsync();

            return CreatedAtAction(nameof(GetTournament), new { id = tournament.Id }, ToDto(tournament));
        }

        // PUT: api/tournaments/5
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateTournament(int id, TournamentRequest request)
        {
            var tournament = await _context.Tournaments.FindAsync(id);
            if (tournament == null)
                return NotFound();

            var venueExists = await _context.Venues.AnyAsync(v => v.Id == request.VenueId);
            if (!venueExists)
                return BadRequest($"Venue with id {request.VenueId} does not exist.");

            tournament.Name = request.Name;
            tournament.Description = request.Description;
            tournament.Type = request.Type;
            tournament.Format = request.Format;
            tournament.Status = request.Status;
            tournament.PrizePool = request.PrizePool;
            tournament.StartDate = request.StartDate;
            tournament.EndDate = request.EndDate;
            tournament.RegistrationDeadline = request.RegistrationDeadline;
            tournament.VenueId = request.VenueId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/tournaments/5
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTournament(int id)
        {
            var tournament = await _context.Tournaments.FindAsync(id);
            if (tournament == null)
                return NotFound();

            _context.Tournaments.Remove(tournament);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/tournaments/{id:int}/documents
        [HttpPost("{tournamentId:int}/documents")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UploadDocument(int tournamentId, IFormFile file)
        {
            var tournament = await _context.Tournaments.FindAsync(tournamentId);
            if (tournament == null)
                return NotFound($"Tournament with id {tournamentId} not found.");

            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            if (!TournamentDocumentValidator.IsValid(file, out var validationError))
                return BadRequest(validationError);

            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "tournaments", tournamentId.ToString());
            Directory.CreateDirectory(uploadsPath);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var document = new TournamentDocument
            {
                TournamentId = tournamentId,
                FileName = file.FileName,
                FilePath = $"/uploads/tournaments/{tournamentId}/{fileName}",
                ContentType = file.ContentType,
                FileSize = file.Length,
                CreatedAt = DateTime.UtcNow
            };

            _context.TournamentDocuments.Add(document);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDocument), new { id = document.Id }, new
            {
                document.Id,
                document.FileName,
                document.FilePath,
                document.FileSize,
                document.CreatedAt
            });
        }

        // GET: api/tournaments/{tournamentId:int}/documents
        [HttpGet("{tournamentId:int}/documents")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<object>>> GetDocuments(int tournamentId)
        {
            var tournament = await _context.Tournaments.FindAsync(tournamentId);
            if (tournament == null)
                return NotFound($"Tournament with id {tournamentId} not found.");

            var documents = await _context.TournamentDocuments
                .Where(d => d.TournamentId == tournamentId)
                .OrderByDescending(d => d.CreatedAt)
                .Select(d => new
                {
                    d.Id,
                    d.FileName,
                    d.FilePath,
                    d.FileSize,
                    d.CreatedAt
                })
                .ToListAsync();

            return Ok(documents);
        }

        // GET: api/tournaments/documents/{id}
        [HttpGet("documents/{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDocument(int id)
        {
            var document = await _context.TournamentDocuments.FindAsync(id);
            if (document == null)
                return NotFound($"Document with id {id} not found.");

            return Ok(new
            {
                document.Id,
                document.FileName,
                document.FilePath,
                document.FileSize,
                document.CreatedAt
            });
        }

        // DELETE: api/tournaments/documents/{id}
        [HttpDelete("documents/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            var document = await _context.TournamentDocuments.FindAsync(id);
            if (document == null)
                return NotFound($"Document with id {id} not found.");

            // Delete file from disk
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", document.FilePath.TrimStart('/'));
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.TournamentDocuments.Remove(document);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private static TournamentDto ToDto(Tournament tournament)
        {
            return new TournamentDto
            {
                Id = tournament.Id,
                Name = tournament.Name,
                Description = tournament.Description,
                Type = tournament.Type.ToString(),
                Format = tournament.Format.ToString(),
                Status = tournament.Status.ToString(),
                PrizePool = tournament.PrizePool,
                StartDate = tournament.StartDate,
                EndDate = tournament.EndDate,
                RegistrationDeadline = tournament.RegistrationDeadline,
                VenueId = tournament.VenueId,
                VenueName = tournament.Venue?.Name ?? string.Empty,
                TeamCount = tournament.TeamsList?.Count ?? 0
            };
        }
    }
}
