using System.ComponentModel.DataAnnotations;

namespace League_of_Legends_Tournament_Hosting.Models
{
    public class Coach
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string GamerTag { get; set; }
        public DateTime HiredAt { get; set; }
        public int YearsOfExperience { get; set; }

        // EF Core required parameterless constructor
        public Coach() { }

        public Coach(
            int id,
            string name,
            string gamerTag,
            DateTime hiredAt,
            int yearsOfExperience)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            GamerTag = gamerTag ?? throw new ArgumentNullException(nameof(gamerTag));
            HiredAt = hiredAt;
            YearsOfExperience = yearsOfExperience;
        }
    }
}