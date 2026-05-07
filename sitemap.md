# Sitemap - Routing and URL Structure

## Overview
This document provides a comprehensive map of all available URLs in the League of Legends Tournament Hosting application, including the controller actions and views used.

## URL Routes

### Home Routes

| URL | Controller | Action | View | Route Type | Description |
|-----|-----------|--------|------|-----------|-------------|
| `/` | Home | Index | Home/Index | Default | Application home page / dashboard |
| `/Home/Index` | Home | Index | Home/Index | Default | Explicit home route |
| `/Privacy` | Home | Privacy | Home/Privacy | Default | Privacy policy page |
| `/Home/Privacy` | Home | Privacy | Home/Privacy | Default | Explicit privacy route |

### Team Routes (Custom Routes)

| URL | Controller | Action | View | Route Type | Description |
|-----|-----------|--------|------|-----------|-------------|
| `/tim` | Teams | Index | Teams/Index | Custom `[Route("")]` | Teams list with default route |
| `/tim/pregled` | Teams | Index | Teams/Index | Custom `[Route("pregled")]` | Teams overview page |
| `/tim/sve` | Teams | Index | Teams/Index | Custom `[Route("sve")]` | All teams view |
| `/tim/detalji/5` | Teams | Details | Teams/Details | Custom `[Route("detalji/{id:int}")]` | Team details by ID (Croatian: detalji) |
| `/tim/profil/5` | Teams | Details | Teams/Details | Custom `[Route("profil/{id:int}")]` | Team profile view (Croatian: profil) |

### Tournament Routes (Custom Routes)

| URL | Controller | Action | View | Route Type | Description |
|-----|-----------|--------|------|-----------|-------------|
| `/turnir` | Tournaments | Index | Tournaments/Index | Custom `[Route("")]` | Tournaments list with default route |
| `/turnir/pregled` | Tournaments | Index | Tournaments/Index | Custom `[Route("pregled")]` | Tournaments overview |
| `/turnir/lista` | Tournaments | Index | Tournaments/Index | Custom `[Route("lista")]` | Tournaments list view (Croatian: lista) |
| `/Tournaments/Details/3` | Tournaments | Details | Tournaments/Details | Default | Tournament details (fallback route) |

### Player Routes (Custom Routes)

| URL | Controller | Action | View | Route Type | Description |
|-----|-----------|--------|------|-----------|-------------|
| `/igrac` | Players | Index | Players/Index | Custom `[Route("")]` | Players list with default route |
| `/igrac/pregled` | Players | Index | Players/Index | Custom `[Route("pregled")]` | Players overview |
| `/igrac/red` | Players | Index | Players/Index | Custom `[Route("red")]` | Players roster (Croatian: red) |
| `/igrac/detalji/1` | Players | Details | Players/Details | Custom `[Route("detalji/{id:int}")]` | Player details by ID (Croatian: detalji) |
| `/igrac/profil/1` | Players | Details | Players/Details | Custom `[Route("profil/{id:int}")]` | Player profile view (Croatian: profil) |
| `/igrac/kreiraj` | Players | Create | Players/Create | Custom `[Route("kreiraj")]` | Create new player form (GET) |
| `/igrac/uredi/1` | Players | Edit | Players/Edit | Custom `[Route("uredi/{id:int}")]` | Edit player form (GET) |

### Coach Routes (Custom Routes)

| URL | Controller | Action | View | Route Type | Description |
|-----|-----------|--------|------|-----------|-------------|
| `/trener` | Coaches | Index | Coaches/Index | Custom `[Route("")]` | Coaches list with default route |
| `/trener/pregled` | Coaches | Index | Coaches/Index | Custom `[Route("pregled")]` | Coaches overview page |
| `/trener/sve` | Coaches | Index | Coaches/Index | Custom `[Route("sve")]` | All coaches view |
| `/trener/detalji/2` | Coaches | Details | Coaches/Details | Custom `[Route("detalji/{id:int}")]` | Coach details by ID (Croatian: detalji) |
| `/trener/profil/2` | Coaches | Details | Coaches/Details | Custom `[Route("profil/{id:int}")]` | Coach profile view (Croatian: profil) |

