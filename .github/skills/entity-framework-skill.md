---
name: entity-framework
description: Handles Entity Framework related tasks such as modifying EF classes, generating migrations, updating models, and database schema changes.
trigger: "EF|entity framework|migration|model|DbContext|database schema|foreign key|relationship"
---

# Entity Framework Skill

## Purpose
This skill is invoked when working with Entity Framework Core in the ASP.NET Core MVC application. It covers modifying models, configuring relationships, generating migrations, and managing database schema changes.

## When to Use This Skill
- "Add a new property to the Player model"
- "Create a many-to-many relationship between Teams and Tournaments"
- "Generate a migration for model changes"
- "Configure owned types for embedded data"
- "Fix a foreign key constraint"
- "Update DbContext for new entities"

## Workflow

### 1. Modify Entity Models

Update the model class in `Models/{Entity}.cs`:
```csharp
public class Entity
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    
    // Add new property
    public string NewProperty { get; set; }
    
    // Navigation property
    public virtual RelatedEntity RelatedEntity { get; set; }
    
    // Foreign key
    public int RelatedEntityId { get; set; }
}
```

### 2. Configure Relationships in DbContext

Modify `Data/TournamentDbContext.cs`:
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // One-to-Many
    modelBuilder.Entity<Entity>()
        .HasOne(e => e.RelatedEntity)
        .WithMany(r => r.Entities)
        .HasForeignKey(e => e.RelatedEntityId);
    
    // Many-to-Many
    modelBuilder.Entity<Entity>()
        .HasMany(e => e.RelatedEntities)
        .WithMany(r => r.Entities)
        .UsingEntity(j => j.ToTable("EntityRelatedEntities"));
    
    // Owned type
    modelBuilder.Entity<Entity>().OwnsOne(e => e.OwnedData);
}
```

### 3. Generate Migration

Run in terminal:
```powershell
dotnet ef migrations add MigrationName --context TournamentDbContext
```

### 4. Apply Migration

```powershell
dotnet ef database update --context TournamentDbContext
```

### 5. Handle Migration Issues

- If migration fails, check model configurations
- For data loss, create custom migration with data migration
- Rollback if needed: `dotnet ef database update PreviousMigration`

### 6. Test Changes

- Verify database schema updates
- Test CRUD operations
- Check relationships load correctly
- Validate data integrity