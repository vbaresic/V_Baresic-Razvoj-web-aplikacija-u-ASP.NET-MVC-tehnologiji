---
name: Routing Configuration Skill
description: Use this skill when you need to configure custom URL routes, attribute-based routing, or route constraints in the League of Legends Tournament Hosting project.
trigger: "routing|route|URL pattern|custom route|attribute routing|MapControllerRoute"
---

# Routing Configuration Skill

## Purpose
This skill is invoked when working with ASP.NET Core routing in the League of Legends Tournament Hosting project. Use it for:
- Adding custom URL routes using `[Route]` attributes
- Configuring route constraints (`:int`, `:alpha`, `:minlength`)
- Creating friendly URLs for controllers and actions
- Managing route conflicts and precedence
- Implementing SEO-friendly URL structures

## When to Use This Skill
- "Create a custom route for the Teams list at `/tim`"
- "Add a route constraint to ensure ID is an integer"
- "Change the Tournaments URL to `/turnir`"
- "Create multiple route options for the same action"
- "Set up a catch-all route pattern"
- "Add a custom route for coaches at `/trener`"

## Routing Approaches

### 1. Attribute-Based Routing (Recommended)
Applied directly to controller and action methods using `[Route]` attributes.

**Advantages**:
- Clear and explicit
- Collocated with action logic
- Easier to manage and trace
- Better for MVC/complex scenarios

**Example**:
```csharp
[Route("tim")]
public class TeamsController : Controller
{
    [Route("")]                           // /tim
    [Route("pregled")]                    // /tim/pregled
    [Route("sve")]                        // /tim/sve
    public async Task<IActionResult> Index() { }

    [Route("detalji/{id:int}")]           // /tim/detalji/5
    [Route("profil/{id:int}")]            // /tim/profil/5
    public async Task<IActionResult> Details(int id) { }
}
```

### 2. Convention-Based Routing
Default pattern: `/{controller}/{action}/{id?}`

**Example**:
- `/Coaches/Index`
- `/Sponsors/Details/1`

## Current Routes in Application

### Teams Routes
```
[Route("tim")]
- /tim                    → Index
- /tim/pregled            → Index
- /tim/sve                → Index
- /tim/detalji/{id}       → Details
- /tim/profil/{id}        → Details
```

### Tournaments Routes
```
[Route("turnir")]
- /turnir                 → Index
- /turnir/pregled         → Index
- /turnir/lista           → Index
```

### Players Routes
```
[Route("igrac")]
- /igrac                  → Index
- /igrac/pregled          → Index
- /igrac/red              → Index
- /igrac/detalji/{id}     → Details
- /igrac/profil/{id}      → Details
```

## Creating Custom Routes

### Step 1: Add Class-Level Route
```csharp
[Route("tim")]  // Controller base route
public class TeamsController : Controller
{
    // All actions inherit this prefix
}
```

### Step 2: Add Action-Level Routes
```csharp
[Route("")]           // Maps to /tim
public IActionResult Index() { }

[Route("detalji/{id:int}")]  // Maps to /tim/detalji/5
public IActionResult Details(int id) { }
```

### Step 3: Multiple Routes for Same Action
Multiple `[Route]` attributes on one action enable multiple URL patterns:

```csharp
[Route("")]           // /tim
[Route("sve")]        // /tim/sve
[Route("pregled")]    // /tim/pregled
public IActionResult Index() { }
```

## Route Constraints

### Common Constraints
| Constraint | Format | Example | Match |
|-----------|--------|---------|-------|
| int | `{id:int}` | `/5` | Integers only |
| alpha | `{name:alpha}` | `/john` | Letters only |
| float | `{price:float}` | `/19.99` | Decimal numbers |
| minlength | `{name:minlength(3)}` | `/abc` | Minimum 3 chars |
| maxlength | `{name:maxlength(10)}` | `/abcdefghij` | Maximum 10 chars |
| length | `{name:length(5)}` | `/abcde` | Exactly 5 chars |
| regex | `{id:regex(^\d{3}$)}` | `/123` | Matches regex |

### Examples

#### Integer Constraint
```csharp
[Route("detalji/{id:int}")]
public IActionResult Details(int id) { }
// Matches: /tim/detalji/5
// Not: /tim/detalji/abc
```

#### Multiple Constraints
```csharp
[Route("profil/{username:alpha:minlength(3)}")]
public IActionResult Profile(string username) { }
// Matches: /profil/john
// Not: /profil/jn (too short)
// Not: /profil/john123 (contains numbers)
```

#### Regex Constraint
```csharp
[Route("turnir/{year:regex(^\\d{4}$)}")]
public IActionResult ByYear(int year) { }
// Matches: /turnir/2025
// Not: /turnir/25
```

## Route Parameter Binding

