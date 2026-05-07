---
name: Edit Form Creation Skill
description: Use this skill when you need to create or modify forms for editing/creating entities in the League of Legends Tournament Hosting project.
trigger: "edit form|create form|edit page|form view|update entity|new entity form|POST form"
---

# Edit Form Creation Skill

## Purpose
This skill is invoked when creating or modifying forms for editing/creating entities in the League of Legends Tournament Hosting project. Use it for:
- Creating Edit/Create views (`.cshtml` forms)
- Building form layouts with validation
- Implementing POST handlers
- Adding dropdown/select lists for foreign keys
- Client-side validation
- Server-side validation messages

## When to Use This Skill
- "Create an edit form for Coach model"
- "Add a form to create new Players"
- "Build a form to edit Tournament details"
- "Create a Sponsor registration form"
- "Add validation to Team edit form"
- "Create a form with dropdown for selecting Coach and Manager"

## Important Note
**This project follows the Index + Details pattern only (no Edit/Create pages in current design).** This skill is for:
1. **Future extensions** when Edit/Create functionality is needed
2. **Reference implementation** for how forms should be structured
3. **Optional feature**: You can extend the app with Edit/Create as enhancement

## Workflow

### 1. Create Controller Actions

**GET action** (display empty/prefilled form):
```csharp
[Route("uredi/{id:int}")]
public async Task<IActionResult> Edit(int id)
{
    var entity = await _dbContext.EntityName.FindAsync(id);
    if (entity == null) return NotFound();
    
    var breadcrumbs = new List<BreadcrumbItem>
    {
        new("Home", "/"),
        new("Entities", Url.Action("Index")),
        new($"Edit {entity.Name}")
    };
    ViewData["Breadcrumbs"] = breadcrumbs;
    
    // For dropdowns: Load related entities
    ViewData["CoachOptions"] = await _dbContext.Coaches.ToListAsync();
    
    return View(entity);
}
```

**POST action** (handle form submission):
```csharp
[HttpPost]
[Route("uredi/{id:int}")]
public async Task<IActionResult> Edit(int id, EntityName model)
{
    if (id != model.Id) return BadRequest();
    
    if (!ModelState.IsValid)
    {
        ViewData["CoachOptions"] = await _dbContext.Coaches.ToListAsync();
        return View(model);
    }
    
    try
    {
        _dbContext.Update(model);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id = model.Id });
    }
    catch (DbUpdateException)
    {
        ModelState.AddModelError("", "Unable to save changes. Database error.");
        return View(model);
    }
}
```

### 2. Create the Edit Form View
Location: `Views/{ControllerName}/Edit.cshtml`

Base structure:
```html
@model EntityName

@{
    ViewData["Title"] = "Edit Entity";
}

<div class="container mt-4">
    <h1>@ViewData["Title"]</h1>
    
    <form method="post" class="form">
        <input type="hidden" asp-for="Id" />
        
        <div class="form-group mb-3">
            <label asp-for="Name" class="form-label">Name</label>
            <input type="text" asp-for="Name" class="form-control" required />
            <span asp-validation-for="Name" class="text-danger"></span>
        </div>
        
        <div class="form-group mb-3">
            <label asp-for="CoachId" class="form-label">Coach</label>
            <select asp-for="CoachId" asp-items="ViewData["CoachOptions"] as SelectList" class="form-control">
                <option value="">-- Select Coach --</option>
            </select>
            <span asp-validation-for="CoachId" class="text-danger"></span>
        </div>
        
        <div class="form-group">
            <button type="submit" class="btn btn-primary">Save</button>
            <a href="@Url.Action("Details", new { id = Model.Id })" class="btn btn-secondary">Cancel</a>
        </div>
    </form>
</div>

@section Scripts {
    @await Html.PartialAsync("_ValidationScriptsPartial")
}
```

### 3. Create a Create Form
Similar to Edit, but POST to different action:

**Controller**:
```csharp
[Route("novi")]
public async Task<IActionResult> Create()
{
    var breadcrumbs = new List<BreadcrumbItem>
    {
        new("Home", "/"),
        new("Entities", Url.Action("Index")),
        new("Create New")
    };
    ViewData["Breadcrumbs"] = breadcrumbs;
    ViewData["CoachOptions"] = await _dbContext.Coaches.ToListAsync();
    
    return View();
}

[HttpPost]
[Route("novi")]
public async Task<IActionResult> Create(EntityName model)
{
    if (!ModelState.IsValid)
    {
        ViewData["CoachOptions"] = await _dbContext.Coaches.ToListAsync();
        return View(model);
    }
    
    _dbContext.Add(model);
    await _dbContext.SaveChangesAsync();
    
    return RedirectToAction(nameof(Details), new { id = model.Id });
}
```

