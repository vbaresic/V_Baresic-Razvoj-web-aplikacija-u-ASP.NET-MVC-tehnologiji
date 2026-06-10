using System.ComponentModel.DataAnnotations;

namespace League_of_Legends_Tournament_Hosting.DTOs
{
    public class SponsorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public decimal SponsorshipAmount { get; set; }
        public DateTime ContractStart { get; set; }
        public DateTime ContractEnd { get; set; }
    }

    public class SponsorRequest
    {
        [Required]
        [StringLength(150, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Url]
        [StringLength(500)]
        public string Website { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string ContactEmail { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string ContactPhone { get; set; } = string.Empty;

        [Required]
        [Range(0, 10000000)]
        public decimal SponsorshipAmount { get; set; }

        [Required]
        public DateTime ContractStart { get; set; }

        [Required]
        public DateTime ContractEnd { get; set; }
    }
}
