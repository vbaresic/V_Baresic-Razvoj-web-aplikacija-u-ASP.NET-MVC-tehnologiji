namespace League_of_Legends_Tournament_Hosting.Models
{
    public class Sponsor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Website { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public decimal SponsorshipAmount { get; set; }
        public DateTime ContractStart { get; set; }
        public DateTime ContractEnd { get; set; }

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