namespace League_of_Legends_Tournament_Hosting.Models
{
    public class Tournament
    {
        public const int MaximumTeamsCount = 12;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TournamentType Type { get; set; }
        public TournamentFormat Format { get; set; }
        public TournamentStatus Status { get; set; }
        public decimal PrizePool { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime RegistrationDeadline { get; set; }
        public Venue Venue { get; set; }

        private readonly List<Team> _teams = new();
        private readonly List<Sponsor> _sponsors = new();

        public IReadOnlyList<Team> Teams => _teams.AsReadOnly();
        public IReadOnlyList<Sponsor> Sponsors => _sponsors.AsReadOnly();

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
        }

        public void RegisterTeam(Team team)
        {
            if (team == null) throw new ArgumentNullException(nameof(team));
            if (Status != TournamentStatus.Upcoming)
                throw new InvalidOperationException("Teams can only be registered while the tournament is upcoming.");
            if (_teams.Count >= MaximumTeamsCount)
                throw new InvalidOperationException($"Tournament is full. Maximum team count is {MaximumTeamsCount}.");
            if (_teams.Contains(team))
                throw new InvalidOperationException("Team is already registered in this tournament.");

            _teams.Add(team);
        }

        public bool RemoveTeam(Team team)
        {
            if (team == null) throw new ArgumentNullException(nameof(team));
            if (Status != TournamentStatus.Upcoming)
                throw new InvalidOperationException("Teams can only be removed while the tournament is upcoming.");

            return _teams.Remove(team);
        }

        public void AddSponsor(Sponsor sponsor)
        {
            if (sponsor == null) throw new ArgumentNullException(nameof(sponsor));
            if (_sponsors.Contains(sponsor))
                throw new InvalidOperationException("Sponsor is already added to this tournament.");

            _sponsors.Add(sponsor);
        }

        public bool RemoveSponsor(Sponsor sponsor)
        {
            if (sponsor == null) throw new ArgumentNullException(nameof(sponsor));
            return _sponsors.Remove(sponsor);
        }

        public void StartTournament()
        {
            if (Status != TournamentStatus.Upcoming)
                throw new InvalidOperationException("Only upcoming tournaments can be started.");
            if (_teams.Count < 2)
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