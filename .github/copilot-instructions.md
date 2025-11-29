# HomeLibrary Solution - AI Assistant Instructions

## Solution Overview

HomeLibrary is a personal library management application built with ASP.NET Core MVC (.NET 8) that allows users to track their book collections, wishlists, and reading progress.

### Architecture Pattern
- **Clean Architecture** with separation of concerns
- **Repository Pattern** via Entity Framework Core
- **Manager Pattern** for business logic layer
- **DTO Pattern** for data transfer between layers

### Project Structure

```
HomeLibrary/
??? HomeLibrary.Core/              # Domain models, interfaces, DTOs, records
??? HomeLibrary.DataAccess/        # EF Core DbContext and data access
??? HomeLibrary.BusinessLogic/     # Business logic managers
??? HomeLibrary.Web.Mvc/           # ASP.NET Core MVC presentation layer
??? HomeLibrary.BusinessLogic.Tests/ # Unit tests
```

## Technology Stack

### Backend
- **.NET 8** - Target framework
- **ASP.NET Core MVC** - Web framework (NOT Razor Pages or Blazor)
- **Entity Framework Core 9.0.9** - ORM with SQL Server provider
- **C# 12.0** - Language version
- **Nullable Reference Types** - Enabled across all projects

### Frontend
- **TypeScript 5.3.3** - For type-safe JavaScript
- **ES2020** - Target JavaScript version
- **Custom CSS** - Mobile-first, no Bootstrap framework dependency
- **Bootstrap Icons 1.11.3** - Icon library (CDN)
- **Axios** - For REST API calls (future use)
- **No JavaScript frameworks** - Vanilla TypeScript/JavaScript only

### Database
- **SQL Server** - Primary database
- **Code-First** approach with EF Core

## Coding Standards & Conventions

### C# Conventions

#### Namespace & Using Statements
```csharp
// Use file-scoped namespaces (C# 10+)
namespace HomeLibrary.Core.Models;

// Implicit usings enabled - no need for common System namespaces
```

#### Nullable Reference Types
```csharp
// Always use nullable reference types appropriately
public string Name { get; set; } = null!;  // Non-nullable with null-forgiving operator
public string? Description { get; set; }   // Nullable property
```

#### Primary Constructors (C# 12)
```csharp
// Use primary constructors for dependency injection
public class AuthorManager(HomeLibrarySqlContext context) : IAuthorManager
{
    // Direct parameter usage - no need for private fields
}
```

#### Async/Await
```csharp
// Always use async for database operations
public async Task<IEnumerable<AuthorDto>> GetAuthorsAsync()
{
    return await context.Authors
        .AsNoTracking()
        .ToListAsync();
}
```

### Layer Responsibilities

#### 1. HomeLibrary.Core (Domain Layer)
**Purpose**: Contains domain models, interfaces, DTOs, and records. No dependencies on other projects.

**Contents**:
- `/Models/` - Entity Framework entities with data annotations
- `/Interfaces/` - Interface definitions (e.g., `IBookManager`, `IAuthorManager`)
- `/DataTransferObjects/` - DTOs for data transfer (e.g., `AuthorDto`, `GenreDto`)
- `/Records/` - Immutable record types (e.g., `BookStatuses`)

**Conventions**:
```csharp
// Entity models
public class Book
{
    [Key]
    public int Id { get; set; }
    
    public string Title { get; set; } = null!;
    
    [ForeignKey("AuthorId")]
    [InverseProperty("Books")]
    public virtual Author? Author { get; set; }
}

// DTOs - simple POCOs
public class AuthorDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

// Records - immutable constants
public record BookStatuses(int Id, string Name)
{
    public static BookStatuses Owned { get; } = new BookStatuses(1, "Owned");
    public static BookStatuses Wishlist { get; } = new BookStatuses(2, "Wishlist");
}
```

#### 2. HomeLibrary.DataAccess (Data Layer)
**Purpose**: Database context and EF Core configuration.

**Dependencies**: `HomeLibrary.Core`

**Conventions**:
```csharp
// DbContext with primary constructor
public class HomeLibrarySqlContext(DbContextOptions<HomeLibrarySqlContext> options) 
    : DbContext(options)
{
    public virtual DbSet<Author> Authors { get; set; }
    public virtual DbSet<Book> Books { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure relationships explicitly
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasOne(d => d.Author)
                .WithMany(p => p.Books)
                .HasConstraintName("FK_Books_Authors");
        });
    }
}
```

