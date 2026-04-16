namespace League_of_Legends_Tournament_Hosting.ViewModels
{
    public sealed class HomeSpotlightCardViewModel
    {
        public HomeSpotlightCardViewModel(
            string title,
            string subtitle,
            string body,
            string badgeText,
            string badgeVariant,
            string badgeIcon,
            string linkText,
            string linkUrl,
            bool isFeatured = false,
            string? metricLabel = null,
            string? metricValue = null,
            int? progressValue = null,
            string? progressLabel = null,
            string? progressCaption = null)
        {
            Title = title;
            Subtitle = subtitle;
            Body = body;
            BadgeText = badgeText;
            BadgeVariant = badgeVariant;
            BadgeIcon = badgeIcon;
            LinkText = linkText;
            LinkUrl = linkUrl;
            IsFeatured = isFeatured;
            MetricLabel = metricLabel;
            MetricValue = metricValue;
            ProgressValue = progressValue;
            ProgressLabel = progressLabel;
            ProgressCaption = progressCaption;
        }

        public string Title { get; }

        public string Subtitle { get; }

        public string Body { get; }

        public string BadgeText { get; }

        public string BadgeVariant { get; }

        public string BadgeIcon { get; }

        public string LinkText { get; }

        public string LinkUrl { get; }

        public bool IsFeatured { get; }

        public string? MetricLabel { get; }

        public string? MetricValue { get; }

        public int? ProgressValue { get; }

        public string? ProgressLabel { get; }

        public string? ProgressCaption { get; }
    }
}
