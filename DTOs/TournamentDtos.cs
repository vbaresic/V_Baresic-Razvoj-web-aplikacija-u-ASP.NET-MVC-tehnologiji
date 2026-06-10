using System.ComponentModel.DataAnnotations;
using League_of_Legends_Tournament_Hosting.Models;

namespace League_of_Legends_Tournament_Hosting.DTOs
{
    public class TournamentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal PrizePool { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime RegistrationDeadline { get; set; }
        public int VenueId { get; set; }
        public string VenueName { get; set; } = string.Empty;
        public int TeamCount { get; set; }
    }

    public class TournamentRequest
    {
        [Required]
        [StringLength(150, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(1000, MinimumLength = 5)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public TournamentType Type { get; set; }

        [Required]
        public TournamentFormat Format { get; set; }

        public TournamentStatus Status { get; set; }

        [Required]
        [Range(0, 10000000)]
        public decimal PrizePool { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public DateTime RegistrationDeadline { get; set; }

        [Required]
        public int VenueId { get; set; }
    }
}