**View** (`Views/Entity/Create.cshtml`):
```html
@model EntityName

@{
    ViewData["Title"] = "Create New Entity";
}

<div class="container mt-4">
    <h1>@ViewData["Title"]</h1>
    
    <form method="post" class="form">
        <!-- Same form fields as Edit, but without hidden Id input -->
        <div class="form-group mb-3">
            <label asp-for="Name" class="form-label">Name</label>
            <input type="text" asp-for="Name" class="form-control" required />
            <span asp-validation-for="Name" class="text-danger"></span>
        </div>
        
        <div class="form-group">
            <button type="submit" class="btn btn-primary">Create</button>
            <a href="@Url.Action("Index")" class="btn btn-secondary">Cancel</a>
        </div>
    </form>
</div>

@section Scripts {
    @await Html.PartialAsync("_ValidationScriptsPartial")
}
```

## Form Input Types

### Text Input
```html
<input type="text" asp-for="PropertyName" class="form-control" />
```

### TextArea
```html
<textarea asp-for="PropertyName" class="form-control" rows="4"></textarea>
```

### Dropdown/Select
```html
<select asp-for="PropertyName" asp-items="@ViewData["Options"] as SelectList" class="form-control">
    <option value="">-- Select --</option>
</select>
```

In controller:
```csharp
ViewData["Options"] = new SelectList(await _dbContext.Coaches.ToListAsync(), "Id", "Name");
```

### Date Input
```html
<input type="date" asp-for="HiredAt" class="form-control" />
```

### Number Input
```html
<input type="number" asp-for="YearsOfExperience" class="form-control" min="0" />
```

### Checkbox
```html
<input type="checkbox" asp-for="IsActive" class="form-check-input" />
<label asp-for="IsActive" class="form-check-label">Is Active</label>
```

## Validation

### Client-Side Validation
Always include at end of form view:
```html
@section Scripts {
    @await Html.PartialAsync("_ValidationScriptsPartial")
}
```

### Server-Side Validation
In controller action:
```csharp
if (!ModelState.IsValid)
{
    // Return view with errors shown
    return View(model);
}
```

Display validation errors:
```html
<span asp-validation-for="PropertyName" class="text-danger"></span>
```

### Data Annotations for Validation
In model:
```csharp
[Required(ErrorMessage = "Name is required")]
[StringLength(100, MinimumLength = 3)]
public string Name { get; set; }

[Range(1, 50, ErrorMessage = "Experience must be 1-50 years")]
public int YearsOfExperience { get; set; }
```

## Best Practices

1. **Always Include Hidden Id Field** (for Edit):
   ```html
   <input type="hidden" asp-for="Id" />
   ```

2. **Breadcrumbs**: Always add to forms
   ```csharp
   var breadcrumbs = new List<BreadcrumbItem>
   {
       new("Home", "/"),
       new("List", Url.Action("Index")),
       new("Edit")
   };
   ViewData["Breadcrumbs"] = breadcrumbs;
   ```

3. **Load Related Entities**: For foreign keys
   ```csharp
   ViewData["CoachOptions"] = await _dbContext.Coaches.ToListAsync();
   ```

4. **Responsive Layout**: Use Bootstrap grid
   ```html
   <div class="col-md-6 col-lg-4">
       <!-- Form content -->
   </div>
   ```

5. **Cancel Button**: Always provide way to exit without saving
   ```html
   <a href="@Url.Action("Details", new { id = Model.Id })" class="btn btn-secondary">Cancel</a>
   ```

6. **Error Messages**: Display custom messages
   ```html
   @if (!ModelState.IsValid)
   {
       <div class="alert alert-danger">
           Please correct the errors below.
       </div>
   }
   ```

7. **Async/Await**: Always use async database calls
   ```csharp
   var entity = await _dbContext.Entity.FindAsync(id);
   ```

## Common Tasks

### Add Edit Page
1. Create `Edit` GET and POST actions
2. Create `Views/Entity/Edit.cshtml`
3. Add form with all entity properties
4. Include validation error display
5. Add breadcrumbs

### Add Create Page
1. Create `Create` GET and POST actions
2. Create `Views/Entity/Create.cshtml`
3. Same form as Edit (without Id hidden field)
4. Redirect to Details/Index on success

### Add Dropdown for Foreign Key
1. Load related entities in controller: `ViewData["Options"] = new SelectList(...)`
2. In view use: `<select asp-for="ForeignKeyId" asp-items="...">`
3. Handle validation in ModelState

### Add Custom Validation
1. Use `[CustomValidation(...)]` attribute or
2. Check in POST action: `if (model.StartDate > model.EndDate) ModelState.AddModelError(...)`
3. Display with `<span asp-validation-for="PropertyName">`

## File Locations
- Entity models: `Models/{EntityName}.cs`
- Views: `Views/{ControllerName}/Edit.cshtml` or `Create.cshtml`
- Controllers: `Controllers/{ControllerName}Controller.cs`
- Validation scripts: `Views/Shared/_ValidationScriptsPartial.cshtml`
