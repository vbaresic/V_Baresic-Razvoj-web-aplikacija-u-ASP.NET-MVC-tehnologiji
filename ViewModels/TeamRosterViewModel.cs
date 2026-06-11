using League_of_Legends_Tournament_Hosting.Models;

namespace League_of_Legends_Tournament_Hosting.ViewModels
{
    public sealed class TeamRosterViewModel
    {
        public TeamRosterViewModel(
            int teamId,
            string teamName,
            bool isRosterConfirmed,
            IReadOnlyCollection<Player> currentPlayers,
            IReadOnlyCollection<Player> availablePlayers)
        {
            TeamId = teamId;
            TeamName = teamName;
            IsRosterConfirmed = isRosterConfirmed;
            CurrentPlayers = currentPlayers;
            AvailablePlayers = availablePlayers;
        }

        public int TeamId { get; }

        public string TeamName { get; }

        public bool IsRosterConfirmed { get; }

        public IReadOnlyCollection<Player> CurrentPlayers { get; }

        public IReadOnlyCollection<Player> AvailablePlayers { get; }

        public int MinimumPlayersCount => Team.MinimumPlayersCount;

        public int MaximumPlayersCount => Team.MaximumPlayersCount;
    }
}
