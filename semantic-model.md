# Semantic Database Model

## Overview
This document describes the semantic data model for the League of Legends Tournament Hosting system. The model consists of 7 main entities with multiple relationships for managing tournaments, teams, players, and related resources.

## Entities and Schema

### 1. **Coach**
- **Table**: `Coaches`
- **Purpose**: Represents coaching staff members
- **Primary Key**: `Id` (int)
- **Key Properties**:
  - `Name` (string): Full name of the coach
  - `GamerTag` (string): Professional gaming nickname
  - `HiredAt` (DateTime): Date the coach was hired
  - `YearsOfExperience` (int): Professional experience in years
- **Relationships**: 1:Many with Team (through foreign key)

### 2. **Manager**
- **Table**: `Managers`
- **Purpose**: Represents team management personnel
- **Primary Key**: `Id` (int)
- **Key Properties**:
  - `Name` (string): Full name of the manager
  - `HiredAt` (DateTime): Date the manager was hired
  - `YearsOfExperience` (int): Years of management experience
- **Relationships**: 1:Many with Team (through foreign key)

### 3. **Player**
- **Table**: `Players`
- **Purpose**: Individual player accounts and profiles
- **Primary Key**: `Id` (int)
- **Key Properties**:
  - `Name` (string): Player's real name
  - `GamerTag` (string): In-game username
  - `Role` (PlayerRole enum): Primary role (Player, TeamCaptain, Substitute)
  - `PreferredPosition` (Position enum): Preferred lane/position
  - `SecondaryPosition` (Position enum): Alternative lane/position
  - `JoinedAt` (DateTime): Date player joined
  - `AccountInformation` (Owned Type): Embedded account details
    - `SummonerName` (string): League of Legends summoner name
    - `RiotTag` (string): Riot Games account tag
    - `Region` (Region enum): Server region (EUW, NA, KR, etc.)
    - `LeagueTier` (LeagueTier enum): Competitive rank (Iron-Challenger)
- **Relationships**: Many:Many with Team (through `TeamPlayers` junction table)

### 4. **Team**
- **Table**: `Teams`
- **Purpose**: Professional esports teams
- **Primary Key**: `Id` (int)
- **Key Properties**:
  - `Name` (string): Team name
  - `CoachId` (int): Foreign key to Coach
  - `Coach` (Coach): Navigation property to coach
  - `ManagerId` (int): Foreign key to Manager
  - `Manager` (Manager): Navigation property to manager
  - `PlayersList` (ICollection<Player>): Roster (5-7 players)
  - `RegisteredAt` (DateTime): Registration date
- **Constraints**:
  - Minimum 5 players, maximum 7 players
  - Must have confirmed roster for tournament participation
- **Relationships**:
  - Many:1 with Coach (CoachId foreign key)
  - Many:1 with Manager (ManagerId foreign key)
  - Many:Many with Player (TeamPlayers junction table)
  - Many:Many with Tournament (TournamentTeams junction table)

### 5. **Sponsor**
- **Table**: `Sponsors`
- **Purpose**: Financial sponsors and partners
- **Primary Key**: `Id` (int)
- **Key Properties**:
  - `Name` (string): Sponsor company name
  - `Website` (string): Official website URL
  - `ContactEmail` (string): Primary contact email
  - `ContactPhone` (string): Contact phone number
  - `SponsorshipAmount` (decimal): Sponsorship value
  - `ContractStart` (DateTime): Contract start date
  - `ContractEnd` (DateTime): Contract end date
- **Relationships**: Many:Many with Tournament (through `TournamentSponsors` junction table)

### 6. **Venue**
- **Table**: `Venues`
- **Purpose**: Physical tournament venues and event locations
- **Primary Key**: `Id` (int)
- **Key Properties**:
  - `Name` (string): Venue name
  - `Address` (string): Street address
  - `City` (string): City location
  - `Capacity` (int): Maximum attendee capacity
  - `IsAvailable` (bool): Availability status
  - `BookingFrom` (DateTime): Booking period start
  - `BookingTo` (DateTime): Booking period end
  - `ContactEmail` (string): Venue contact email
  - `ContactPhone` (string): Venue contact phone
