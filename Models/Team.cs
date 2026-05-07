using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace League_of_Legends_Tournament_Hosting.Models
{
    public class Team
    {
        public const int MinimumPlayersCount = 5;
        public const int MaximumPlayersCount = 7;

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        // Foreign Keys
        public int CoachId { get; set; }
        public int ManagerId { get; set; }

        // Navigation Properties
        [ForeignKey("CoachId")]
        public virtual Coach Coach { get; set; }

        [ForeignKey("ManagerId")]
        public virtual Manager Manager { get; set; }

        public DateTime RegisteredAt { get; set; }

        // EF Core collection for many-to-many relationship
        public virtual ICollection<Player> PlayersList { get; set; } = new List<Player>();

        // Roster state tracking
        public bool IsRosterConfirmed { get; set; }

        // Business logic properties - work directly with EF collection
        [NotMapped]
        public IReadOnlyCollection<Player> Players =>
            (PlayersList as IReadOnlyCollection<Player>) ?? new List<Player>();

        [NotMapped]
        public IReadOnlyCollection<Player> ConfirmedPlayers
        {
            get
            {
                if (!IsRosterConfirmed)
                    throw new InvalidOperationException("Roster is not confirmed yet. Confirm roster first to view finalized player set.");

                return (PlayersList as IReadOnlyCollection<Player>) ?? new List<Player>();
            }
        }

        [NotMapped]
        public IEnumerable<Player> StartingPlayers =>
            (PlayersList ?? Enumerable.Empty<Player>())
                .Where(p => p.Role == PlayerRole.Player || p.Role == PlayerRole.TeamCaptain);

        [NotMapped]
        public IEnumerable<Player> SubstitutePlayers =>
            (PlayersList ?? Enumerable.Empty<Player>())
                .Where(p => p.Role == PlayerRole.Substitute);

        // EF Core required parameterless constructor
        public Team() { }

        public Team(int id, string name, Coach coach, Manager manager, IEnumerable<Player> players, DateTime registeredAt)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Coach = coach ?? throw new ArgumentNullException(nameof(coach));
            Manager = manager ?? throw new ArgumentNullException(nameof(manager));
            CoachId = coach.Id;
            ManagerId = manager.Id;
            RegisteredAt = registeredAt;
            IsRosterConfirmed = false;

            if (players == null)
                throw new ArgumentNullException(nameof(players));

            var playerList = players.ToList();
            if (playerList.Count < MinimumPlayersCount || playerList.Count > MaximumPlayersCount)
                throw new ArgumentException($"A team must have between {MinimumPlayersCount} and {MaximumPlayersCount} players (inclusive).", nameof(players));

            PlayersList = playerList;
        }

        public void AddPlayer(Player player)
        {
            EnsureRosterNotConfirmed();
            if (player == null) throw new ArgumentNullException(nameof(player));
            if ((PlayersList?.Count ?? 0) >= MaximumPlayersCount)
                throw new InvalidOperationException($"Cannot add more than {MaximumPlayersCount} players.");

            PlayersList?.Add(player);
        }

        public bool RemovePlayer(Player player)
        {
            EnsureRosterNotConfirmed();
            if (player == null) throw new ArgumentNullException(nameof(player));
            return PlayersList?.Remove(player) ?? false;
        }

        public void ConfirmRoster()
        {
            var playerCount = PlayersList?.Count ?? 0;
            if (playerCount < MinimumPlayersCount || playerCount > MaximumPlayersCount)
                throw new InvalidOperationException($"Roster must have between {MinimumPlayersCount} and {MaximumPlayersCount} players.");

            var playerRoleCount = (PlayersList?.Count(p => p.Role == PlayerRole.Player)) ?? 0;
            var captainRoleCount = (PlayersList?.Count(p => p.Role == PlayerRole.TeamCaptain)) ?? 0;
            var substituteRoleCount = (PlayersList?.Count(p => p.Role == PlayerRole.Substitute)) ?? 0;

            if (playerRoleCount != 4)
                throw new InvalidOperationException("Confirmed roster must contain exactly 4 players with Role == Player.");

            if (captainRoleCount != 1)
                throw new InvalidOperationException("Confirmed roster must contain exactly 1 player with Role == TeamCaptain.");

            if (playerRoleCount + captainRoleCount + substituteRoleCount != playerCount)
                throw new InvalidOperationException("All roster members must have Player, TeamCaptain, or Substitute role.");

            IsRosterConfirmed = true;
        }

        public void UnconfirmRoster()
        {
            EnsureRosterNotConfirmed(false);
            IsRosterConfirmed = false;
        }

        private void EnsureRosterNotConfirmed(bool expected = true)
        {
            if (expected && IsRosterConfirmed)
                throw new InvalidOperationException("Roster is confirmed and cannot be modified until unconfirmed.");

            if (!expected && !IsRosterConfirmed)
                throw new InvalidOperationException("Roster is not confirmed.");
        }

        public bool IsRosterSizeValid() =>
            (PlayersList?.Count ?? 0) >= MinimumPlayersCount && (PlayersList?.Count ?? 0) <= MaximumPlayersCount;
    }
}