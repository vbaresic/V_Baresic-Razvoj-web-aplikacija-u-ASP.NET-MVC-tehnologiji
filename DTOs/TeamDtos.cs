using System.ComponentModel.DataAnnotations;

namespace League_of_Legends_Tournament_Hosting.DTOs
{
    public class TeamDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CoachId { get; set; }
        public string CoachName { get; set; } = string.Empty;
        public int ManagerId { get; set; }
        public string ManagerName { get; set; } = string.Empty;
        public DateTime RegisteredAt { get; set; }
        public bool IsRosterConfirmed { get; set; }
        public int PlayerCount { get; set; }
    }

    public class TeamRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int CoachId { get; set; }

        [Required]
        public int ManagerId { get; set; }

        [Required]
        public DateTime RegisteredAt { get; set; }

        public bool IsRosterConfirmed { get; set; }
    }
}
