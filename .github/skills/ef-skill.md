---
name: EF Model Configuration Skill
description: Use this skill when you need to add or modify Entity Framework models, generate migrations, or manage database schema changes in the League of Legends Tournament Hosting project.
trigger: "EF|model|migration|DbContext|Entity Framework|database schema"
---

# EF Model Configuration Skill

## Purpose
This skill is invoked when working with Entity Framework Core in the League of Legends Tournament Hosting project. Use it for:
- Adding or modifying entity models
- Configuring database relationships
- Generating and applying migrations
- Managing the database schema
- Updating DbContext configurations

## When to Use This Skill
- "Add a new Player field called `IsActive`"
- "Create a migration for the model changes"
- "Configure a many-to-many relationship between Team and Tournament"
- "Update the Coach model to include CertificationLevel"
- "Fix a foreign key constraint issue"
- "Apply pending migrations to the database"

## Workflow

### 1. Understand the Current Model
- Identify which entity/model needs modification
- Review the current DbContext configuration in `Data/TournamentDbContext.cs`
- Check existing migrations in `Migrations/` folder

### 2. Modify the Model
- Update the model class in `Models/{EntityName}.cs`
- Add `[Key]`, `[ForeignKey]`, or other data annotations as needed
- For relationships: Add navigation properties and mark as `virtual`
- Add `public ICollection<T>` for 1-Many or Many-Many relationships
- Ensure parameterless constructor exists for EF Core

### 3. Update DbContext Configuration
- Modify `OnModelCreating()` in `TournamentDbContext.cs` if needed
- Configure relationships using `HasOne()`, `HasMany()`, `WithMany()`
- Use `.UsingEntity()` for Many-Many join tables
- Configure owned types with `.OwnsOne()`

### 4. Generate Migration
```powershell
dotnet-ef migrations add {MigrationName} --context "League_of_Legends_Tournament_Hosting.Data.TournamentDbContext"
```

Example:
```powershell
dotnet-ef migrations add AddPlayerCertification --context "League_of_Legends_Tournament_Hosting.Data.TournamentDbContext"
```

### 5. Apply Migration
```powershell
dotnet-ef database update --context "League_of_Legends_Tournament_Hosting.Data.TournamentDbContext"
```

### 6. Test Changes
- Build the project: `dotnet build`
- Verify database schema changes
- Test affected controllers and views

## Best Practices

1. **Naming Conventions**:
   - Entity names are singular (Team, not Teams)
   - DbSet properties are plural (Teams, not Team)
   - Foreign key properties follow pattern: `{EntityName}Id`

2. **Relationship Configuration**:
   - Always use `virtual` for navigation properties
   - Use `[ForeignKey]` attribute or `.HasForeignKey()` in OnModelCreating
   - Use `.UsingEntity()` for explicit Many-Many configurations

3. **Data Annotations**:
   - Always use `[Key]` for primary keys
   - Use `[Required]` for non-nullable properties
   - Use `[StringLength(n)]` to limit string properties
   - Use `[Precision(precision, scale)]` for decimal values

4. **Owned Types**:
   - Use for value objects like `AccountInformation`
   - Configure with `.OwnsOne()` in OnModelCreating
   - These don't have separate database tables

5. **Migration Naming**:
   - Use descriptive names: `AddPlayerCertification`, `RemoveCoachEmail`, `UpdateTournamentStatus`
   - Not: `Migration1`, `Fix`, `Update`

## Common Tasks

### Add a New Property to an Entity
1. Open the model file: `Models/{EntityName}.cs`
2. Add the property with proper type and data annotations
3. Generate migration: `dotnet-ef migrations add {MigrationName}...`
4. Apply migration: `dotnet-ef database update...`

### Create a New Entity
1. Create new model file: `Models/{EntityName}.cs`
2. Add `[Key]` to Id property
3. Add navigation properties with `virtual` keyword
4. Add `DbSet<{EntityName}>` to `TournamentDbContext`
5. Configure relationships in `OnModelCreating()`
6. Generate and apply migration

### Configure a Relationship
In `TournamentDbContext.OnModelCreating()`:
```csharp
modelBuilder.Entity<Team>()
    .HasOne(t => t.Coach)
    .WithMany()
    .HasForeignKey(t => t.CoachId)
    .IsRequired();
```

### Seed Initial Data
In `TournamentDbContext.OnModelCreating()`:
```csharp
modelBuilder.Entity<Coach>().HasData(
    new Coach { Id = 1, Name = "Coach Name", ... }
);
```

## Current Model Status

### Entities
- Coach (✓ Configured)
- Manager (✓ Configured)
- Player (✓ Configured with owned AccountInformation)
- Team (✓ Configured with Coach, Manager, Players relationships)
- Sponsor (✓ Configured)
- Venue (✓ Configured)
- Tournament (✓ Configured with Venue, Teams, Sponsors relationships)

### Junction Tables (Auto-generated)
- TeamPlayers (Team Many:Many Player)
- TournamentTeams (Tournament Many:Many Team)
- TournamentSponsors (Tournament Many:Many Sponsor)

## Troubleshooting

### Migration Fails to Apply
- Verify SQL Server LocalDB is running
- Check connection string in `appsettings.json`
- Ensure all model properties are nullable or have defaults
- Check for circular dependencies in relationships

### Build Fails After Model Change
- Ensure all navigation properties are `public` and `virtual`
- Verify `[ForeignKey]` attributes are correct
- Check that owned types are properly configured
- Rebuild project: `dotnet build`

### Shadow Properties Warning
- If EF creates shadow properties, explicitly add foreign key properties
- Example: Add `public int CoachId { get; set; }` to Team model
- Use `[ForeignKey("Coach")]` attribute

## Project Structure

```
League of Legends Tournament Hosting/
├── Models/                          # Entity models
│   ├── Coach.cs
│   ├── Manager.cs
│   ├── Player.cs
│   ├── Team.cs
│   ├── Sponsor.cs
│   ├── Venue.cs
│   ├── Tournament.cs
│   └── AccountInformation.cs        # Owned type
├── Data/
│   ├── TournamentDbContext.cs       # DbContext class
│   └── .../Migrations/              # Migration files
├── Controllers/                     # Updated to use DbContext
└── appsettings.json                 # Connection strings
```

## Connection String

Located in `appsettings.json`:
```json
"ConnectionStrings": {
  "TournamentDbContext": "Server=(localdb)\\mssqllocaldb;Database=LoL_Tournament;Trusted_Connection=true;TrustServerCertificate=True;MultipleActiveResultSets=True;"
}
```

Database: `LoL_Tournament`
Provider: SQL Server LocalDB
