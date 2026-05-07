---
name: edit-form-page
description: Creates or modifies Create/Edit form pages for entities, including input validation and submission handling.
trigger: "create form|edit form|form page|POST action|input validation|form submission"
---

# Edit Form Page Skill

## Purpose
This skill is invoked when the user needs to create or modify Create/Edit form pages for entities in the ASP.NET Core MVC application. It handles the full workflow of adding form views, controller actions, validation, and submission handling.

## When to Use This Skill
- "Create a new Create form for Players"
- "Add an Edit page for Teams"
- "Implement form validation for Sponsor registration"
- "Add POST handler for form submission"
- "Create a form with dropdown lists for foreign keys"
- "Implement client-side and server-side validation"

## Workflow

### 1. Create Controller Actions

**GET Action** (Display the form):
```csharp
[HttpGet]
[Route("create")]  // or [Route("edit/{id:int}")]
public async Task<IActionResult> Create()  // or Edit(int id)
{
    // For edit: load existing entity
    // var entity = await _dbContext.Entity.FindAsync(id);
    
    // Load data for dropdowns
    ViewData["RelatedEntities"] = await _dbContext.RelatedEntities.ToListAsync();
    
    // Set breadcrumbs
    var breadcrumbs = new List<BreadcrumbItem>
    {
        new("Home", "/"),
        new("Entities", Url.Action("Index")),
        new("Create Entity")  // or "Edit Entity"
    };
    ViewData["Breadcrumbs"] = breadcrumbs;
    
    return View(/* entity or new Entity() */);
}
```

**POST Action** (Handle form submission):
```csharp
[HttpPost]
[Route("create")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(Entity model)
{
    if (!ModelState.IsValid)
    {
        // Reload dropdown data
        ViewData["RelatedEntities"] = await _dbContext.RelatedEntities.ToListAsync();
        return View(model);
    }
    
    try
    {
        _dbContext.Add(model);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    catch (Exception)
    {
        ModelState.AddModelError("", "Unable to save changes.");
        return View(model);
    }
}
```

### 2. Create the Form View

Location: `Views/{Controller}/Create.cshtml` or `Edit.cshtml`

```html
@model Entity

@{
    ViewData["Title"] = "Create Entity";
}

<div class="container mt-4">
    <h1>@ViewData["Title"]</h1>
    
    <!-- Breadcrumbs -->
    @if (ViewData["Breadcrumbs"] is List<BreadcrumbItem> breadcrumbs)
    {
        <nav aria-label="breadcrumb">
            <ol class="breadcrumb">
                @foreach (var crumb in breadcrumbs)
                {
                    <li class="breadcrumb-item">
                        <a href="@crumb.Url">@crumb.Text</a>
                    </li>
                }
            </ol>
        </nav>
    }
    
    <div class="row">
        <div class="col-md-8">
            <form asp-action="Create" method="post">
                <div asp-validation-summary="ModelOnly" class="text-danger"></div>
                
                <!-- Form fields -->
                <div class="mb-3">
                    <label asp-for="Name" class="form-label"></label>
                    <input asp-for="Name" class="form-control" />
                    <span asp-validation-for="Name" class="text-danger"></span>
                </div>
                
                <!-- Dropdown example -->
                <div class="mb-3">
                    <label asp-for="RelatedEntityId" class="form-label"></label>
                    <select asp-for="RelatedEntityId" asp-items="ViewBag.RelatedEntities" class="form-control">
                        <option value="">-- Select --</option>
                    </select>
                    <span asp-validation-for="RelatedEntityId" class="text-danger"></span>
                </div>
                
                <div class="mb-3">
                    <input type="submit" value="Create" class="btn btn-primary" />
                    <a asp-action="Index" class="btn btn-secondary">Cancel</a>
                </div>
            </form>
        </div>
    </div>
</div>

@section Scripts {
    @{await Html.RenderPartialAsync("_ValidationScriptsPartial");}
}
```

### 3. Update Model Validation

Ensure the model has appropriate validation attributes:
```csharp
public class Entity
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; }
    
    [Required]
    [Display(Name = "Related Entity")]
    public int RelatedEntityId { get; set; }
    
    [ForeignKey("RelatedEntityId")]
    public virtual RelatedEntity RelatedEntity { get; set; }
}
```

### 4. Test the Form

- Submit valid data and verify creation/update
- Submit invalid data and check validation messages
- Test dropdown population
- Verify redirects after success
- Check breadcrumbs navigation