#### 3. HomeLibrary.BusinessLogic (Business Layer)
**Purpose**: Business logic, data transformation, validation.

**Dependencies**: `HomeLibrary.Core`, `HomeLibrary.DataAccess`

**Naming**: Managers (e.g., `BookManager`, `AuthorManager`)

**Conventions**:
```csharp
// Manager implements interface from Core
public class BookManager(HomeLibrarySqlContext context) : IBookManager
{
    // Use AsNoTracking for read-only queries
    public async Task<int> GetOwnedBookCountAsync()
    {
        return await context.Books
            .AsNoTracking()
            .Where(b => b.BookStatusId == BookStatuses.Owned.Id)
            .CountAsync();
    }
    
    // Transform entities to DTOs
    public async Task<IEnumerable<AuthorDto>> GetAuthorsAsync()
    {
        return await context.Authors
            .AsNoTracking()
            .OrderBy(a => a.Name)
            .Select(a => new AuthorDto
            {
                Id = a.Id,
                Name = a.Name
            })
            .ToListAsync();
    }
}
```

#### 4. HomeLibrary.Web.Mvc (Presentation Layer)
**Purpose**: ASP.NET Core MVC controllers, views, view models, and frontend assets.

**Dependencies**: `HomeLibrary.Core`, `HomeLibrary.BusinessLogic`, `HomeLibrary.DataAccess`

**Structure**:
```
HomeLibrary.Web.Mvc/
??? Controllers/          # MVC controllers
??? Models/              # View models only (not domain models)
??? Views/               # Razor views
?   ??? Home/
?   ??? Shared/
??? wwwroot/
?   ??? css/            # Custom CSS files
?   ??? js/             # Compiled JavaScript (from TypeScript)
?   ??? ts/             # TypeScript source files
??? Program.cs          # Application startup and DI configuration
??? tsconfig.json       # TypeScript configuration
??? package.json        # NPM dependencies
```

**Conventions**:
```csharp
// Controllers - inject managers via primary constructor
public class HomeController(
    ILogger<HomeController> logger,
    IBookManager bookManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var viewModel = new HomeViewModel
        {
            TotalBooksOwned = await bookManager.GetOwnedBookCountAsync()
        };
        return View(viewModel);
    }
}

// View Models - simple POCOs for views
public class HomeViewModel
{
    public int TotalBooksOwned { get; set; }
    public int TotalWishlist { get; set; }
}
```

**Dependency Injection Registration** (Program.cs):
```csharp
// DbContext registration
builder.Services.AddDbContext<HomeLibrarySqlContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HomeLibrarySql")));

// Manager registration (use Transient for stateless services)
builder.Services.AddTransient<IBookManager, BookManager>();
builder.Services.AddTransient<IAuthorManager, AuthorManager>();
builder.Services.AddTransient<IGenreManager, GenreManager>();
```

## Frontend Guidelines

### Icons - Bootstrap Icons

**Always use Bootstrap Icons** for all iconography throughout the application.

**CDN Integration**:
```html
<!-- Include in _Layout.cshtml <head> -->
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css">
```

**Usage in Razor Views**:
```html
<!-- Basic icon -->
<i class="bi bi-book-fill"></i>

<!-- Icon with text -->
<button class="btn">
    <i class="bi bi-plus-circle"></i>
    <span>Add Book</span>
</button>

<!-- Icon in link -->
<a href="#" class="menu-link">
    <i class="bi bi-heart"></i>
    <span>My Wishlist</span>
</a>
```

**Standard Icons Used**:
- **Navigation**: `bi-list` (menu), `bi-x-lg` (close), `bi-three-dots-vertical` (more options)
- **Library**: `bi-book-fill`, `bi-book`, `bi-journal-bookmark`
- **Actions**: `bi-plus-circle` (add), `bi-heart` (wishlist), `bi-heart-fill` (filled heart)
- **Search**: `bi-search`, `bi-mic-fill` (voice search)
- **Settings**: `bi-gear`
- **User**: `bi-person`
- **Tags**: `bi-tag`
- **Empty State**: `bi-inbox`

