using System.ComponentModel.DataAnnotations;

namespace League_of_Legends_Tournament_Hosting.Models
{
    public class Manager
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Manager name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Hire date is required")]
        [DataType(DataType.DateTime)]
        public DateTime HiredAt { get; set; }

        [Required(ErrorMessage = "Years of experience is required")]
        [Range(0, 100, ErrorMessage = "Years of experience must be between 0 and 100")]
        public int YearsOfExperience { get; set; }

        // EF Core required parameterless constructor
        public Manager() { }

        public Manager(
            int id,
            string name,
            DateTime hiredAt,
            int yearsOfExperience)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            HiredAt = hiredAt;
            YearsOfExperience = yearsOfExperience;
        }
    }
}