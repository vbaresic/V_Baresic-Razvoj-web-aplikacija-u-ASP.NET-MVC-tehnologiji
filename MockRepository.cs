using League_of_Legends_Tournament_Hosting.Models;

namespace League_of_Legends_Tournament_Hosting
{
    public static class MockRepository
    {
        private static IReadOnlyList<Coach> _coaches = Array.Empty<Coach>();
        private static IReadOnlyList<Manager> _managers = Array.Empty<Manager>();
        private static IReadOnlyList<Player> _players = Array.Empty<Player>();
        private static IReadOnlyList<Team> _teams = Array.Empty<Team>();
        private static IReadOnlyList<Sponsor> _sponsors = Array.Empty<Sponsor>();
        private static IReadOnlyList<Venue> _venues = Array.Empty<Venue>();
        private static IReadOnlyList<Tournament> _tournaments = Array.Empty<Tournament>();

        private static Dictionary<int, Coach> _coachById = new();
        private static Dictionary<int, Manager> _managerById = new();
        private static Dictionary<int, Player> _playerById = new();
        private static Dictionary<int, Team> _teamById = new();
        private static Dictionary<int, Sponsor> _sponsorById = new();
        private static Dictionary<int, Venue> _venueById = new();
        private static Dictionary<int, Tournament> _tournamentById = new();

        public static IReadOnlyList<Coach> Coaches => _coaches;
        public static IReadOnlyList<Manager> Managers => _managers;
        public static IReadOnlyList<Player> Players => _players;
        public static IReadOnlyList<Team> Teams => _teams;
        public static IReadOnlyList<Sponsor> Sponsors => _sponsors;
        public static IReadOnlyList<Venue> Venues => _venues;
        public static IReadOnlyList<Tournament> Tournaments => _tournaments;

        public static void Initialize(
            IEnumerable<Coach> coaches,
            IEnumerable<Manager> managers,
            IEnumerable<Player> players,
            IEnumerable<Team> teams,
            IEnumerable<Sponsor> sponsors,
            IEnumerable<Venue> venues,
            IEnumerable<Tournament> tournaments)
        {
            _coaches = coaches.ToList();
            _managers = managers.ToList();
            _players = players.ToList();
            _teams = teams.ToList();
            _sponsors = sponsors.ToList();
            _venues = venues.ToList();
            _tournaments = tournaments.ToList();

            _coachById = _coaches.ToDictionary(coach => coach.Id);
            _managerById = _managers.ToDictionary(manager => manager.Id);
            _playerById = _players.ToDictionary(player => player.Id);
            _teamById = _teams.ToDictionary(team => team.Id);
            _sponsorById = _sponsors.ToDictionary(sponsor => sponsor.Id);
            _venueById = _venues.ToDictionary(venue => venue.Id);
            _tournamentById = _tournaments.ToDictionary(tournament => tournament.Id);
        }

        public static Coach? GetCoach(int id) => _coachById.TryGetValue(id, out var coach) ? coach : null;

        public static Manager? GetManager(int id) => _managerById.TryGetValue(id, out var manager) ? manager : null;

        public static Player? GetPlayer(int id) => _playerById.TryGetValue(id, out var player) ? player : null;

        public static Team? GetTeam(int id) => _teamById.TryGetValue(id, out var team) ? team : null;

        public static Sponsor? GetSponsor(int id) => _sponsorById.TryGetValue(id, out var sponsor) ? sponsor : null;

        public static Venue? GetVenue(int id) => _venueById.TryGetValue(id, out var venue) ? venue : null;

        public static Tournament? GetTournament(int id) => _tournamentById.TryGetValue(id, out var tournament) ? tournament : null;

        public static Team? GetTeamForPlayer(int playerId)
        {
            return _teams.FirstOrDefault(team => team.Players.Any(player => player.Id == playerId));
        }

        public static IEnumerable<Tournament> GetTournamentsForTeam(int teamId)
        {
            return _tournaments.Where(tournament => tournament.Teams.Any(team => team.Id == teamId));
        }

        public static IEnumerable<Tournament> GetTournamentsForSponsor(int sponsorId)
        {
            return _tournaments.Where(tournament => tournament.Sponsors.Any(sponsor => sponsor.Id == sponsorId));
        }

        public static IEnumerable<Tournament> GetTournamentsForVenue(int venueId)
        {
            return _tournaments.Where(tournament => tournament.Venue.Id == venueId);
        }
    }
}