**CSS Styling for Icons**:
```css
/* Icon sizing */
.app-header__logo {
    font-size: 1.5rem;
    color: var(--primary-color);
}

/* Icon with text (flex layout) */
.btn {
    display: flex;
    align-items: center;
    gap: var(--spacing-xs);
}

.btn i {
    font-size: 1.25rem;
}

/* Icon colors */
.menu-link i {
    color: var(--primary-color);
}

/* Icon hover effects */
.icon-btn:hover i {
    color: var(--primary-hover);
}
```

**Icon Guidelines**:
- Use semantic icon choices that match functionality
- Maintain consistent sizing within contexts (header icons, button icons, card icons)
- Always pair icons with text for accessibility (use `<span>` for text)
- Apply `aria-label` to icon-only buttons
- Use filled variants (`-fill`) for active/selected states
- Color icons using CSS variables for theme consistency

### CSS Architecture

**Mobile-First Approach**: Always design for mobile first, then add responsive breakpoints.

**CSS Custom Properties** (Root Variables):
```css
:root {
    /* Colors */
    --primary-color: #4A90E2;
    --primary-hover: #3A7BC8;
    --background-color: #F5F5F5;
    --card-background: #FFFFFF;
    --text-primary: #333333;
    
    /* Spacing */
    --spacing-xs: 0.5rem;
    --spacing-sm: 1rem;
    --spacing-md: 1.5rem;
    --spacing-lg: 2rem;
    
    /* Typography */
    --font-size-base: 1rem;
    --font-size-lg: 1.25rem;
}
```

**Component-Based CSS**: Use BEM-like naming convention
```css
/* Component block */
.app-header { }

/* Component element */
.app-header__title { }
.app-header__menu-btn { }

/* Component modifier */
.btn-primary { }
.btn-secondary { }
```

**Responsive Breakpoints**:
```css
/* Mobile-first (default) */
.container { max-width: 600px; }

/* Tablet */
@media (min-width: 768px) {
    .container { max-width: 800px; }
}

/* Desktop */
@media (min-width: 1024px) {
    .container { max-width: 1000px; }
}
```

### TypeScript Guidelines

**Configuration** (tsconfig.json):
- Target: ES2020
- Module: ES2020
- Strict mode: enabled
- Output: `wwwroot/js/`
- Source: `wwwroot/ts/`

**Class-Based Modules**:
```typescript
interface DashboardElements {
    menuBtn: HTMLButtonElement | null;
    sideMenu: HTMLElement | null;
}

class Dashboard {
    private elements: DashboardElements;
    
    constructor() {
        this.elements = {
            menuBtn: document.getElementById('menuBtn') as HTMLButtonElement,
            sideMenu: document.getElementById('sideMenu') as HTMLElement
        };
        this.init();
    }
    
    private init(): void {
        this.attachEventListeners();
    }
    
    private attachEventListeners(): void {
        this.elements.menuBtn?.addEventListener('click', () => this.handleClick());
    }
}

// Initialize on DOM ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => new Dashboard());
} else {
    new Dashboard();
}
```

**Axios for API Calls** (future pattern):
```typescript
import axios from 'axios';

async function fetchBooks(): Promise<Book[]> {
    try {
        const response = await axios.get<Book[]>('/api/books');
        return response.data;
    } catch (error) {
        console.error('Error fetching books:', error);
        return [];
    }
}
```

## Database Conventions

### Entity Relationships
- Use navigation properties with `[InverseProperty]` attribute
- Configure foreign key constraints in `OnModelCreating`
- Use `[Column]` attribute for non-standard column names (e.g., `[Column("ISBN")]`)

### Entities
```csharp
public class Book
{
    [Key]
    public int Id { get; set; }
    
    public string Title { get; set; } = null!;
    
    [Column("ISBN")]
    [StringLength(50)]
    public string? Isbn { get; set; }
    
    public int BookStatusId { get; set; }
    
    [ForeignKey("BookStatusId")]
    [InverseProperty("Books")]
    public virtual BookStatus BookStatus { get; set; } = null!;
}
```

### Lookup/Reference Tables
- Use records for static lookup values (e.g., `BookStatuses`)
- Store static IDs as constants in record definitions

```csharp
public record BookStatuses(int Id, string Name)
{
    public static BookStatuses Owned { get; } = new BookStatuses(1, "Owned");
    public static BookStatuses Wishlist { get; } = new BookStatuses(2, "Wishlist");
    public static BookStatuses Read { get; } = new BookStatuses(3, "Read");
}
```

## Testing Conventions

