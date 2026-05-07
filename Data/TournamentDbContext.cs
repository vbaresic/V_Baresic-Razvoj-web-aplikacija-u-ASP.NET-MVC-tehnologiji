using League_of_Legends_Tournament_Hosting.Models;
using Microsoft.EntityFrameworkCore;

namespace League_of_Legends_Tournament_Hosting.Data
{
    public class TournamentDbContext : DbContext
    {
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Sponsor> Sponsors { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }

        public TournamentDbContext(DbContextOptions<TournamentDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Team relationships
            modelBuilder.Entity<Team>()
                .HasOne(t => t.Coach)
                .WithMany()
                .HasForeignKey(t => t.CoachId)
                .IsRequired();

            modelBuilder.Entity<Team>()
                .HasOne(t => t.Manager)
                .WithMany()
                .HasForeignKey(t => t.ManagerId)
                .IsRequired();

            // Configure Team-Player many-to-many relationship with explicit navigation
            modelBuilder.Entity<Team>()
                .HasMany(t => t.PlayersList)
                .WithMany(p => p.Teams)
                .UsingEntity("TeamPlayers");

            // Configure Tournament relationships
            modelBuilder.Entity<Tournament>()
                .HasOne(t => t.Venue)
                .WithMany()
                .HasForeignKey(t => t.VenueId)
                .IsRequired();

            modelBuilder.Entity<Tournament>()
                .HasMany(t => t.TeamsList)
                .WithMany()
                .UsingEntity("TournamentTeams");

            modelBuilder.Entity<Tournament>()
                .HasMany(t => t.SponsorsList)
                .WithMany()
                .UsingEntity("TournamentSponsors");

            // Configure Player owned types
            modelBuilder.Entity<Player>()
                .OwnsOne(p => p.AccountInformation);

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Coaches
            modelBuilder.Entity<Coach>().HasData(
                new Coach { Id = 1, Name = "Ivan Horvat", GamerTag = "IvanCoach", HiredAt = new DateTime(2022, 3, 15), YearsOfExperience = 5 },
                new Coach { Id = 2, Name = "Marko Perić", GamerTag = "MarkoPeric", HiredAt = new DateTime(2021, 6, 1), YearsOfExperience = 7 },
                new Coach { Id = 3, Name = "Luka Novak", GamerTag = "LukaN", HiredAt = new DateTime(2023, 1, 10), YearsOfExperience = 3 },
                new Coach { Id = 4, Name = "Tomislav Blažević", GamerTag = "TomiBlazer", HiredAt = new DateTime(2020, 9, 5), YearsOfExperience = 9 },
                new Coach { Id = 5, Name = "Ante Jurić", GamerTag = "AnteJ", HiredAt = new DateTime(2022, 11, 20), YearsOfExperience = 4 },
                new Coach { Id = 6, Name = "Nikola Šarić", GamerTag = "NikoSaric", HiredAt = new DateTime(2019, 4, 12), YearsOfExperience = 11 }
            );

            // Seed Managers
            modelBuilder.Entity<Manager>().HasData(
                new Manager { Id = 1, Name = "Petra Kovač", HiredAt = new DateTime(2022, 3, 15), YearsOfExperience = 4 },
                new Manager { Id = 2, Name = "Ana Babić", HiredAt = new DateTime(2021, 6, 1), YearsOfExperience = 6 },
                new Manager { Id = 3, Name = "Maja Tomić", HiredAt = new DateTime(2023, 1, 10), YearsOfExperience = 2 },
                new Manager { Id = 4, Name = "Sara Marić", HiredAt = new DateTime(2020, 9, 5), YearsOfExperience = 8 },
                new Manager { Id = 5, Name = "Iva Paulić", HiredAt = new DateTime(2022, 11, 20), YearsOfExperience = 3 },
                new Manager { Id = 6, Name = "Dora Knežević", HiredAt = new DateTime(2019, 4, 12), YearsOfExperience = 10 }
            );

            // Seed Venues
            modelBuilder.Entity<Venue>().HasData(
                new Venue { Id = 1, Name = "Zagreb Esports Arena", Address = "Ilica 35", City = "Zagreb", Capacity = 500, BookingFrom = new DateTime(2025, 6, 1), BookingTo = new DateTime(2025, 6, 3), ContactEmail = "contact@zagrebesports.hr", ContactPhone = "+385 1 234 5678", IsAvailable = true },
                new Venue { Id = 2, Name = "Split Gaming Hub", Address = "Domovinskog rata 12", City = "Split", Capacity = 300, BookingFrom = new DateTime(2025, 7, 10), BookingTo = new DateTime(2025, 7, 11), ContactEmail = "info@splitgaming.hr", ContactPhone = "+385 21 345 6789", IsAvailable = true },
                new Venue { Id = 3, Name = "Rijeka LAN Center", Address = "Korzo 5", City = "Rijeka", Capacity = 200, BookingFrom = new DateTime(2025, 8, 20), BookingTo = new DateTime(2025, 8, 21), ContactEmail = "hello@rjekalanc.hr", ContactPhone = "+385 51 456 7890", IsAvailable = true }
            );

            // Seed Sponsors
            modelBuilder.Entity<Sponsor>().HasData(
                new Sponsor { Id = 1, Name = "HT Telekom", Website = "https://www.t.ht.hr", ContactEmail = "esports@ht.hr", ContactPhone = "+385 1 111 2222", SponsorshipAmount = 5000.00m, ContractStart = new DateTime(2025, 1, 1), ContractEnd = new DateTime(2025, 12, 31) },
                new Sponsor { Id = 2, Name = "Razer", Website = "https://www.razer.com", ContactEmail = "sponsorships@razer.com", ContactPhone = "+1 800 123 4567", SponsorshipAmount = 8000.00m, ContractStart = new DateTime(2025, 1, 1), ContractEnd = new DateTime(2025, 12, 31) },
                new Sponsor { Id = 3, Name = "Red Bull", Website = "https://www.redbull.com", ContactEmail = "esports@redbull.com", ContactPhone = "+43 662 6582 0", SponsorshipAmount = 10000.00m, ContractStart = new DateTime(2025, 1, 1), ContractEnd = new DateTime(2025, 12, 31) }
            );

            // Seed Players owned type data (AccountInformation) as shadow properties
            modelBuilder.Entity<Player>().HasData(
                new { Id = 1, Name = "Mirko Horvat", GamerTag = "MirkoH", Role = PlayerRole.Player, PreferredPosition = Position.TopLane, SecondaryPosition = Position.Jungle, JoinedAt = new DateTime(2024, 1, 15), AccountInformation_SummonerName = "MirkoTop", AccountInformation_RiotTag = "MirkoH#EUW1", AccountInformation_Region = Region.EUW, AccountInformation_LeagueTier = LeagueTier.Platinum },
                new { Id = 2, Name = "Petar Marković", GamerTag = "PetarMark", Role = PlayerRole.Player, PreferredPosition = Position.Jungle, SecondaryPosition = Position.MidLane, JoinedAt = new DateTime(2024, 2, 10), AccountInformation_SummonerName = "PetarJungle", AccountInformation_RiotTag = "PetarMark#EUW1", AccountInformation_Region = Region.EUW, AccountInformation_LeagueTier = LeagueTier.Diamond },
                new { Id = 3, Name = "Ana Horvat", GamerTag = "AnaH", Role = PlayerRole.Player, PreferredPosition = Position.MidLane, SecondaryPosition = Position.ADC, JoinedAt = new DateTime(2024, 1, 20), AccountInformation_SummonerName = "AnaMid", AccountInformation_RiotTag = "AnaH#EUW1", AccountInformation_Region = Region.EUW, AccountInformation_LeagueTier = LeagueTier.Gold },
                new { Id = 4, Name = "Jovan Nikolić", GamerTag = "JovanN", Role = PlayerRole.Player, PreferredPosition = Position.ADC, SecondaryPosition = Position.Support, JoinedAt = new DateTime(2024, 3, 5), AccountInformation_SummonerName = "JovanBot", AccountInformation_RiotTag = "JovanN#EUW1", AccountInformation_Region = Region.EUNE, AccountInformation_LeagueTier = LeagueTier.Platinum },
                new { Id = 5, Name = "Marko Petrović", GamerTag = "MarkoPetro", Role = PlayerRole.TeamCaptain, PreferredPosition = Position.Support, SecondaryPosition = Position.TopLane, JoinedAt = new DateTime(2023, 12, 1), AccountInformation_SummonerName = "MarkoCaptain", AccountInformation_RiotTag = "MarkoPetro#EUW1", AccountInformation_Region = Region.EUW, AccountInformation_LeagueTier = LeagueTier.Master },
                new { Id = 6, Name = "Filip Aleksić", GamerTag = "FilipA", Role = PlayerRole.Substitute, PreferredPosition = Position.MidLane, SecondaryPosition = Position.Jungle, JoinedAt = new DateTime(2024, 4, 10), AccountInformation_SummonerName = "FilipSub", AccountInformation_RiotTag = "FilipA#EUW1", AccountInformation_Region = Region.EUW, AccountInformation_LeagueTier = LeagueTier.Gold },
                new { Id = 7, Name = "Iva Filipović", GamerTag = "IvaF", Role = PlayerRole.Substitute, PreferredPosition = Position.ADC, SecondaryPosition = Position.MidLane, JoinedAt = new DateTime(2024, 4, 15), AccountInformation_SummonerName = "IvaSub", AccountInformation_RiotTag = "IvaF#EUNE1", AccountInformation_Region = Region.EUNE, AccountInformation_LeagueTier = LeagueTier.Silver },
                new { Id = 8, Name = "Luka Kovač", GamerTag = "LukaK", Role = PlayerRole.Player, PreferredPosition = Position.TopLane, SecondaryPosition = Position.MidLane, JoinedAt = new DateTime(2024, 2, 1), AccountInformation_SummonerName = "LukaTop2", AccountInformation_RiotTag = "LukaK#EUW1", AccountInformation_Region = Region.EUW, AccountInformation_LeagueTier = LeagueTier.Gold },
                new { Id = 9, Name = "Petra Novak", GamerTag = "PetraN", Role = PlayerRole.Player, PreferredPosition = Position.Jungle, SecondaryPosition = Position.Support, JoinedAt = new DateTime(2024, 2, 20), AccountInformation_SummonerName = "PetraJungle", AccountInformation_RiotTag = "PetraN#EUW1", AccountInformation_Region = Region.EUW, AccountInformation_LeagueTier = LeagueTier.Platinum },
                new { Id = 10, Name = "Marija Bjelica", GamerTag = "MarijaB", Role = PlayerRole.Player, PreferredPosition = Position.MidLane, SecondaryPosition = Position.Jungle, JoinedAt = new DateTime(2024, 1, 10), AccountInformation_SummonerName = "MarijaMid", AccountInformation_RiotTag = "MarijaB#EUW1", AccountInformation_Region = Region.EUW, AccountInformation_LeagueTier = LeagueTier.Diamond },
                new { Id = 11, Name = "Nikola Radivojević", GamerTag = "NikolaR", Role = PlayerRole.TeamCaptain, PreferredPosition = Position.ADC, SecondaryPosition = Position.MidLane, JoinedAt = new DateTime(2023, 11, 15), AccountInformation_SummonerName = "NikolaBot", AccountInformation_RiotTag = "NikolaR#EUW1", AccountInformation_Region = Region.EUW, AccountInformation_LeagueTier = LeagueTier.Master },
                new { Id = 12, Name = "Sandra Todorović", GamerTag = "SandraT", Role = PlayerRole.Substitute, PreferredPosition = Position.Support, SecondaryPosition = Position.TopLane, JoinedAt = new DateTime(2024, 5, 1), AccountInformation_SummonerName = "SandraSup", AccountInformation_RiotTag = "SandraT#EUNE1", AccountInformation_Region = Region.EUNE, AccountInformation_LeagueTier = LeagueTier.Bronze }
            );

            // Seed Teams
            modelBuilder.Entity<Team>().HasData(
                new Team { Id = 1, Name = "Zagreb Dragons", CoachId = 1, ManagerId = 1, RegisteredAt = new DateTime(2024, 1, 1), IsRosterConfirmed = true },
                new Team { Id = 2, Name = "Split Warriors", CoachId = 2, ManagerId = 2, RegisteredAt = new DateTime(2024, 1, 5), IsRosterConfirmed = true },
                new Team { Id = 3, Name = "Rijeka Titans", CoachId = 3, ManagerId = 3, RegisteredAt = new DateTime(2024, 1, 10), IsRosterConfirmed = false }
            );

            // Seed TeamPlayers (many-to-many relationship)
            // Team 1 (Zagreb Dragons): Players 1,2,3,4,5 (starter) + 6,7 (subs)
            modelBuilder.Entity("TeamPlayers").HasData(
                new { PlayersListId = 1, TeamsId = 1 },
                new { PlayersListId = 2, TeamsId = 1 },
                new { PlayersListId = 3, TeamsId = 1 },
                new { PlayersListId = 4, TeamsId = 1 },
                new { PlayersListId = 5, TeamsId = 1 },
                new { PlayersListId = 6, TeamsId = 1 },
                new { PlayersListId = 7, TeamsId = 1 }
            );
            // Team 2 (Split Warriors): Players 8,9,10,11 (4 players + 1 captain) + 12 (sub)
            modelBuilder.Entity("TeamPlayers").HasData(
                new { PlayersListId = 8, TeamsId = 2 },
                new { PlayersListId = 9, TeamsId = 2 },
                new { PlayersListId = 10, TeamsId = 2 },
                new { PlayersListId = 11, TeamsId = 2 },
                new { PlayersListId = 12, TeamsId = 2 }
            );

            // Seed Tournaments
            modelBuilder.Entity<Tournament>().HasData(
                new Tournament { Id = 1, Name = "Spring Split 2025", Description = "Spring esports tournament", Type = TournamentType.Preliminary, Format = TournamentFormat.Online, Status = TournamentStatus.Upcoming, PrizePool = 25000m, StartDate = new DateTime(2025, 6, 1), EndDate = new DateTime(2025, 6, 3), RegistrationDeadline = new DateTime(2025, 5, 25), VenueId = 1 },
                new Tournament { Id = 2, Name = "Summer Championship 2025", Description = "Main summer tournament", Type = TournamentType.Final, Format = TournamentFormat.Offline, Status = TournamentStatus.Upcoming, PrizePool = 50000m, StartDate = new DateTime(2025, 8, 15), EndDate = new DateTime(2025, 8, 18), RegistrationDeadline = new DateTime(2025, 8, 1), VenueId = 2 }
            );

            // Seed TournamentTeams (many-to-many relationship)
            modelBuilder.Entity("TournamentTeams").HasData(
                new { TeamsListId = 1, TournamentId = 1 },
                new { TeamsListId = 2, TournamentId = 1 },
                new { TeamsListId = 1, TournamentId = 2 },
                new { TeamsListId = 2, TournamentId = 2 }
            );

            // Seed TournamentSponsors (many-to-many relationship)
            modelBuilder.Entity("TournamentSponsors").HasData(
                new { SponsorsListId = 1, TournamentId = 1 },
                new { SponsorsListId = 2, TournamentId = 1 },
                new { SponsorsListId = 2, TournamentId = 2 },
                new { SponsorsListId = 3, TournamentId = 2 }
            );
        }
    }
}
