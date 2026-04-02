namespace League_of_Legends_Tournament_Hosting.Models
{
    public class Team
    {
        public const int MinimumPlayersCount = 5;
        public const int MaximumPlayersCount = 7;

        public int Id { get; set; }
        public string Name { get; set; }
        public Coach Coach { get; set; }
        public Manager Manager { get; set; }
        public DateTime RegisteredAt { get; set; }

        private readonly List<Player> _players = new();
        private bool _rosterConfirmed;

        public bool IsRosterConfirmed => _rosterConfirmed;

        public IReadOnlyList<Player> Players => _players.AsReadOnly();

        public IReadOnlyList<Player> ConfirmedPlayers
        {
            get
            {
                if (!_rosterConfirmed)
                    throw new InvalidOperationException("Roster is not confirmed yet. Confirm roster first to view finalized player set.");

                return _players.AsReadOnly();
            }
        }

        public IEnumerable<Player> StartingPlayers =>
            _players.Where(p => p.Role == PlayerRole.Player || p.Role == PlayerRole.TeamCaptain);

        public IEnumerable<Player> SubstitutePlayers =>
            _players.Where(p => p.Role == PlayerRole.Substitute);

        public Team(int id, string name, Coach coach, Manager manager, IEnumerable<Player> players, DateTime registeredAt)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Coach = coach ?? throw new ArgumentNullException(nameof(coach));
            Manager = manager ?? throw new ArgumentNullException(nameof(manager));
            RegisteredAt = registeredAt;

            if (players == null)
                throw new ArgumentNullException(nameof(players));

            var playerList = players.ToList();
            if (playerList.Count < MinimumPlayersCount || playerList.Count > MaximumPlayersCount)
                throw new ArgumentException($"A team must have between {MinimumPlayersCount} and {MaximumPlayersCount} players (inclusive).", nameof(players));

            _players.AddRange(playerList);
        }

        public void AddPlayer(Player player)
        {
            EnsureRosterNotConfirmed();
            if (player == null) throw new ArgumentNullException(nameof(player));
            if (_players.Count >= MaximumPlayersCount)
                throw new InvalidOperationException($"Cannot add more than {MaximumPlayersCount} players.");

            _players.Add(player);
        }

        public bool RemovePlayer(Player player)
        {
            EnsureRosterNotConfirmed();
            if (player == null) throw new ArgumentNullException(nameof(player));
            return _players.Remove(player);
        }

        public void ConfirmRoster()
        {
            if (_players.Count < MinimumPlayersCount || _players.Count > MaximumPlayersCount)
                throw new InvalidOperationException($"Roster must have between {MinimumPlayersCount} and {MaximumPlayersCount} players.");

            var playerRoleCount = _players.Count(p => p.Role == PlayerRole.Player);
            var captainRoleCount = _players.Count(p => p.Role == PlayerRole.TeamCaptain);
            var substituteRoleCount = _players.Count(p => p.Role == PlayerRole.Substitute);

            if (playerRoleCount != 4)
                throw new InvalidOperationException("Confirmed roster must contain exactly 4 players with Role == Player.");

            if (captainRoleCount != 1)
                throw new InvalidOperationException("Confirmed roster must contain exactly 1 player with Role == TeamCaptain.");

            if (playerRoleCount + captainRoleCount + substituteRoleCount != _players.Count)
                throw new InvalidOperationException("All roster members must have Player, TeamCaptain, or Substitute role.");

            _rosterConfirmed = true;
        }

        public void UnconfirmRoster()
        {
            EnsureRosterNotConfirmed(false);
            _rosterConfirmed = false;
        }

        private void EnsureRosterNotConfirmed(bool expected = true)
        {
            if (expected && _rosterConfirmed)
                throw new InvalidOperationException("Roster is confirmed and cannot be modified until unconfirmed.");

            if (!expected && !_rosterConfirmed)
                throw new InvalidOperationException("Roster is not confirmed.");
        }

        public bool IsRosterSizeValid() =>
            _players.Count >= MinimumPlayersCount && _players.Count <= MaximumPlayersCount;
    }
}