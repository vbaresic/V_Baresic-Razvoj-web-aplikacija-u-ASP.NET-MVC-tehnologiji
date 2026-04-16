using League_of_Legends_Tournament_Hosting.Models;
using System.Collections.Generic;

namespace League_of_Legends_Tournament_Hosting.ViewModels
{
    public sealed class TournamentTeamPreviewViewModel
    {
        public TournamentTeamPreviewViewModel(string name, string linkUrl)
        {
            Name = name;
            LinkUrl = linkUrl;
        }

        public string Name { get; }

        public string LinkUrl { get; }
    }

    public sealed class TournamentSpotlightViewModel
    {
        public TournamentSpotlightViewModel(
            string name,
            string description,
            TournamentType type,
            TournamentStatus status,
            string stageLabel,
            int stageNumber,
            string dateLabel,
            string venueLabel,
            string teamLabel,
            string linkUrl,
            string linkText,
            string? venueLinkUrl = null,
            string? prizePoolLabel = null,
            string? teamNamesLabel = null,
            string? registrationLabel = null,
            bool isLiveNow = false,
            string? teamCapacityLabel = null,
            string? detailsAriaLabel = null,
            string? statusStageMessage = null,
            string? competitionStateLabel = null,
            bool showProgressBar = true,
            string? competitionAssistiveLabel = null,
            IReadOnlyList<string>? teamPreviewNames = null,
            int additionalTeamCount = 0,
            IReadOnlyList<TournamentTeamPreviewViewModel>? teamPreviewLinks = null)
        {
            Name = name;
            Description = description;
            Type = type;
            Status = status;
            StageLabel = stageLabel;
            StageNumber = stageNumber;
            DateLabel = dateLabel;
            VenueLabel = venueLabel;
            VenueLinkUrl = venueLinkUrl ?? string.Empty;
            TeamLabel = teamLabel;
            LinkUrl = linkUrl;
            LinkText = linkText;
            PrizePoolLabel = prizePoolLabel ?? string.Empty;
            TeamNamesLabel = teamNamesLabel ?? string.Empty;
            RegistrationLabel = registrationLabel ?? string.Empty;
            IsLiveNow = isLiveNow;
            TeamCapacityLabel = teamCapacityLabel ?? string.Empty;
            DetailsAriaLabel = detailsAriaLabel ?? $"View details for {name}";
            StatusStageMessage = statusStageMessage ?? string.Empty;
            CompetitionStateLabel = competitionStateLabel ?? string.Empty;
            ShowProgressBar = showProgressBar;
            CompetitionAssistiveLabel = competitionAssistiveLabel ?? "Tournament phase";
            TeamPreviewNames = teamPreviewNames ?? new List<string>();
            AdditionalTeamCount = additionalTeamCount;
            TeamPreviewLinks = teamPreviewLinks ?? new List<TournamentTeamPreviewViewModel>();
        }

        public string Name { get; }

        public string Description { get; }

        public TournamentType Type { get; }

        public TournamentStatus Status { get; }

        public string StageLabel { get; }

        public int StageNumber { get; }

        public string DateLabel { get; }

        public string VenueLabel { get; }

        public string VenueLinkUrl { get; }

        public string TeamLabel { get; }

        public string LinkUrl { get; }

        public string LinkText { get; }

        public string PrizePoolLabel { get; }

        public string TeamNamesLabel { get; }

        public string RegistrationLabel { get; }

        public bool IsLiveNow { get; }

        public string TeamCapacityLabel { get; }

        public string DetailsAriaLabel { get; }

        public string StatusStageMessage { get; }

        public string CompetitionStateLabel { get; }

        public bool ShowProgressBar { get; }

        public string CompetitionAssistiveLabel { get; }

        public IReadOnlyList<string> TeamPreviewNames { get; }

        public int AdditionalTeamCount { get; }

        public IReadOnlyList<TournamentTeamPreviewViewModel> TeamPreviewLinks { get; }
    }
}
