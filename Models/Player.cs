namespace League_of_Legends_Tournament_Hosting.Models
{
    public class Player
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string GamerTag { get; set; }

        public PlayerRole Role { get; set; }

        public Position PreferredPosition { get; set; }

        public Position SecondaryPosition { get; set; }

        public DateTime JoinedAt { get; set; }

        public AccountInformation AccountInformation { get; set; }

        public Player(int id, string name, string gamerTag, PlayerRole role, Position preferredPosition, Position secondaryPosition, AccountInformation accountInformation, DateTime joinedAt)
        {
            Id = id;
            Name = name;
            GamerTag = gamerTag;
            Role = role;
            PreferredPosition = preferredPosition;
            SecondaryPosition = secondaryPosition;
            AccountInformation = accountInformation;
            JoinedAt = joinedAt;
        }

    }
}
