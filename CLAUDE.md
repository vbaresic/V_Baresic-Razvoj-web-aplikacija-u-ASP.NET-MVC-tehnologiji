# Project Instructions

## Project Context
This is an ASP.NET Core MVC application for hosting League of Legends tournaments, built with Razor Views.
It uses Entity Framework Core with SQL Server LocalDB for persistence.

**DbContext**: `League_of_Legends_Tournament_Hosting.Data.TournamentDbContext`
**Connection string DB**: `LoL_Tournament` (Server=(localdb)\\mssqllocaldb)

## Sub-Agent Delegation

### UX Sub-Agent
**Agent**: `.claude/agents/ux-sub-agent.md`
**Trigger** — spawn UX Sub-Agent when:
- User asks to generate or modify any `.cshtml` view file
- User asks to write or modify CSS in `/wwwroot/css/`
- User asks to create layout or partial view files
- User asks to build navigation, cards, lists, detail pages, or any UI components
- User explicitly requests "UX" or "design system" work

When delegating UI work, use the Agent tool with the ux-sub-agent and log the handoff:
> [SUB-AGENT INVOKED] UX Sub-Agent — applying design system from ux-sub-agent.md

## Logging Protocol
- When starting any request, begin response with:
  `[MAIN-AGENT START] {brief description}`
  `**User Prompt:** "{full user prompt as literally written}"`
- When delegating to a sub-agent:
  `[SUB-AGENT INVOKED] {agent name} — **Original Prompt:** "{full user prompt}"`
  `**Reason:** {delegation reason}`
- When a sub-agent finishes:
  `[SUB-AGENT DONE] {agent name} | Files: {files affected}`
- When returning to main context:
  `[MAIN-AGENT RESUME]`

Always log the complete user prompt exactly as provided.

## General Rules
- Navigation must be complete: menu, list-to-details links, breadcrumbs.
- No inline styles — all CSS goes in `/wwwroot/css/site.css` or component-specific stylesheets.
- Use async/await for all EF queries.

## Entity Framework Skill

### Commands
```powershell
# Add migration
dotnet-ef migrations add {MigrationName} --context "League_of_Legends_Tournament_Hosting.Data.TournamentDbContext"

# Apply migration
dotnet-ef database update --context "League_of_Legends_Tournament_Hosting.Data.TournamentDbContext"
```

### Model Conventions
- Entity names are singular (Team, not Teams); DbSet properties are plural
- Foreign key pattern: `{EntityName}Id`
- Always use `virtual` for navigation properties
- Use `[Key]`, `[Required]`, `[StringLength(n)]`, `[Precision(precision, scale)]` data annotations
- Owned types (e.g. `AccountInformation`) configured with `.OwnsOne()` — no separate table

### Current Entities
- Coach, Manager, Player (with owned AccountInformation), Team, Sponsor, Venue, Tournament
- Junction tables (auto): TeamPlayers, TournamentTeams, TournamentSponsors

### Relationship Template
```csharp
modelBuilder.Entity<Team>()
    .HasOne(t => t.Coach)
    .WithMany()
    .HasForeignKey(t => t.CoachId)
    .IsRequired();
```

## Routing Skill

### Attribute Routing (preferred)
```csharp
[Route("tim")]
public class TeamsController : Controller
{
    [Route("")]
    [Route("pregled")]
    public IActionResult Index() { }

    [Route("detalji/{id:int}")]
    public IActionResult Details(int id) { }
}
```

### Current Route Prefixes
| Controller | Prefix | Language |
|-----------|--------|----------|
| Teams | `/tim` | Croatian |
| Tournaments | `/turnir` | Croatian |
| Players | `/igrac` | Croatian |

Use Croatian for URL slugs: `detalji` (details), `pregled` (overview), `profil` (profile), `sve` (all).

### Route Parameter → Action Parameter names must match exactly.

## List Page Skill
- Query DB asynchronously with EF
- Create per-item view models
- Include links to Details pages
- Use design system: `content-card` and `entity-list` CSS classes
- Show entity count in page title (e.g., "12 Teams found")

## Edit/Form Page Skill
- Use `[HttpGet]` for display, `[HttpPost]` for submission
- Bind models with `[Bind]` attributes
- Redirect to Index or Details after success
- Include `@Html.AntiForgeryToken()` and validation summary
