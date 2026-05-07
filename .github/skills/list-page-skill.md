---
name: List Page Creation Skill
description: Use this skill when you need to create a new list/index page for displaying entities in the League of Legends Tournament Hosting project.
trigger: "list page|index view|display all|create list|view entities|show all items"
---

# List Page Creation Skill

## Purpose
This skill is invoked when creating Index/list pages to display collections of entities in the League of Legends Tournament Hosting project. Use it for:
- Creating new list/index views (`.cshtml`)
- Implementing entity card/grid layouts
- Adding filtering and sorting
- Implementing pagination
- Creating breadcrumb navigation
- Adding links to detail pages

## When to Use This Skill
- "Create a list page for Coaches"
- "Add a new index view that displays all Sponsors"
- "Build a list page with cards for Players"
- "Create a searchable list of Teams"
- "Add pagination to the Venues list"
- "Make a grid display for Tournaments with filters"

## Workflow

### 1. Create Controller Action
Add an `Index` action in the controller that:
- Fetches all entities from DbContext
- Maps to ViewModels if needed
- Passes to the view

```csharp
[Route("")]
[Route("pregled")]
public async Task<IActionResult> Index()
{
    var entities = await _dbContext.EntityName
        .AsNoTracking()
        .ToListAsync();
    
    var viewModel = entities
        .Select(e => new EntityCardViewModel(
            title: e.Name,
            subtitle: e.Subtitle ?? "N/A",
            body: e.Description ?? "",
            linkText: "View Details",
            linkUrl: Url.Action("Details", new { id = e.Id })
        ))
        .ToList();
    
    return View(viewModel);
}
```

### 2. Create the Index View
Location: `Views/{ControllerName}/Index.cshtml`

Base structure:
```html
@model IEnumerable<EntityCardViewModel>

@{
    ViewData["Title"] = "Entity List";
    var breadcrumbs = new List<BreadcrumbItem>
    {
        new("Home", "/"),
        new("Entities")
    };
    ViewData["Breadcrumbs"] = breadcrumbs;
}

<div class="container mt-4">
    <h1>@ViewData["Title"]</h1>
    
    <div class="row">
        @foreach (var item in Model)
        {
            <div class="col-md-4 mb-3">
                <partial name="Components/_Card" model="item" />
            </div>
        }
    </div>
</div>
```

### 3. Use the Card Component
The `_Card.cshtml` partial is already available in `Views/Shared/Components/_Card.cshtml`.
It accepts `EntityCardViewModel` and renders a Bootstrap card.

```html
<partial name="Components/_Card" model="viewModelItem" />
```

### 4. (Optional) Add Filtering/Search
```csharp
public async Task<IActionResult> Index(string search)
{
    var query = _dbContext.EntityName.AsQueryable();
    
    if (!string.IsNullOrWhiteSpace(search))
    {
        query = query.Where(e => e.Name.Contains(search) || e.Description.Contains(search));
    }
    
    var entities = await query.ToListAsync();
    // ... rest of action
}
```

View form:
```html
<form method="get" class="mb-3">
    <div class="input-group">
        <input type="text" name="search" class="form-control" placeholder="Search..." value="@Context.Request.Query["search"]">
        <button class="btn btn-primary" type="submit">Search</button>
    </div>
</form>
```

### 5. (Optional) Add Pagination
Use `PagedList` NuGet package or simple offset/limit pattern:

```csharp
const int pageSize = 12;
int page = int.TryParse(pageRequest, out int p) ? p : 1;
var total = await query.CountAsync();
var items = await query
    .Skip((page - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync();

// Pass paging info to view
ViewData["CurrentPage"] = page;
ViewData["TotalPages"] = (int)Math.Ceiling((double)total / pageSize);
```

## Best Practices

1. **ViewModels**: Always map entities to ViewModels for cleaner views
   - Use `EntityCardViewModel` for card layouts
   - Include only necessary fields for display

2. **Breadcrumbs**: Always add breadcrumb navigation
   ```csharp
   var breadcrumbs = new List<BreadcrumbItem>
   {
       new("Home", "/"),
       new("Entities")
   };
   ViewData["Breadcrumbs"] = breadcrumbs;
   ```

3. **Async/Await**: Always use async database calls
   - `await _dbContext.Entity.ToListAsync()`
   - `AsNoTracking()` for read-only operations

4. **Naming**: 
   - View file: `Views/{Controller}/Index.cshtml`
   - Route: `[Route("")]` and `[Route("pregled")]` for list views

5. **Responsive Design**:
   - Use Bootstrap grid: `col-md-4 col-lg-3` for card layouts
   - Ensure mobile-friendly display

6. **Empty State**: Handle when no entities exist
   ```html
   @if (Model.Count() == 0)
   {
       <div class="alert alert-info">
           No entities found.
       </div>
   }
   ```

## Common Tasks

### Display All Entities as Cards
1. Create controller action with entity list
2. Map to `EntityCardViewModel` collection
3. Create `Index.cshtml` with card grid
4. Use `_Card` partial in loop
5. Set breadcrumbs in `ViewData["Breadcrumbs"]`

### Add Search/Filter
1. Add `search` parameter to Index action
2. Filter DbSet based on parameter
3. Add form with search box above grid
4. Preserve search in query string

### Add Pagination
1. Calculate total count and pages
2. Use `Skip()` and `Take()` on query
3. Pass page info via ViewData
4. Add pagination controls below grid

## Example: Complete List Page for Coaches

**Controller Action**:
```csharp
[Route("treneri")]
public class CoachesController : Controller
{
    private readonly TournamentDbContext _dbContext;
    
    [Route("")]
    [Route("pregled")]
    public async Task<IActionResult> Index()
    {
        var coaches = await _dbContext.Coaches
            .AsNoTracking()
            .ToListAsync();
        
        var breadcrumbs = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Coaches")
        };
        ViewData["Breadcrumbs"] = breadcrumbs;
        
        var viewModel = coaches
            .Select(c => new EntityCardViewModel(
                title: c.Name,
                subtitle: c.GamerTag,
                body: $"Experience: {c.YearsOfExperience} years\nHired: {c.HiredAt:yyyy-MM-dd}",
                linkText: "View Profile",
                linkUrl: Url.Action("Details", new { id = c.Id })
            ))
            .ToList();
        
        return View(viewModel);
    }
}
```

**View** (`Views/Coaches/Index.cshtml`):
```html
@model IEnumerable<EntityCardViewModel>

@{
    ViewData["Title"] = "Coaches";
}

<div class="container mt-4">
    <h1>@ViewData["Title"]</h1>
    
    <div class="row">
        @foreach (var coach in Model)
        {
            <div class="col-md-4 mb-3">
                <partial name="Components/_Card" model="coach" />
            </div>
        }
    </div>
</div>
```

## File Locations
- Entity models: `Models/{EntityName}.cs`
- Views: `Views/{ControllerName}/Index.cshtml`
- View models: `ViewModels/{ViewModelName}.cs`
- Components: `Views/Shared/Components/`
