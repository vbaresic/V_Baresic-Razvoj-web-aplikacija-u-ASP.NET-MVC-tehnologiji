using System.ComponentModel.DataAnnotations;
using League_of_Legends_Tournament_Hosting.Models;

namespace League_of_Legends_Tournament_Hosting.DTOs
{
    public class AccountInformationDto
    {
        public string SummonerName { get; set; } = string.Empty;
        public string RiotTag { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string LeagueTier { get; set; } = string.Empty;
    }

    public class PlayerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string GamerTag { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string PreferredPosition { get; set; } = string.Empty;
        public string SecondaryPosition { get; set; } = string.Empty;
        public DateTime JoinedAt { get; set; }
        public AccountInformationDto AccountInformation { get; set; } = new();
    }

    public class PlayerRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string GamerTag { get; set; } = string.Empty;

        [Required]
        public PlayerRole Role { get; set; }

        [Required]
        public Position PreferredPosition { get; set; }

        public Position SecondaryPosition { get; set; }

        [Required]
        public DateTime JoinedAt { get; set; }

        [Required]
        [StringLength(100)]
        public string SummonerName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string RiotTag { get; set; } = string.Empty;

        [Required]
        public Region Region { get; set; }

        [Required]
        public LeagueTier LeagueTier { get; set; }
    }
}