### Unit Tests (HomeLibrary.BusinessLogic.Tests)
- Use xUnit framework
- Test business logic managers
- Mock DbContext where appropriate
- Name pattern: `MethodName_Scenario_ExpectedResult`

## UI/UX Patterns

### Design System
- **Mobile-first** responsive design
- **Card-based** layouts for content
- **Button styles**: Large touch targets (min 44px height)
- **Typography**: System font stack for fast loading
- **Colors**: Primary blue (`#4A90E2`), neutral grays
- **Spacing**: Consistent spacing scale (xs, sm, md, lg, xl)
- **Icons**: Bootstrap Icons for all iconography

### Navigation
- Hamburger menu (`bi-list`) for mobile navigation
- Side drawer menu with overlay
- Dropdown menu for contextual actions (`bi-three-dots-vertical`)
- Sticky header for easy access

### Components
- Action buttons (full-width on mobile, max-width on desktop) with icons
- Stat cards (stacked on mobile, grid on tablet+) with icon indicators
- Form inputs with mobile-friendly sizes
- Search bars with icon buttons
- Loading states and error handling

## Common Patterns

### Adding a New Feature

1. **Define Interface** in `HomeLibrary.Core/Interfaces/`
   ```csharp
   public interface IPublisherManager
   {
       Task<IEnumerable<PublisherDto>> GetPublishersAsync();
   }
   ```

2. **Create DTO** in `HomeLibrary.Core/DataTransferObjects/`
   ```csharp
   public class PublisherDto
   {
       public int Id { get; set; }
       public string Name { get; set; } = string.Empty;
   }
   ```

3. **Implement Manager** in `HomeLibrary.BusinessLogic/Managers/`
   ```csharp
   public class PublisherManager(HomeLibrarySqlContext context) : IPublisherManager
   {
       public async Task<IEnumerable<PublisherDto>> GetPublishersAsync()
       {
           return await context.Publishers
               .AsNoTracking()
               .Select(p => new PublisherDto { Id = p.Id, Name = p.Name })
               .ToListAsync();
       }
   }
   ```

4. **Register in DI** in `Program.cs`
   ```csharp
   builder.Services.AddTransient<IPublisherManager, PublisherManager>();
   ```

5. **Use in Controller**
   ```csharp
   public class PublisherController(IPublisherManager publisherManager) : Controller
   {
       public async Task<IActionResult> Index()
       {
           var publishers = await publisherManager.GetPublishersAsync();
           return View(publishers);
       }
   }
   ```

### Creating a New View

1. **Create View Model** in `HomeLibrary.Web.Mvc/Models/`
2. **Create Controller Action** that returns view with model
3. **Create Razor View** in appropriate `Views/` folder with Bootstrap Icons
4. **Add CSS** using component naming in `site.css`
5. **Add TypeScript** for interactions in `wwwroot/ts/`
6. **Compile TypeScript** to `wwwroot/js/`

### Adding Icons to a View

```html
<!-- Button with icon -->
<button class="btn btn-primary">
    <i class="bi bi-plus-circle"></i>
    <span>Add New Item</span>
</button>

<!-- Navigation link with icon -->
<a href="#" class="nav-link">
    <i class="bi bi-book-fill"></i>
    <span>My Library</span>
</a>

<!-- Icon-only button (with aria-label) -->
<button class="icon-btn" aria-label="Close menu">
    <i class="bi bi-x-lg"></i>
</button>

<!-- Card with icon indicator -->
<div class="stat-card">
    <i class="bi bi-heart-fill stat-card__icon"></i>
    <div class="stat-card__label">Total Wishlist</div>
    <div class="stat-card__value">20</div>
</div>
```

## Error Handling

### Controllers
```csharp
public async Task<IActionResult> GetBook(int id)
{
    try
    {
        var book = await _bookManager.GetBookByIdAsync(id);
        if (book == null)
        {
            return NotFound();
        }
        return View(book);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error retrieving book {BookId}", id);
        return View("Error");
    }
}
```

### TypeScript
```typescript
private async fetchData(): Promise<void> {
    try {
        const response = await axios.get('/api/data');
        this.handleData(response.data);
    } catch (error) {
        console.error('Failed to fetch data:', error);
        this.showError('Unable to load data. Please try again.');
    }
}
```

## Performance Best Practices

