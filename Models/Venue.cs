using System.ComponentModel.DataAnnotations;

namespace League_of_Legends_Tournament_Hosting.Models
{
    public class Venue
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Venue name is required")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 150 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 200 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "City must be between 2 and 100 characters")]
        public string City { get; set; }

        [Required(ErrorMessage = "Capacity is required")]
        [Range(1, 1000000, ErrorMessage = "Capacity must be at least 1")]
        public int Capacity { get; set; }

        public bool IsAvailable { get; set; }

        [Required(ErrorMessage = "Booking from date is required")]
        [DataType(DataType.DateTime)]
        public DateTime BookingFrom { get; set; }

        [Required(ErrorMessage = "Booking to date is required")]
        [DataType(DataType.DateTime)]
        public DateTime BookingTo { get; set; }

        [Required(ErrorMessage = "Contact email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string ContactEmail { get; set; }

        [Required(ErrorMessage = "Contact phone is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
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