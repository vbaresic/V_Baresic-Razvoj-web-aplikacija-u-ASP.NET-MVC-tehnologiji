# Database Summary - Quick Reference

## Overview
Compact summary of all 7 entities (models/classes/tables) with key properties and relationships in the League of Legends Tournament Hosting system.

---

## Entities Overview

| # | Entity | Table | Primary Key | Purpose |
|---|--------|-------|------------|---------|
| 1 | `Coach` | Coaches | Id (int) | Coaching staff members |
| 2 | `Manager` | Managers | Id (int) | Team management personnel |
| 3 | `Player` | Players | Id (int) | Individual player accounts |
| 4 | `Team` | Teams | Id (int) | Professional esports teams |
| 5 | `Sponsor` | Sponsors | Id (int) | Financial sponsors/partners |
| 6 | `Venue` | Venues | Id (int) | Tournament locations |
| 7 | `Tournament` | Tournaments | Id (int) | Tournament events |

---

## Entity Properties (Key Attributes)

### 1. Coach
| Property | Type | Notes |
|----------|------|-------|
| Id | int | Primary Key |
| Name | string | Full name |
| GamerTag | string | Professional nickname |
| HiredAt | DateTime | Hire date |
| YearsOfExperience | int | Professional years |

### 2. Manager
| Property | Type | Notes |
|----------|------|-------|
| Id | int | Primary Key |
| Name | string | Full name |
| HiredAt | DateTime | Hire date |
| YearsOfExperience | int | Management years |

### 3. Player
| Property | Type | Notes |
|----------|------|-------|
| Id | int | Primary Key |
| Name | string | Real name |
| GamerTag | string | In-game username |
| Role | PlayerRole enum | Player / TeamCaptain / Substitute |
| PreferredPosition | Position enum | TopLane, MidLane, Jungle, ADC, Support |
| SecondaryPosition | Position enum | Alternative position |
| JoinedAt | DateTime | Join date |
| **AccountInformation** | Owned Type | **Embedded object** |
| ├─ SummonerName | string | LoL summoner name |
| ├─ RiotTag | string | Riot Games tag |
| ├─ Region | Region enum | Server region |
| └─ LeagueTier | LeagueTier enum | Rank (Iron-Challenger) |

### 4. Team
| Property | Type | Notes |
|----------|------|-------|
| Id | int | Primary Key |
| Name | string | Team name |
| CoachId | int | **Foreign Key** → Coach |
| ManagerId | int | **Foreign Key** → Manager |
| RegisteredAt | DateTime | Registration date |
| IsRosterConfirmed | bool | Roster confirmation status |
| **Constraints**: 5-7 players min/max |

### 5. Sponsor
| Property | Type | Notes |
|----------|------|-------|
| Id | int | Primary Key |
| Name | string | Company name |
| Website | string | Website URL |
| ContactEmail | string | Contact email |
| ContactPhone | string | Contact phone |
| SponsorshipAmount | decimal | Sponsorship value |
| ContractStart | DateTime | Contract start date |
| ContractEnd | DateTime | Contract end date |

### 6. Venue
| Property | Type | Notes |
|----------|------|-------|
| Id | int | Primary Key |
| Name | string | Venue name |
| Address | string | Street address |
| City | string | City location |
| Capacity | int | Max attendees |
| IsAvailable | bool | Availability status |
| BookingFrom | DateTime | Booking period start |
| BookingTo | DateTime | Booking period end |
| ContactEmail | string | Contact email |
| ContactPhone | string | Contact phone |

### 7. Tournament
| Property | Type | Notes |
|----------|------|-------|
| Id | int | Primary Key |
| Name | string | Tournament name |
| Description | string | Rules/description |
| Type | TournamentType enum | Preliminary, Quarterfinal, Semifinal, Final |
| Format | TournamentFormat enum | Online / Offline |
| Status | TournamentStatus enum | Upcoming, Ongoing, Completed, Cancelled |
| PrizePool | decimal | Prize money |
| StartDate | DateTime | Start date/time |
| EndDate | DateTime | End date/time |
| RegistrationDeadline | DateTime | Registration deadline |
| VenueId | int | **Foreign Key** → Venue |
| **Constraints**: Max 12 teams |

---

## Database Relationships

### Direct Foreign Key Relationships (Many:1)

| From Table | To Table | Foreign Key | Cardinality |
|-----------|----------|-------------|-------------|
| Teams | Coaches | CoachId | Many:1 |
| Teams | Managers | ManagerId | Many:1 |
| Tournaments | Venues | VenueId | Many:1 |

### Many-to-Many Relationships (Via Junction Tables)

| Entity 1 | Entity 2 | Junction Table | Purpose |
|----------|----------|----------------|---------|
| Team | Player | **TeamPlayers** | Team rosters |
| Tournament | Team | **TournamentTeams** | Team registration |
| Tournament | Sponsor | **TournamentSponsors** | Sponsor association |

### Relationship Diagram (Simplified)

```
Coach ←─ Team ─→ Manager
             ↓
         Player (via TeamPlayers M:M)
             
Tournament ←─ Venue
    ↓
  Team (via TournamentTeams M:M)
    ↓
 Sponsor (via TournamentSponsors M:M)
```

---

## Enumerations

### PlayerRole
- `Player` (0) - Standard player
- `TeamCaptain` (1) - Team leader
- `Substitute` (2) - Backup player

### Position
- `TopLane` (0), `MidLane` (1), `Jungle` (2), `ADC` (3), `Support` (4), `TBD` (5)

### Region
- `EUW` (0), `EUNE` (1), `NA` (2), `LAN` (3), `LAS` (4), `BR` (5), `RU` (6), `TR` (7), `JP` (8), `KR` (9), `CN` (10), `OCE` (11)

### LeagueTier
- `Iron` (0), `Bronze` (1), `Silver` (2), `Gold` (3), `Platinum` (4), `Diamond` (5), `Master` (6), `GrandMaster` (7), `Challenger` (8)

### TournamentType
- `Preliminary` (0), `Quarterfinal` (1), `Semifinal` (2), `Final` (3)

### TournamentFormat
- `Online` (0), `Offline` (1)

### TournamentStatus
- `Upcoming` (0), `Ongoing` (1), `Completed` (2), `Cancelled` (3)

---

## Key Business Rules

✅ **Team Roster**: Must have 5-7 confirmed players  
✅ **Tournament Capacity**: Max 12 teams per tournament  
✅ **Sponsor Contracts**: Time-bound with ContractStart/ContractEnd  
✅ **Player Classification**: Classified by LeagueTier and Position  
✅ **Venue Booking**: Has BookingFrom and BookingTo windows  
✅ **Tournament Status**: Transitions Upcoming → Ongoing → Completed/Cancelled

---

## Seeded Data

**Initial Population** (from migrations):
- 6 Coaches
- 6 Managers  
- 3 Venues
- 3 Sponsors
- 12 Players (with AccountInformation)
- 3 Teams (roster 5-7 confirmed)
- 2 Tournaments
- 12 TeamPlayers relationships
- 4 TournamentTeams relationships
- 4 TournamentSponsors relationships

---

## Files & References

- **Models Location**: `/Models/` (7 C# classes)
- **DbContext**: `/Data/TournamentDbContext.cs`
- **Migrations**: `/Migrations/` (3 migration files)
- **Detailed Documentation**: See `semantic-model.md`
- **Routing & Views**: See `sitemap.md`
