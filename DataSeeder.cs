using System.Diagnostics;
using League_of_Legends_Tournament_Hosting.Models;

namespace League_of_Legends_Tournament_Hosting
{
    public static class DataSeeder
    {
        public static void Seed()
        {
            // ── VENUES ──────────────────────────────────────────────────────
            var venue1 = new Venue(1, "Zagreb Esports Arena", "Ilica 35", "Zagreb", 500,
                new DateTime(2025, 6, 1), new DateTime(2025, 6, 3),
                "contact@zagrebesports.hr", "+385 1 234 5678");

            var venue2 = new Venue(2, "Split Gaming Hub", "Domovinskog rata 12", "Split", 300,
                new DateTime(2025, 7, 10), new DateTime(2025, 7, 11),
                "info@splitgaming.hr", "+385 21 345 6789");

            var venue3 = new Venue(3, "Rijeka LAN Center", "Korzo 5", "Rijeka", 200,
                new DateTime(2025, 8, 20), new DateTime(2025, 8, 21),
                "hello@rjekalanc.hr", "+385 51 456 7890");

            // ── SPONSORS ─────────────────────────────────────────────────────
            var sponsor1 = new Sponsor(1, "HT Telekom", "https://www.t.ht.hr", "esports@ht.hr",
                "+385 1 111 2222", 5000.00m,
                new DateTime(2025, 1, 1), new DateTime(2025, 12, 31));

            var sponsor2 = new Sponsor(2, "Razer", "https://www.razer.com", "sponsorships@razer.com",
                "+1 800 123 4567", 8000.00m,
                new DateTime(2025, 1, 1), new DateTime(2025, 12, 31));

            var sponsor3 = new Sponsor(3, "Red Bull", "https://www.redbull.com", "esports@redbull.com",
                "+43 662 6582 0", 10000.00m,
                new DateTime(2025, 1, 1), new DateTime(2025, 12, 31));

            // ── COACHES ──────────────────────────────────────────────────────
            var coach1 = new Coach(1, "Ivan Horvat", "IvanCoach", new DateTime(2022, 3, 15), 5);
            var coach2 = new Coach(2, "Marko Perić", "MarkoPeric", new DateTime(2021, 6, 1), 7);
            var coach3 = new Coach(3, "Luka Novak", "LukaN", new DateTime(2023, 1, 10), 3);
            var coach4 = new Coach(4, "Tomislav Blažević", "TomiBlazer", new DateTime(2020, 9, 5), 9);
            var coach5 = new Coach(5, "Ante Jurić", "AnteJ", new DateTime(2022, 11, 20), 4);
            var coach6 = new Coach(6, "Nikola Šarić", "NikoSaric", new DateTime(2019, 4, 12), 11);

            // ── MANAGERS ─────────────────────────────────────────────────────
            var manager1 = new Manager(1, "Petra Kovač", new DateTime(2022, 3, 15), 4);
            var manager2 = new Manager(2, "Ana Babić", new DateTime(2021, 6, 1), 6);
            var manager3 = new Manager(3, "Maja Tomić", new DateTime(2023, 1, 10), 2);
            var manager4 = new Manager(4, "Sara Marić", new DateTime(2020, 9, 5), 8);
            var manager5 = new Manager(5, "Iva Paulić", new DateTime(2022, 11, 20), 3);
            var manager6 = new Manager(6, "Dora Knežević", new DateTime(2019, 4, 12), 10);

            // ── PLAYERS ──────────────────────────────────────────────────────

            // Team 1 - Midnight Wolves
            var p1 = new Player(1, "Dario Šimić", "DarkWolf", PlayerRole.TeamCaptain, Position.MidLane, Position.TopLane,
                new AccountInformation("DarkWolf", "DarkWolf#EUW", Region.EUW, LeagueTier.Diamond), new DateTime(2023, 1, 5));
            var p2 = new Player(2, "Leon Butković", "LeonB", PlayerRole.Player, Position.TopLane, Position.MidLane,
                new AccountInformation("LeonB", "LeonB#EUW", Region.EUW, LeagueTier.Platinum), new DateTime(2023, 1, 5));
            var p3 = new Player(3, "Fran Gašpar", "FranG", PlayerRole.Player, Position.Jungle, Position.Support,
                new AccountInformation("FranG", "FranG#EUW", Region.EUW, LeagueTier.Gold), new DateTime(2023, 1, 5));
            var p4 = new Player(4, "Karlo Vuković", "KarloV", PlayerRole.Player, Position.ADC, Position.MidLane,
                new AccountInformation("KarloV", "KarloV#EUW", Region.EUW, LeagueTier.Platinum), new DateTime(2023, 1, 5));
            var p5 = new Player(5, "Bruno Stipić", "BrunoS", PlayerRole.Player, Position.Support, Position.ADC,
                new AccountInformation("BrunoS", "BrunoS#EUW", Region.EUW, LeagueTier.Gold), new DateTime(2023, 1, 5));

            // Team 2 - Neon Dragons
            var p6 = new Player(6, "Tin Matić", "NeonX", PlayerRole.TeamCaptain, Position.ADC, Position.MidLane,
                new AccountInformation("NeonX", "NeonX#EUW", Region.EUW, LeagueTier.Diamond), new DateTime(2023, 2, 10));
            var p7 = new Player(7, "Roko Filipović", "RokoF", PlayerRole.Player, Position.TopLane, Position.Jungle,
                new AccountInformation("RokoF", "RokoF#EUW", Region.EUW, LeagueTier.Gold), new DateTime(2023, 2, 10));
            var p8 = new Player(8, "Josip Ivanić", "JosipI", PlayerRole.Player, Position.Jungle, Position.TopLane,
                new AccountInformation("JosipI", "JosipI#EUW", Region.EUW, LeagueTier.Silver), new DateTime(2023, 2, 10));
            var p9 = new Player(9, "Marin Blažić", "MarinB", PlayerRole.Player, Position.MidLane, Position.ADC,
                new AccountInformation("MarinB", "MarinB#EUW", Region.EUW, LeagueTier.Platinum), new DateTime(2023, 2, 10));
            var p10 = new Player(10, "Stipe Jurić", "StipeJ", PlayerRole.Player, Position.Support, Position.MidLane,
                new AccountInformation("StipeJ", "StipeJ#EUW", Region.EUW, LeagueTier.Gold), new DateTime(2023, 2, 10));

            // Team 3 - Iron Phantoms
            var p11 = new Player(11, "Vedran Knežević", "VedranK", PlayerRole.TeamCaptain, Position.Jungle, Position.MidLane,
                new AccountInformation("VedranK", "VedranK#EUNE", Region.EUNE, LeagueTier.Emerald), new DateTime(2023, 3, 15));
            var p12 = new Player(12, "Domagoj Sorić", "DomaS", PlayerRole.Player, Position.TopLane, Position.Jungle,
                new AccountInformation("DomaS", "DomaS#EUNE", Region.EUNE, LeagueTier.Platinum), new DateTime(2023, 3, 15));
            var p13 = new Player(13, "Patrik Pavlović", "PatrikP", PlayerRole.Player, Position.MidLane, Position.TopLane,
                new AccountInformation("PatrikP", "PatrikP#EUNE", Region.EUNE, LeagueTier.Gold), new DateTime(2023, 3, 15));
            var p14 = new Player(14, "Damir Čović", "DamirC", PlayerRole.Player, Position.ADC, Position.Support,
                new AccountInformation("DamirC", "DamirC#EUNE", Region.EUNE, LeagueTier.Silver), new DateTime(2023, 3, 15));
            var p15 = new Player(15, "Zvonimir Grubić", "ZvoniG", PlayerRole.Player, Position.Support, Position.ADC,
                new AccountInformation("ZvoniG", "ZvoniG#EUNE", Region.EUNE, LeagueTier.Gold), new DateTime(2023, 3, 15));

            // Team 4 - Solar Rift
            var p16 = new Player(16, "Matej Herceg", "SolarM", PlayerRole.TeamCaptain, Position.TopLane, Position.MidLane,
                new AccountInformation("SolarM", "SolarM#EUW", Region.EUW, LeagueTier.Diamond), new DateTime(2023, 4, 1));
            var p17 = new Player(17, "Dominik Vrban", "DomVrban", PlayerRole.Player, Position.MidLane, Position.ADC,
                new AccountInformation("DomVrban", "DomVrban#EUW", Region.EUW, LeagueTier.Platinum), new DateTime(2023, 4, 1));
            var p18 = new Player(18, "Borna Šestić", "BornaS", PlayerRole.Player, Position.Jungle, Position.TopLane,
                new AccountInformation("BornaS", "BornaS#EUW", Region.EUW, LeagueTier.Gold), new DateTime(2023, 4, 1));
            var p19 = new Player(19, "Niko Radić", "NikoR", PlayerRole.Player, Position.ADC, Position.Jungle,
                new AccountInformation("NikoR", "NikoR#EUW", Region.EUW, LeagueTier.Emerald), new DateTime(2023, 4, 1));
            var p20 = new Player(20, "Luka Štimac", "LukaS", PlayerRole.Player, Position.Support, Position.MidLane,
                new AccountInformation("LukaS", "LukaS#EUW", Region.EUW, LeagueTier.Platinum), new DateTime(2023, 4, 1));

            // Team 5 - Crimson Tide
            var p21 = new Player(21, "Ivan Petrović", "IvanP", PlayerRole.TeamCaptain, Position.Support, Position.ADC,
                new AccountInformation("IvanP", "IvanP#EUNE", Region.EUNE, LeagueTier.Diamond), new DateTime(2023, 5, 5));
            var p22 = new Player(22, "Marko Vidić", "MarkoV", PlayerRole.Player, Position.TopLane, Position.Jungle,
                new AccountInformation("MarkoV", "MarkoV#EUNE", Region.EUNE, LeagueTier.Gold), new DateTime(2023, 5, 5));
            var p23 = new Player(23, "Tomislav Ćorić", "TomiC", PlayerRole.Player, Position.MidLane, Position.TopLane,
                new AccountInformation("TomiC", "TomiC#EUNE", Region.EUNE, LeagueTier.Silver), new DateTime(2023, 5, 5));
            var p24 = new Player(24, "Goran Lučić", "GoranL", PlayerRole.Player, Position.Jungle, Position.MidLane,
                new AccountInformation("GoranL", "GoranL#EUNE", Region.EUNE, LeagueTier.Platinum), new DateTime(2023, 5, 5));
            var p25 = new Player(25, "Filip Barić", "FilipB", PlayerRole.Player, Position.ADC, Position.Support,
                new AccountInformation("FilipB", "FilipB#EUNE", Region.EUNE, LeagueTier.Gold), new DateTime(2023, 5, 5));

            // Team 6 - Void Walkers
            var p26 = new Player(26, "Ante Tomašević", "AnteT", PlayerRole.TeamCaptain, Position.MidLane, Position.ADC,
                new AccountInformation("AnteT", "AnteT#EUW", Region.EUW, LeagueTier.Emerald), new DateTime(2023, 6, 10));
            var p27 = new Player(27, "Stjepan Knez", "StjeK", PlayerRole.Player, Position.TopLane, Position.MidLane,
                new AccountInformation("StjeK", "StjeK#EUW", Region.EUW, LeagueTier.Gold), new DateTime(2023, 6, 10));
            var p28 = new Player(28, "Petar Galić", "PetarG", PlayerRole.Player, Position.Jungle, Position.ADC,
                new AccountInformation("PetarG", "PetarG#EUW", Region.EUW, LeagueTier.Platinum), new DateTime(2023, 6, 10));
            var p29 = new Player(29, "Kristijan Vukić", "KrisV", PlayerRole.Player, Position.ADC, Position.Jungle,
                new AccountInformation("KrisV", "KrisV#EUW", Region.EUW, LeagueTier.Silver), new DateTime(2023, 6, 10));
            var p30 = new Player(30, "Robert Ančić", "RobertA", PlayerRole.Player, Position.Support, Position.TopLane,
                new AccountInformation("RobertA", "RobertA#EUW", Region.EUW, LeagueTier.Gold), new DateTime(2023, 6, 10));

            // ── TEAMS ─────────────────────────────────────────────────────────
            var team1 = new Team(1, "Midnight Wolves", coach1, manager1, new List<Player> { p1, p2, p3, p4, p5 }, new DateTime(2025, 5, 1));
            team1.ConfirmRoster();

            var team2 = new Team(2, "Neon Dragons", coach2, manager2, new List<Player> { p6, p7, p8, p9, p10 }, new DateTime(2025, 5, 2));
            team2.ConfirmRoster();

            var team3 = new Team(3, "Iron Phantoms", coach3, manager3, new List<Player> { p11, p12, p13, p14, p15 }, new DateTime(2025, 5, 3));
            team3.ConfirmRoster();

            var team4 = new Team(4, "Solar Rift", coach4, manager4, new List<Player> { p16, p17, p18, p19, p20 }, new DateTime(2025, 5, 4));
            team4.ConfirmRoster();

            var team5 = new Team(5, "Crimson Tide", coach5, manager5, new List<Player> { p21, p22, p23, p24, p25 }, new DateTime(2025, 5, 5));
            team5.ConfirmRoster();

            var team6 = new Team(6, "Void Walkers", coach6, manager6, new List<Player> { p26, p27, p28, p29, p30 }, new DateTime(2025, 5, 6));
            team6.ConfirmRoster();

            // ── TOURNAMENTS ───────────────────────────────────────────────────
            var tournament1 = new Tournament(1, "Croatia Clash Preliminaries", "Open preliminary stage for all registered teams.",
                TournamentType.Preliminary, TournamentFormat.Online, 1000.00m,
                new DateTime(2025, 6, 1), new DateTime(2025, 6, 3),
                new DateTime(2025, 5, 25), venue1);

            tournament1.RegisterTeam(team1);
            tournament1.RegisterTeam(team2);
            tournament1.RegisterTeam(team3);
            tournament1.AddSponsor(sponsor1);
            tournament1.AddSponsor(sponsor2);
            tournament1.StartTournament();

            var tournament2 = new Tournament(2, "Croatia Clash Semifinals", "Top teams battle for a spot in the finals.",
                TournamentType.Semifinal, TournamentFormat.Offline, 3000.00m,
                new DateTime(2025, 7, 10), new DateTime(2025, 7, 11),
                new DateTime(2025, 6, 30), venue2);

            tournament2.RegisterTeam(team4);
            tournament2.RegisterTeam(team5);
            tournament2.AddSponsor(sponsor2);
            tournament2.AddSponsor(sponsor3);

            var tournament3 = new Tournament(3, "Croatia Clash Grand Final", "The ultimate showdown for the championship.",
                TournamentType.Final, TournamentFormat.Offline, 10000.00m,
                new DateTime(2025, 8, 20), new DateTime(2025, 8, 21),
                new DateTime(2025, 8, 10), venue3);

            tournament3.RegisterTeam(team6);
            tournament3.AddSponsor(sponsor1);
            tournament3.AddSponsor(sponsor3);

            // ── LINQ QUERIES ──────────────────────────────────────────────────
            var allTeams = new List<Team> { team1, team2, team3, team4, team5, team6 };
            var allTournaments = new List<Tournament> { tournament1, tournament2, tournament3 };
            var allPlayers = allTeams.SelectMany(t => t.Players).ToList();

            // 1. All midlaners across all teams
            var midlaners = allPlayers
                .Where(p => p.PreferredPosition == Position.MidLane)
                .ToList();
            Debug.WriteLine($"\n--- Midlaners ({midlaners.Count}) ---");
            midlaners.ForEach(p => Debug.WriteLine($"  {p.GamerTag} ({p.AccountInformation.LeagueTier})"));

            // 2. All teams with a confirmed roster
            var confirmedTeams = allTeams
                .Where(t => t.IsRosterConfirmed)
                .ToList();
            Debug.WriteLine($"\n--- Confirmed Rosters ({confirmedTeams.Count}) ---");
            confirmedTeams.ForEach(t => Debug.WriteLine($"  {t.Name}"));

            // 3. All Diamond+ players
            var elitePlayers = allPlayers
                .Where(p => p.AccountInformation.LeagueTier >= LeagueTier.Diamond)
                .OrderByDescending(p => p.AccountInformation.LeagueTier)
                .ToList();
            Debug.WriteLine($"\n--- Diamond+ Players ({elitePlayers.Count}) ---");
            elitePlayers.ForEach(p => Debug.WriteLine($"  {p.GamerTag} - {p.AccountInformation.LeagueTier}"));

            // 4. All offline tournaments
            var offlineTournaments = allTournaments
                .Where(t => t.Format == TournamentFormat.Offline)
                .ToList();
            Debug.WriteLine($"\n--- Offline Tournaments ({offlineTournaments.Count}) ---");
            offlineTournaments.ForEach(t => Debug.WriteLine($"  {t.Name}"));

            // 5. Total prize pool across all tournaments
            var totalPrizePool = allTournaments
                .Sum(t => t.PrizePool);
            Debug.WriteLine($"\n--- Total Prize Pool ---");
            Debug.WriteLine($"  {totalPrizePool:C}");

            // 6. Teams sorted by registration date
            var teamsByDate = allTeams
                .OrderBy(t => t.RegisteredAt)
                .ToList();
            Debug.WriteLine($"\n--- Teams by Registration Date ---");
            teamsByDate.ForEach(t => Debug.WriteLine($"  {t.Name} - {t.RegisteredAt:dd.MM.yyyy}"));

            // 7. Most experienced coach
            var allCoaches = allTeams.Select(t => t.Coach).ToList();
            var topCoach = allCoaches
                .OrderByDescending(c => c.YearsOfExperience)
                .First();
            Debug.WriteLine($"\n--- Most Experienced Coach ---");
            Debug.WriteLine($"  {topCoach.Name} ({topCoach.YearsOfExperience} years)");
        }
    }
}