### Manager Routes (Custom Routes)

| URL | Controller | Action | View | Route Type | Description |
|-----|-----------|--------|------|-----------|-------------|
| `/menadzer` | Managers | Index | Managers/Index | Custom `[Route("")]` | Managers list with default route |
| `/menadzer/pregled` | Managers | Index | Managers/Index | Custom `[Route("pregled")]` | Managers overview page |
| `/menadzer/sve` | Managers | Index | Managers/Index | Custom `[Route("sve")]` | All managers view |
| `/menadzer/detalji/2` | Managers | Details | Managers/Details | Custom `[Route("detalji/{id:int}")]` | Manager details by ID (Croatian: detalji) |
| `/menadzer/profil/2` | Managers | Details | Managers/Details | Custom `[Route("profil/{id:int}")]` | Manager profile view (Croatian: profil) |

### Sponsor Routes (Custom Routes)

| URL | Controller | Action | View | Route Type | Description |
|-----|-----------|--------|------|-----------|-------------|
| `/sponzor` | Sponsors | Index | Sponsors/Index | Custom `[Route("")]` | Sponsors list with default route |
| `/sponzor/pregled` | Sponsors | Index | Sponsors/Index | Custom `[Route("pregled")]` | Sponsors overview page |
| `/sponzor/sve` | Sponsors | Index | Sponsors/Index | Custom `[Route("sve")]` | All sponsors view |
| `/sponzor/detalji/1` | Sponsors | Details | Sponsors/Details | Custom `[Route("detalji/{id:int}")]` | Sponsor details by ID (Croatian: detalji) |
| `/sponzor/info/1` | Sponsors | Details | Sponsors/Details | Custom `[Route("info/{id:int}")]` | Sponsor info view (Croatian: info) |

### Venue Routes (Custom Routes)

| URL | Controller | Action | View | Route Type | Description |
|-----|-----------|--------|------|-----------|-------------|
| `/mjesto` | Venues | Index | Venues/Index | Custom `[Route("")]` | Venues list with default route |
| `/mjesto/pregled` | Venues | Index | Venues/Index | Custom `[Route("pregled")]` | Venues overview page |
| `/mjesto/sve` | Venues | Index | Venues/Index | Custom `[Route("sve")]` | All venues view |
| `/mjesto/detalji/1` | Venues | Details | Venues/Details | Custom `[Route("detalji/{id:int}")]` | Venue details by ID (Croatian: detalji) |
| `/mjesto/info/1` | Venues | Details | Venues/Details | Custom `[Route("info/{id:int}")]` | Venue info view (Croatian: info) |

## Route Mapping Details

### Custom Route Prefixes

1. **Teams Controller**: `[Route("tim")]`
   - Base: `/tim`
   - Index Actions: `[Route("")]`, `[Route("pregled")]`, `[Route("sve")]`
   - Details Actions: `[Route("detalji/{id:int}")]`, `[Route("profil/{id:int}")]`

2. **Tournaments Controller**: `[Route("turnir")]`
   - Base: `/turnir`
   - Index Actions: `[Route("")]`, `[Route("pregled")]`, `[Route("lista")]`
   - Details Action: Uses default convention or explicit route

3. **Players Controller**: `[Route("igrac")]`
   - Base: `/igrac`
   - Index Actions: `[Route("")]`, `[Route("pregled")]`, `[Route("red")]`
   - Details Actions: `[Route("detalji/{id:int}")]`, `[Route("profil/{id:int}")]`

4. **Coaches Controller**: `[Route("trener")]`
   - Base: `/trener`
   - Index Actions: `[Route("")]`, `[Route("pregled")]`, `[Route("sve")]`
   - Details Actions: `[Route("detalji/{id:int}")]`, `[Route("profil/{id:int}")]`

