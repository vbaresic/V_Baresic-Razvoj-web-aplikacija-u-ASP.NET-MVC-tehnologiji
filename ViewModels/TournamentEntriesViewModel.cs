using League_of_Legends_Tournament_Hosting.Models;

namespace League_of_Legends_Tournament_Hosting.ViewModels
{
    public sealed class TournamentEntriesViewModel
    {
        public TournamentEntriesViewModel(
            int tournamentId,
            string tournamentName,
            TournamentStatus status,
            IReadOnlyCollection<Team> registeredTeams,
            IReadOnlyCollection<Team> availableTeams,
            IReadOnlyCollection<Sponsor> sponsors,
            IReadOnlyCollection<Sponsor> availableSponsors)
        {
            TournamentId = tournamentId;
            TournamentName = tournamentName;
            Status = status;
            RegisteredTeams = registeredTeams;
            AvailableTeams = availableTeams;
            Sponsors = sponsors;
            AvailableSponsors = availableSponsors;
        }

        public int TournamentId { get; }

        public string TournamentName { get; }

        public TournamentStatus Status { get; }

        public IReadOnlyCollection<Team> RegisteredTeams { get; }

        public IReadOnlyCollection<Team> AvailableTeams { get; }

        public IReadOnlyCollection<Sponsor> Sponsors { get; }

        public IReadOnlyCollection<Sponsor> AvailableSponsors { get; }

        public int MaximumTeamsCount => Tournament.MaximumTeamsCount;
    }
}
