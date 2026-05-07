using System.ComponentModel.DataAnnotations;

namespace League_of_Legends_Tournament_Hosting.Models
{
    public class Venue
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int Capacity { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime BookingFrom { get; set; }
        public DateTime BookingTo { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }

        // EF Core required parameterless constructor
        public Venue() { }

        public Venue(
            int id,
            string name,
            string address,
            string city,
            int capacity,
            DateTime bookingFrom,
            DateTime bookingTo,
            string contactEmail,
            string contactPhone,
            bool isAvailable = true)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Address = address ?? throw new ArgumentNullException(nameof(address));
            City = city ?? throw new ArgumentNullException(nameof(city));
            Capacity = capacity;
            BookingFrom = bookingFrom;
            BookingTo = bookingTo;
            ContactEmail = contactEmail ?? throw new ArgumentNullException(nameof(contactEmail));
            ContactPhone = contactPhone ?? throw new ArgumentNullException(nameof(contactPhone));
            IsAvailable = isAvailable;
        }
    }
}