# ?? Home Library

A modern web application for managing your personal book collection and wishlist. Built with ASP.NET Core MVC and TypeScript, Home Library helps you catalog books you own and track titles you'd like to acquire.

![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![TypeScript](https://img.shields.io/badge/TypeScript-ES2020-3178C6?logo=typescript)
![SQL Server](https://img.shields.io/badge/SQL_Server-Database-CC2927?logo=microsoftsqlserver)
![License](https://img.shields.io/badge/license-MIT-green)

## ? Features

- **?? Library Management**: Track all books you own with detailed information
- **?? Wishlist**: Maintain a list of books you want to read or purchase
- **?? Search & Filter**: Quickly find books by title, author, genre, or other criteria
- **? Add/Edit Books**: Easy-to-use forms for managing your collection
- **?? Dashboard**: Overview of your library statistics
- **?? Responsive Design**: Works seamlessly on desktop and mobile devices
- **?? Modern UI**: Clean, intuitive interface with Bootstrap Icons

## ??? Architecture

The application follows a clean, layered architecture:

```
HomeLibrary/
??? HomeLibrary.Web.Mvc/          # ASP.NET Core MVC Web Layer
?   ??? Controllers/              # MVC Controllers
?   ??? Views/                    # Razor Views
?   ??? Models/                   # View Models
?   ??? wwwroot/                  # Static assets (CSS, JS, TypeScript)
??? HomeLibrary.BusinessLogic/    # Business Logic Layer
?   ??? Managers/                 # Business logic managers
??? HomeLibrary.Core/             # Core Domain Layer
?   ??? Models/                   # Domain entities
?   ??? DataTransferObjects/      # DTOs
?   ??? Interfaces/               # Service interfaces
??? HomeLibrary.DataAccess/       # Data Access Layer
?   ??? Contexts/                 # Entity Framework DbContext
??? HomeLibrary.BusinessLogic.Tests/  # Unit Tests
```

### Key Components

- **Controllers**: HomeController, LibraryController, WishlistController, AddBookController, BooksController
- **Managers**: BookManager, AuthorManager, GenreManager, PublisherManager
- **Database Entities**: Book, Author, Genre, Publisher, BookStatus

## ?? Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) (LocalDB, Express, or full version)
- [Node.js](https://nodejs.org/) (for TypeScript compilation)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/tirrellcotton/home-library.git
   cd home-library
   ```

2. **Configure the database connection**
   
   Update the connection string in `HomeLibrary.Web.Mvc/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "HomeLibrarySql": "Server=(localdb)\\mssqllocaldb;Database=HomeLibrary;Trusted_Connection=True;MultipleActiveResultSets=true"
     }
   }
   ```

3. **Apply database migrations**
   ```bash
   cd HomeLibrary.Web.Mvc
   dotnet ef database update
   ```

4. **Compile TypeScript files**
   ```bash
   cd HomeLibrary.Web.Mvc
   tsc
   ```

5. **Run the application**
   ```bash
   dotnet run --project HomeLibrary.Web.Mvc
   ```

6. **Access the application**
   
   Open your browser and navigate to: `https://localhost:5001` (or the port shown in the console)

## ??? Database Schema

The application uses Entity Framework Core with the following main entities:

- **Books**: Core book information (Title, ISBN, Published Year, Cover Image, Notes)
- **Authors**: Book authors
- **Genres**: Book categories/genres
- **Publishers**: Publishing companies
- **BookStatus**: Status indicators (Owned, Wishlist, etc.)

## ?? Frontend Technologies

- **TypeScript**: Type-safe JavaScript for enhanced development
- **ES2020 Modules**: Modern JavaScript module system
- **Custom CSS**: Responsive, mobile-first design
- **Bootstrap Icons**: Beautiful, consistent iconography

### TypeScript Modules

- `bookmodal.ts`: Book detail modal functionality
- `library.ts`: Library page interactions
- `wishlist.ts`: Wishlist page search and filtering
- `addbook.ts`: Add/edit book form handling
- `dashboard.ts`: Navigation and menu interactions

## ?? Testing

Run the unit tests:

```bash
dotnet test HomeLibrary.BusinessLogic.Tests
```

## ??? Development

### Building TypeScript

TypeScript files are located in `wwwroot/ts/` and compile to `wwwroot/js/`:

```bash
cd HomeLibrary.Web.Mvc
tsc --watch  # Watch mode for development
```

### Project Structure

- **HomeLibrary.Web.Mvc**: Main web application
- **HomeLibrary.BusinessLogic**: Business logic and services
- **HomeLibrary.Core**: Domain models, DTOs, and interfaces
- **HomeLibrary.DataAccess**: Entity Framework context and database access
- **HomeLibrary.BusinessLogic.Tests**: Unit tests

## ?? Configuration

### Environment Variables

The application supports environment-specific configuration:
- `appsettings.json`: Default settings
- `appsettings.Development.json`: Development environment
- `appsettings.Production.json`: Production environment

### Dependency Injection

Services are registered in `Program.cs`:
- `IBookManager` ? `BookManager`
- `IAuthorManager` ? `AuthorManager`
- `IGenreManager` ? `GenreManager`
- `IPublisherManager` ? `PublisherManager`

## ?? Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## ?? License

This project is licensed under the MIT License - see the LICENSE file for details.

## ?? Author

**Tirrell Cotton**
- GitHub: [@tirrellcotton](https://github.com/tirrellcotton)

## ?? Acknowledgments

- Built with [ASP.NET Core](https://docs.microsoft.com/aspnet/core/)
- Icons by [Bootstrap Icons](https://icons.getbootstrap.com/)
- Entity Framework Core for data access

---

? If you find this project useful, please consider giving it a star!
