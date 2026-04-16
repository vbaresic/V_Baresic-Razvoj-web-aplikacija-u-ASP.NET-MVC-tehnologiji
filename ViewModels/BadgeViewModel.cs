namespace League_of_Legends_Tournament_Hosting.ViewModels
{
    public sealed class BadgeViewModel
    {
        public BadgeViewModel(string text, string variant)
        {
            Text = text;
            Variant = variant;
        }

        public string Text { get; }

        public string Variant { get; }
    }
}