1. **Use `AsNoTracking()`** for read-only queries
2. **Project to DTOs** in database queries using `Select()`
3. **Async all the way** - use async/await for I/O operations
4. **Lazy loading disabled** - use explicit includes when needed
5. **Connection string pooling** enabled by default
6. **Minimize JavaScript bundle size** - use ES2020 modules
7. **CDN for icons** - Bootstrap Icons loaded via CDN for caching

## Security Considerations

1. **Nullable reference types** prevent null reference exceptions
2. **Parameterized queries** via EF Core (prevents SQL injection)
3. **HTTPS** enforced in production
4. **CSRF protection** enabled by default in MVC
5. **Input validation** on models using data annotations

## Git Workflow

- **Main branch**: `main` (production-ready code)
- **Feature branches**: `feature/feature-name` (e.g., `feature/ui-dashboard`)
- **Commit messages**: Descriptive and concise
- **Repository**: https://github.com/tirrellcotton/home-library

## When Making Changes

### Always Consider:
1. **Layer separation** - Don't mix concerns across layers
2. **Interface-first** - Define interfaces in Core before implementation
3. **DTO usage** - Never expose entities directly to views
4. **Async patterns** - Use async/await for database operations
5. **Mobile-first** - Design for mobile, then enhance for desktop
6. **Type safety** - Use TypeScript, not JavaScript
7. **CSS variables** - Use root variables for consistency
8. **Component thinking** - Create reusable CSS components
9. **DI registration** - Register new services in Program.cs
10. **Build verification** - Ensure solution builds after changes
11. **Bootstrap Icons** - Use for all icons, not emojis or text symbols

### Don't:
- ? Use Bootstrap CSS framework (conflicts with custom CSS)
- ? Use emojis or text symbols for icons (use Bootstrap Icons)
- ? Add JavaScript frameworks (React, Angular, Vue)
- ? Mix Razor Pages with MVC
- ? Put business logic in controllers
- ? Expose entities directly to views
- ? Use synchronous database calls
- ? Ignore nullable reference type warnings
- ? Create large, monolithic CSS files without components

## Quick Reference

### File Locations
- **Entities**: `HomeLibrary.Core/Models/`
- **Interfaces**: `HomeLibrary.Core/Interfaces/`
- **DTOs**: `HomeLibrary.Core/DataTransferObjects/`
- **Managers**: `HomeLibrary.BusinessLogic/Managers/`
- **Controllers**: `HomeLibrary.Web.Mvc/Controllers/`
- **View Models**: `HomeLibrary.Web.Mvc/Models/`
- **Views**: `HomeLibrary.Web.Mvc/Views/`
- **CSS**: `HomeLibrary.Web.Mvc/wwwroot/css/site.css`
- **TypeScript**: `HomeLibrary.Web.Mvc/wwwroot/ts/`
- **JavaScript**: `HomeLibrary.Web.Mvc/wwwroot/js/` (compiled)

### Bootstrap Icons Reference
- **Documentation**: https://icons.getbootstrap.com/
- **CDN**: `https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css`
- **Usage**: `<i class="bi bi-{icon-name}"></i>`
- **Search**: Browse at https://icons.getbootstrap.com/ for appropriate icons

### Common Commands
```bash
# Build solution
dotnet build

# Run web application
dotnet run --project HomeLibrary.Web.Mvc

# Run tests
dotnet test

# EF Core migrations
dotnet ef migrations add MigrationName --project HomeLibrary.DataAccess --startup-project HomeLibrary.Web.Mvc
dotnet ef database update --project HomeLibrary.DataAccess --startup-project HomeLibrary.Web.Mvc

# TypeScript compilation
cd HomeLibrary.Web.Mvc
npm run build
npm run watch
```

## Questions to Ask When Uncertain

1. **Which layer does this belong in?** (Core, DataAccess, BusinessLogic, or Web.Mvc)
2. **Is this a domain model, DTO, or view model?**
3. **Should this be async?** (If it touches database, yes)
4. **Does this need an interface?** (If it's a service/manager, yes)
5. **Is this mobile-first?** (Always start mobile)
6. **Am I using CSS variables?** (For consistency)
7. **Is TypeScript being used instead of JavaScript?** (Always TypeScript)
8. **Did I register in DI?** (New services need registration)
9. **Am I using Bootstrap Icons?** (Use for all icons, not emojis)
10. **Is the icon semantic?** (Does it clearly represent the action/content?)

---

**Remember**: This is a learning project focused on clean architecture, modern C# features, and mobile-first web development. Prioritize code quality, maintainability, and best practices over quick solutions.
