namespace League_of_Legends_Tournament_Hosting.ViewModels
{
    public sealed class EntityCardViewModel
    {
        public EntityCardViewModel(string title, string subtitle, string body, string linkText, string linkUrl)
        {
            Title = title;
            Subtitle = subtitle;
            Body = body;
            LinkText = linkText;
            LinkUrl = linkUrl;
        }

        public string Title { get; }

        public string Subtitle { get; }

        public string Body { get; }

        public string LinkText { get; }

        public string LinkUrl { get; }
    }
}