using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace League_of_Legends_Tournament_Hosting.Models
{
    public class Player
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Player name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Gamer tag is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Gamer tag must be between 3 and 50 characters")]
        public string GamerTag { get; set; }

        [Required(ErrorMessage = "Player role is required")]
        public PlayerRole Role { get; set; }

        [Required(ErrorMessage = "Preferred position is required")]
        public Position PreferredPosition { get; set; }

        public Position SecondaryPosition { get; set; }

        [Required(ErrorMessage = "Join date is required")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format")]
        public DateTime JoinedAt { get; set; }

        public AccountInformation AccountInformation { get; set; }

        // Navigation property for many-to-many relationship with Team
        public virtual ICollection<Team> Teams { get; set; } = new List<Team>();

        // EF Core required parameterless constructor
        public Player() { }

        public Player(int id, string name, string gamerTag, PlayerRole role, Position preferredPosition, Position secondaryPosition, AccountInformation accountInformation, DateTime joinedAt)
        {
            Id = id;
            Name = name;
            GamerTag = gamerTag;
            Role = role;
            PreferredPosition = preferredPosition;
            SecondaryPosition = secondaryPosition;
            AccountInformation = accountInformation;
            JoinedAt = joinedAt;
        }

    }
}