5. **Managers Controller**: `[Route("menadzer")]`
   - Base: `/menadzer`
   - Index Actions: `[Route("")]`, `[Route("pregled")]`, `[Route("sve")]`
   - Details Actions: `[Route("detalji/{id:int}")]`, `[Route("profil/{id:int}")]`

6. **Sponsors Controller**: `[Route("sponzor")]`
   - Base: `/sponzor`
   - Index Actions: `[Route("")]`, `[Route("pregled")]`, `[Route("sve")]`
   - Details Actions: `[Route("detalji/{id:int}")]`, `[Route("info/{id:int}")]`

7. **Venues Controller**: `[Route("mjesto")]`
   - Base: `/mjesto`
   - Index Actions: `[Route("")]`, `[Route("pregled")]`, `[Route("sve")]`
   - Details Actions: `[Route("detalji/{id:int}")]`, `[Route("info/{id:int}")]`

### Default Route Pattern

All controllers now use custom attribute routing. The default pattern is no longer used:
```
/{controller}/{action}/{id?}
```

**Note**: Prior to this update, Coaches, Managers, Sponsors, and Venues used default convention routing. They have been migrated to custom routing and EF Core for consistency.

## Shared Views

### Layout
- **View**: `Shared/_Layout.cshtml`
- **Used by**: All pages
- **Components**:
  - Sidebar navigation
  - Top navigation bar
  - Breadcrumb trail
  - Footer

### Partial Views
- **`Shared/Components/_Sidebar`**: Navigation sidebar (used in layout)
- **`Shared/Components/_Breadcrumb`**: Breadcrumb navigation (used in layout)
- **`Shared/_ValidationScriptsPartial`**: Client-side validation scripts
- **`Shared/Error`**: Error page (404, 500, etc.)

## Query String Parameters

### Filtering & Pagination
- **None implemented**: Current implementation shows all entities without pagination or filtering

## Error Handling Routes

| Status | Route | View | Description |
|--------|-------|------|-------------|
| 404 | `/error/notfound` or automatic | Shared/Error | Not found errors |
| 500 | `/Home/Error` | Shared/Error | Server errors |

## Route Constraints

### Integer Constraints
- `{id:int}` - Ensures ID parameter is a valid integer
- Applied to: Teams/Details, Players/Details, Tournaments/Details (implicit)

### String Pattern Constraints
- None currently specified (possible enhancement: `{name:alpha}` for filters)

## Navigation Links Structure

### Home Page Links
- Teams: `/tim` (custom route)
- Tournaments: `/turnir` (custom route)
- Players: `/igrac` (custom route)

### Cross-Navigation
- Team cards link to `/tim/detalji/{id}` or `/tim/profil/{id}`
- Player cards link to `/igrac/detalji/{id}` or `/igrac/profil/{id}`
- Tournament cards link to related tournaments and teams

### Breadcrumb Navigation
1. **Home** → `/` (Home/Index)
2. **Teams** → `/tim` (Teams/Index)
3. **Team Name** → `/tim/detalji/{id}` (Teams/Details)

Example breadcrumb for team detail:
```
Home > Teams > Team Alpha
```

## API Considerations (Future)

The current implementation is MVC-based. For future RESTful API endpoints, consider:
- `/api/teams`
- `/api/players/{id}`
- `/api/tournaments`
- `/api/sponsors`

## Notes

1. **Custom Route Priority**: Custom routes (Teams, Tournaments, Players) take precedence over default convention routes
2. **Async Actions**: Index and Details actions are now async (`async Task<IActionResult>`)
3. **Database-First**: Routes now integrate with EF Core DbContext instead of static MockRepository
4. **Localization**: Croatian route names used in some custom routes (tim, turnir, igrac, pregled, red, detalji, profil)