- **Relationships**: 1:Many with Tournament (through foreign key)

### 7. **Tournament**
- **Table**: `Tournaments`
- **Purpose**: Esports tournament events
- **Primary Key**: `Id` (int)
- **Key Properties**:
  - `Name` (string): Tournament name
  - `Description` (string): Tournament description and rules
  - `Type` (TournamentType enum): Tournament type (Preliminary, Quarterfinal, Semifinal, Final)
  - `Format` (TournamentFormat enum): Match format (Single Elim, Double Elim, Round Robin, Swiss)
  - `Status` (TournamentStatus enum): Current status (Upcoming, Ongoing, Completed, Cancelled)
  - `PrizePool` (decimal): Total prize money
  - `StartDate` (DateTime): Tournament start date/time
  - `EndDate` (DateTime): Tournament end date/time
  - `RegistrationDeadline` (DateTime): Team registration deadline
  - `VenueId` (int): Foreign key to Venue
  - `Venue` (Venue): Navigation property to venue
  - `TeamsList` (ICollection<Team>): Participating teams (max 12)
  - `SponsorsList` (ICollection<Sponsor>): Associated sponsors
- **Constraints**:
  - Maximum 12 teams per tournament
  - Requires venue and at least 2 teams to start
- **Relationships**:
  - Many:1 with Venue (VenueId foreign key)
  - Many:Many with Team (TournamentTeams junction table)
  - Many:Many with Sponsor (TournamentSponsors junction table)

## Enumerations

### PlayerRole
- `Player` (0): Standard player
- `TeamCaptain` (1): Team captain/leader
- `Substitute` (2): Backup/substitute player

### Position
- `Top` (0)
- `Jungle` (1)
- `Middle` (2)
- `ADC` (3)
- `Support` (4)

### Region
- `EUW` (0): Europe West
- `EUNE` (1): Europe Nordic & East
- `NA` (2): North America
- `LAN` (3): Latin America North
- `LAS` (4): Latin America South
- `BR` (5): Brazil
- `RU` (6): Russia
- `TR` (7): Turkey
- `JP` (8): Japan
- `KR` (9): Korea
- `CN` (10): China
- `OCE` (11): Oceania

### LeagueTier
- `Iron` (0)
- `Bronze` (1)
- `Silver` (2)
- `Gold` (3)
- `Platinum` (4)
- `Diamond` (5)
- `Master` (6)
- `GrandMaster` (7)
- `Challenger` (8)

### TournamentType
- `Preliminary` (0)
- `Quarterfinal` (1)
- `Semifinal` (2)
- `Final` (3)

### TournamentFormat
- `SingleElimination` (0)
- `DoubleElimination` (1)
- `RoundRobin` (2)
- `Swiss` (3)

### TournamentStatus
- `Upcoming` (0)
- `Ongoing` (1)
- `Completed` (2)
- `Cancelled` (3)

## Relationships Summary

| From | To | Type | Junction Table | Foreign Key |
|------|-----|------|----------------|-------------|
| Team | Coach | Many:1 | - | CoachId |
| Team | Manager | Many:1 | - | ManagerId |
| Team | Player | Many:Many | TeamPlayers | - |
| Tournament | Venue | Many:1 | - | VenueId |
| Tournament | Team | Many:Many | TournamentTeams | - |
| Tournament | Sponsor | Many:Many | TournamentSponsors | - |

## Key Business Rules

1. **Team Roster Validation**: Teams must have 5-7 confirmed players before tournament participation
2. **Tournament Capacity**: Maximum 12 teams per tournament
3. **Sponsor Contracts**: Sponsorships are time-bound with ContractStart and ContractEnd
4. **Player Rank**: Players are classified by League Tier and assigned positions
5. **Venue Availability**: Venues have booking windows specified by BookingFrom and BookingTo
6. **Status Transitions**: Tournaments progress through Upcoming → Ongoing → Completed (or Cancelled)

## Data Access Layer Configuration

- **DbContext**: `TournamentDbContext`
- **Connection String**: Configured in `appsettings.json` as `TournamentDbContext`
- **Database Provider**: SQL Server LocalDB
- **Migrations Assembly**: `League of Legends Tournament Hosting`
