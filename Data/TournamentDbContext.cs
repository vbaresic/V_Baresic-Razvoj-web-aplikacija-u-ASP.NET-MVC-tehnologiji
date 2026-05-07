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
        }
    }
}
