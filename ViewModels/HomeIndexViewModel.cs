namespace League_of_Legends_Tournament_Hosting.ViewModels
{
    public sealed class HomeIndexViewModel
    {
        public HomeIndexViewModel(
            IReadOnlyList<HomeStatViewModel> stats,
            IReadOnlyList<TournamentSpotlightViewModel> highlights,
            IReadOnlyList<HomeSpotlightCardViewModel> topCompetitors,
            IReadOnlyList<HomeSpotlightCardViewModel> recruitingTeams,
            IReadOnlyList<HomeSpotlightCardViewModel> playersLookingForTeam)
        {
            Stats = stats;
            Highlights = highlights;
            TopCompetitors = topCompetitors;
            RecruitingTeams = recruitingTeams;
            PlayersLookingForTeam = playersLookingForTeam;
        }

        public IReadOnlyList<HomeStatViewModel> Stats { get; }

        public IReadOnlyList<TournamentSpotlightViewModel> Highlights { get; }

        public IReadOnlyList<HomeSpotlightCardViewModel> TopCompetitors { get; }

        public IReadOnlyList<HomeSpotlightCardViewModel> RecruitingTeams { get; }

        public IReadOnlyList<HomeSpotlightCardViewModel> PlayersLookingForTeam { get; }
    }
}