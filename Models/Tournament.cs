using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace League_of_Legends_Tournament_Hosting.Models
{
    public class Tournament
    {
        public const int MaximumTeamsCount = 12;

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tournament name is required")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 150 characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, MinimumLength = 5, ErrorMessage = "Description must be between 5 and 1000 characters")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Tournament type is required")]
        public TournamentType Type { get; set; }

        [Required(ErrorMessage = "Tournament format is required")]
        public TournamentFormat Format { get; set; }

        public TournamentStatus Status { get; set; }

        [Required(ErrorMessage = "Prize pool is required")]
        [Range(0, 10000000, ErrorMessage = "Prize pool must be between 0 and 10,000,000")]
        public decimal PrizePool { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Registration deadline is required")]
        [DataType(DataType.DateTime)]
        public DateTime RegistrationDeadline { get; set; }

        // Foreign Key
        [Required(ErrorMessage = "Venue is required")]
        public int VenueId { get; set; }

        // Navigation Properties
        [ForeignKey("VenueId")]
        public virtual Venue Venue { get; set; }

        // EF Core collections for many-to-many relationships
        public virtual ICollection<Team> TeamsList { get; set; } = new List<Team>();
        public virtual ICollection<Sponsor> SponsorsList { get; set; } = new List<Sponsor>();

        // Business logic properties - work directly with EF collections
        [NotMapped]
        public IReadOnlyCollection<Team> Teams =>
            (TeamsList as IReadOnlyCollection<Team>) ?? new List<Team>();

        [NotMapped]
        public IReadOnlyCollection<Sponsor> Sponsors =>
            (SponsorsList as IReadOnlyCollection<Sponsor>) ?? new List<Sponsor>();

        // EF Core required parameterless constructor
        public Tournament() { }

        public Tournament(
            int id,
            string name,
            string description,
            TournamentType type,
            TournamentFormat format,
            decimal prizePool,
            DateTime startDate,
            DateTime endDate,
            DateTime registrationDeadline,
            Venue venue)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Type = type;
            Format = format;
            Status = TournamentStatus.Upcoming;
            PrizePool = prizePool;
            StartDate = startDate;
            EndDate = endDate;
            RegistrationDeadline = registrationDeadline;
            Venue = venue ?? throw new ArgumentNullException(nameof(venue));
            VenueId = venue.Id;
        }

        public void RegisterTeam(Team team)
        {
            if (team == null) throw new ArgumentNullException(nameof(team));
            if (Status != TournamentStatus.Upcoming)
                throw new InvalidOperationException("Teams can only be registered while the tournament is upcoming.");
            if ((TeamsList?.Count ?? 0) >= MaximumTeamsCount)
                throw new InvalidOperationException($"Tournament is full. Maximum team count is {MaximumTeamsCount}.");
            if ((TeamsList?.Any(t => t.Id == team.Id)) ?? false)
                throw new InvalidOperationException("Team is already registered in this tournament.");

            TeamsList?.Add(team);
        }

        public bool RemoveTeam(Team team)
        {
            if (team == null) throw new ArgumentNullException(nameof(team));
            if (Status != TournamentStatus.Upcoming)
                throw new InvalidOperationException("Teams can only be removed while the tournament is upcoming.");

            return TeamsList?.Remove(team) ?? false;
        }

        public void AddSponsor(Sponsor sponsor)
        {
            if (sponsor == null) throw new ArgumentNullException(nameof(sponsor));
            if ((SponsorsList?.Any(s => s.Id == sponsor.Id)) ?? false)
                throw new InvalidOperationException("Sponsor is already added to this tournament.");

            SponsorsList?.Add(sponsor);
        }

        public bool RemoveSponsor(Sponsor sponsor)
        {
            if (sponsor == null) throw new ArgumentNullException(nameof(sponsor));
            return SponsorsList?.Remove(sponsor) ?? false;
        }

        public void StartTournament()
        {
            if (Status != TournamentStatus.Upcoming)
                throw new InvalidOperationException("Only upcoming tournaments can be started.");
            if ((TeamsList?.Count ?? 0) < 2)
                throw new InvalidOperationException("At least 2 teams are required to start a tournament.");

            Status = TournamentStatus.Ongoing;
        }

        public void CompleteTournament()
        {
            if (Status != TournamentStatus.Ongoing)
                throw new InvalidOperationException("Only ongoing tournaments can be completed.");

            Status = TournamentStatus.Completed;
        }

        public void CancelTournament()
        {
            if (Status == TournamentStatus.Completed)
                throw new InvalidOperationException("Cannot cancel a tournament that is already completed.");

            Status = TournamentStatus.Cancelled;
        }
    }
}