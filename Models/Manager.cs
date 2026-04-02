namespace League_of_Legends_Tournament_Hosting.Models
{
    public class Manager
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime HiredAt { get; set; }
        public int YearsOfExperience { get; set; }

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