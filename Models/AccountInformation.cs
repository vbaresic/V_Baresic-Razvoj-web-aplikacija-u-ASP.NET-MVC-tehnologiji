namespace League_of_Legends_Tournament_Hosting.Models
{
    public class AccountInformation
    {
        public string SummonerName { get; set; }
        public string RiotTag { get; set; }

        public Region Region { get; set; }

        public LeagueTier LeagueTier { get; set; }

        public AccountInformation(string summonerName, string riotTag, Region region, LeagueTier leagueTier)
        {
            RiotTag = riotTag;
            SummonerName = summonerName;
            Region = region;
            LeagueTier = leagueTier;
        }
    }
}
