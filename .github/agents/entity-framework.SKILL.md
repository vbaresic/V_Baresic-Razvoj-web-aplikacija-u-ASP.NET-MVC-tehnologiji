---
name: entity-framework
description: Handles Entity Framework related tasks such as modifying EF classes, generating migrations, updating models, and database schema changes.
---

This skill is invoked when the user needs to:
- Add or modify Entity Framework model classes (e.g., adding properties, relationships).
- Generate or apply database migrations.
- Update DbContext configurations.
- Handle database schema changes or seeding.

When using this skill:
- Ensure models follow EF conventions (e.g., [Key], virtual properties for navigation).
- Use `dotnet ef migrations add <name>` for new migrations.
- Apply migrations with `dotnet ef database update`.
- Validate relationships and foreign keys in OnModelCreating if needed.