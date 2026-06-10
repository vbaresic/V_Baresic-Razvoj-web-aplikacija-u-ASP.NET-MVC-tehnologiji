using System.ComponentModel.DataAnnotations;

namespace League_of_Legends_Tournament_Hosting.Models
{
    public class Sponsor
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Sponsor name is required")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 150 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Website is required")]
        [Url(ErrorMessage = "Invalid website URL")]
        [StringLength(500)]
        public string Website { get; set; }

        [Required(ErrorMessage = "Contact email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string ContactEmail { get; set; }

        [Required(ErrorMessage = "Contact phone is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string ContactPhone { get; set; }

        [Required(ErrorMessage = "Sponsorship amount is required")]
        [Range(0, 10000000, ErrorMessage = "Sponsorship amount must be non-negative")]
        public decimal SponsorshipAmount { get; set; }

        [Required(ErrorMessage = "Contract start date is required")]
        [DataType(DataType.DateTime)]
        public DateTime ContractStart { get; set; }

        [Required(ErrorMessage = "Contract end date is required")]
        [DataType(DataType.DateTime)]
        public DateTime ContractEnd { get; set; }

        // EF Core required parameterless constructor
        public Sponsor() { }

        public Sponsor(
            int id,
            string name,
            string website,
            string contactEmail,
            string contactPhone,
            decimal sponsorshipAmount,
            DateTime contractStart,
            DateTime contractEnd)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Website = website ?? throw new ArgumentNullException(nameof(website));
            ContactEmail = contactEmail ?? throw new ArgumentNullException(nameof(contactEmail));
            ContactPhone = contactPhone ?? throw new ArgumentNullException(nameof(contactPhone));
            SponsorshipAmount = sponsorshipAmount;
            ContractStart = contractStart;
            ContractEnd = contractEnd;
        }
    }
}