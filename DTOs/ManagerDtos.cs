using System.ComponentModel.DataAnnotations;

namespace League_of_Legends_Tournament_Hosting.DTOs
{
    public class ManagerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime HiredAt { get; set; }
        public int YearsOfExperience { get; set; }
    }

    public class ManagerRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public DateTime HiredAt { get; set; }

        [Required]
        [Range(0, 100)]
        public int YearsOfExperience { get; set; }
    }
}