### Route Parameter Names Must Match Action Parameters
```csharp
[Route("tim/{teamId:int}")]
public IActionResult Details(int teamId) { }  // ✓ Correct

[Route("tim/{id:int}")]
public IActionResult Details(int teamId) { }  // ✗ Won't bind properly
```

## Best Practices

### 1. Use Descriptive Route Names
- ✓ `/tim/detalji/5` (clear: team details)
- ✗ `/t/d/5` (cryptic)

### 2. Use SEO-Friendly URLs
- ✓ `/turnir/finale-2025` (descriptive)
- ✗ `/Tournaments/Details?id=3` (query string)

### 3. Implement Logical Hierarchy
```csharp
[Route("tim")]
public class TeamsController
{
    [Route("")]           // /tim
    [Route("sve")]        // /tim/sve
    public IActionResult Index() { }

    [Route("detalji/{id:int}")]  // /tim/detalji/5
    public IActionResult Details(int id) { }

    [Route("treneri")]           // /tim/treneri
    public IActionResult Coaches() { }
}
```

### 4. Avoid Route Conflicts
```csharp
// ✓ Good: Different patterns don't conflict
[Route("")]
[Route("sve")]
[Route("pregled")]
public IActionResult Index() { }

// ✗ Bad: Ambiguous which action matches /tim/5?
[Route("{id:int}")]
public IActionResult Details(int id) { }

[Route("")]
public IActionResult Index() { }
```

### 5. Use Consistent Naming
- Use same language throughout (English or Croatian)
- Current app uses Croatian: `tim`, `turnir`, `igrac`, `pregled`, `detalji`
- Document naming conventions

## Advanced Scenarios

### Optional Route Segments
```csharp
[Route("tim/{id:int?}")]
public IActionResult Index(int? id = null) { }
// Matches: /tim and /tim/5
```

### Catch-All Route
```csharp
[Route("tim/{*path}")]
public IActionResult Catch(string path) { }
// Matches: /tim/anything/in/here
```

### Duplicate Route Prevention
```csharp
// Order matters for specificity
[Route("detalji/{id:int}")]
public IActionResult Details(int id) { }

[Route("")]
[Route("sve")]
public IActionResult Index() { }
```

## Generating URLs in Views

### Using Action Name
```html
<a asp-action="Details" asp-route-id="5">Team 5</a>
```

### Using Named Routes (if needed)
```csharp
[Route("detalji/{id:int}", Name = "TeamDetails")]
public IActionResult Details(int id) { }
```

```html
<a asp-route="TeamDetails" asp-route-id="5">Team 5</a>
```

## Testing Routes

### Check Available Routes
Use Routing Debugger Middleware (for development):

```csharp
// In Program.cs for development
if (app.Environment.IsDevelopment())
{
    app.UseRouting();
    // Routes now debuggable
}
```

### Test URLs
Manual testing:
- Navigate to `/tim` → should hit Teams/Index
- Navigate to `/tim/detalji/1` → should hit Teams/Details
- Navigate to `/tim/detalji/abc` → should return 404 (int constraint failed)

## Common Pitfalls

### 1. Forgetting [Route] on Controller
```csharp
// ✗ Routes won't be prefixed
public class TeamsController { }

// ✓ Correct
[Route("tim")]
public class TeamsController { }
```

### 2. Route Parameter Mismatch
```csharp
[Route("detalji/{id:int}")]
public IActionResult Details(string id) { }  // ✗ Won't work correctly

[Route("detalji/{id:int}")]
public IActionResult Details(int id) { }     // ✓ Correct
```

### 3. Ambiguous Route Patterns
```csharp
[Route("{controller}/{action}/{id?}")]  // ✗ Too generic, conflicts likely

[Route("tim")]                           // ✓ Specific prefixes avoid conflicts
public class TeamsController { }
```

## Migration Path

### From Convention to Attribute Routing

Before (Convention):
```csharp
public class TeamsController
{
    public IActionResult Index() { }      // /Teams/Index
    public IActionResult Details(int id) { }  // /Teams/Details/5
}
```

After (Attribute Routing):
```csharp
[Route("tim")]
public class TeamsController
{
    [Route("")]
    public IActionResult Index() { }      // /tim
    
    [Route("detalji/{id:int}")]
    public IActionResult Details(int id) { }  // /tim/detalji/5
}
```

## Documentation References

- Current Routes: See `sitemap.md` for complete URL map
- Controller Implementations: `Controllers/TeamsController.cs`, etc.
- Program.cs: Default route configuration at application startup

## Adding New Routes

### Example: Add Coaches Route
```csharp
[Route("trener")]  // trener = coach in Croatian
public class CoachesController : Controller
{
    [Route("")]
    [Route("sve")]
    public IActionResult Index() { }

    [Route("profil/{id:int}")]
    public IActionResult Details(int id) { }
}
```

This would create:
- `/trener` → All coaches
- `/trener/sve` → All coaches (alternative)
- `/trener/profil/2` → Coach details
