namespace League_of_Legends_Tournament_Hosting.ViewModels
{
    public sealed class BreadcrumbItem
    {
        public BreadcrumbItem(string label, string? url)
        {
            Label = label;
            Url = url;
        }

        public string Label { get; }

        public string? Url { get; }

        public bool IsCurrent => string.IsNullOrWhiteSpace(Url);
    }
}