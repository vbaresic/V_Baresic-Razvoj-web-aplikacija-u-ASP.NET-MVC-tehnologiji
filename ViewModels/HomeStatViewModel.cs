namespace League_of_Legends_Tournament_Hosting.ViewModels
{
    public sealed class HomeStatViewModel
    {
        public HomeStatViewModel(string value, string label, string description)
        {
            Value = value;
            Label = label;
            Description = description;
        }

        public string Value { get; }

        public string Label { get; }

        public string Description { get; }
    }
}