using HomeLibrary.BusinessLogic.Managers;
using HomeLibrary.Core.Models;
using HomeLibrary.Core.Records;
using HomeLibrary.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;

namespace HomeLibrary.BusinessLogic.Tests;

[TestClass]
public class BookManagerTests
{
    private HomeLibrarySqlContext? _context;
    private BookManager? _manager;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<HomeLibrarySqlContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new HomeLibrarySqlContext(options);
        _manager = new BookManager(_context);
        
        // Seed data
        SeedGenres(_context);
        SeedPublishers(_context);
        SeedAuthors(_context);
        SeedBookStatuses(_context);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context?.Database.EnsureDeleted();
        _context?.Dispose();
    }

    #region GetOwnedBookCountAsync Tests

    [TestMethod]
    public async Task GetOwnedBookCountAsync_ReturnsCorrectCount()
    {
        // Arrange
        _context!.Books.AddRange(
            new Book { Id = 1, Title = "Book One", GenreId = 1, BookStatusId = BookStatuses.Owned.Id },
            new Book { Id = 2, Title = "Book Two", GenreId = 1, BookStatusId = BookStatuses.Wishlist.Id },
            new Book { Id = 3, Title = "Book Three", GenreId = 1, BookStatusId = BookStatuses.Owned.Id }
        );
        await _context.SaveChangesAsync();

        // Act
        var count = await _manager!.GetOwnedBookCountAsync();

        // Assert
        Assert.AreEqual(2, count);
    }

    [TestMethod]
    public async Task GetOwnedBookCountAsync_WithNoOwnedBooks_ReturnsZero()
    {
        // Arrange
        _context!.Books.AddRange(
            new Book { Id = 1, Title = "Book One", GenreId = 1, BookStatusId = BookStatuses.Wishlist.Id },
            new Book { Id = 2, Title = "Book Two", GenreId = 1, BookStatusId = BookStatuses.Read.Id }
        );
        await _context.SaveChangesAsync();

        // Act
        var count = await _manager!.GetOwnedBookCountAsync();

        // Assert
        Assert.AreEqual(0, count);
    }

    [TestMethod]
    public async Task GetOwnedBookCountAsync_WithEmptyDatabase_ReturnsZero()
    {
        // Act
        var count = await _manager!.GetOwnedBookCountAsync();

        // Assert
        Assert.AreEqual(0, count);
    }

    #endregion

    #region GetWishlistBookCountAsync Tests

    [TestMethod]
    public async Task GetWishlistBookCountAsync_ReturnsCorrectCount()
    {
        // Arrange
        _context!.Books.AddRange(
            new Book { Id = 1, Title = "Book One", GenreId = 1, BookStatusId = BookStatuses.Owned.Id },
            new Book { Id = 2, Title = "Book Two", GenreId = 1, BookStatusId = BookStatuses.Wishlist.Id },
            new Book { Id = 3, Title = "Book Three", GenreId = 1, BookStatusId = BookStatuses.Wishlist.Id }
        );
        await _context.SaveChangesAsync();

        // Act
        var count = await _manager!.GetWishlistBookCountAsync();

        // Assert
        Assert.AreEqual(2, count);
    }

    [TestMethod]
    public async Task GetWishlistBookCountAsync_WithNoWishlistBooks_ReturnsZero()
    {
        // Arrange
        _context!.Books.AddRange(
            new Book { Id = 1, Title = "Book One", GenreId = 1, BookStatusId = BookStatuses.Owned.Id },
            new Book { Id = 2, Title = "Book Two", GenreId = 1, BookStatusId = BookStatuses.Read.Id }
        );
        await _context.SaveChangesAsync();

        // Act
        var count = await _manager!.GetWishlistBookCountAsync();

        // Assert
        Assert.AreEqual(0, count);
    }

    [TestMethod]
    public async Task GetWishlistBookCountAsync_WithEmptyDatabase_ReturnsZero()
    {
        // Act
        var count = await _manager!.GetWishlistBookCountAsync();

        // Assert
        Assert.AreEqual(0, count);
    }

    #endregion

    #region GetOwnedBooksAsync Tests

    [TestMethod]
    public async Task GetOwnedBooksAsync_WithNoFilters_ReturnsAllOwnedBooks()
    {
        // Arrange
        _context!.Books.AddRange(
            new Book { Id = 1, Title = "Book A", GenreId = 1, AuthorId = 1, BookStatusId = BookStatuses.Owned.Id, PublishedYear = 2020 },
            new Book { Id = 2, Title = "Book B", GenreId = 1, AuthorId = 1, BookStatusId = BookStatuses.Owned.Id, PublishedYear = 2021 },
            new Book { Id = 3, Title = "Book C", GenreId = 1, AuthorId = 1, BookStatusId = BookStatuses.Wishlist.Id, PublishedYear = 2022 }
        );
        await _context.SaveChangesAsync();

        // Act
        var books = await _manager!.GetOwnedBooksAsync();

        // Assert
        Assert.AreEqual(2, books.Count());
        Assert.IsTrue(books.All(b => b.Id == 1 || b.Id == 2));
    }

    [TestMethod]
    public async Task GetOwnedBooksAsync_WithSearchTerm_ReturnsMatchingBooks()
    {
        // Arrange
        _context!.Books.AddRange(
            new Book { Id = 1, Title = "The Great Gatsby", GenreId = 1, AuthorId = 1, BookStatusId = BookStatuses.Owned.Id },
            new Book { Id = 2, Title = "Great Expectations", GenreId = 1, AuthorId = 1, BookStatusId = BookStatuses.Owned.Id },
            new Book { Id = 3, Title = "To Kill a Mockingbird", GenreId = 1, AuthorId = 1, BookStatusId = BookStatuses.Owned.Id }
        );
        await _context.SaveChangesAsync();

        // Act
        var books = await _manager!.GetOwnedBooksAsync(searchTerm: "Great");

        // Assert
        Assert.AreEqual(2, books.Count());
        Assert.IsTrue(books.All(b => b.Title.Contains("Great")));
    }

    [TestMethod]
    public async Task GetOwnedBooksAsync_WithGenreFilter_ReturnsMatchingBooks()
    {
        // Arrange
        _context!.Books.AddRange(
            new Book { Id = 1, Title = "Book A", GenreId = 1, AuthorId = 1, BookStatusId = BookStatuses.Owned.Id },
            new Book { Id = 2, Title = "Book B", GenreId = 2, AuthorId = 1, BookStatusId = BookStatuses.Owned.Id },
            new Book { Id = 3, Title = "Book C", GenreId = 1, AuthorId = 1, BookStatusId = BookStatuses.Owned.Id }
        );
        await _context.SaveChangesAsync();

        // Act
        var books = await _manager!.GetOwnedBooksAsync(genreId: 1);

        // Assert
        Assert.AreEqual(2, books.Count());
        Assert.IsTrue(books.All(b => b.Genre == "Fiction"));
    }

    [TestMethod]
    public async Task GetOwnedBooksAsync_WithAuthorFilter_ReturnsMatchingBooks()
    {
        // Arrange
        _context!.Books.AddRange(
            new Book { Id = 1, Title = "Book A", GenreId = 1, AuthorId = 1, BookStatusId = BookStatuses.Owned.Id },
            new Book { Id = 2, Title = "Book B", GenreId = 1, AuthorId = 2, BookStatusId = BookStatuses.Owned.Id },
            new Book { Id = 3, Title = "Book C", GenreId = 1, AuthorId = 1, BookStatusId = BookStatuses.Owned.Id }
        );
        await _context.SaveChangesAsync();

        // Act
        var books = await _manager!.GetOwnedBooksAsync(authorId: 1);

        // Assert
        Assert.AreEqual(2, books.Count());
        Assert.IsTrue(books.All(b => b.Author == "Author One"));
    }

    [TestMethod]
    public async Task GetOwnedBooksAsync_WithAllFilters_ReturnsMatchingBooks()
    {
        // Arrange
        _context!.Books.AddRange(
            new Book { Id = 1, Title = "Great Book", GenreId = 1, AuthorId = 1, BookStatusId = BookStatuses.Owned.Id },
            new Book { Id = 2, Title = "Great Novel", GenreId = 2, AuthorId = 1, BookStatusId = BookStatuses.Owned.Id },
            new Book { Id = 3, Title = "Great Story", GenreId = 1, AuthorId = 2, BookStatusId = BookStatuses.Owned.Id },
            new Book { Id = 4, Title = "Another Book", GenreId = 1, AuthorId = 1, BookStatusId = BookStatuses.Owned.Id }
        );
        await _context.SaveChangesAsync();

        // Act
        var books = await _manager!.GetOwnedBooksAsync(searchTerm: "Great", genreId: 1, authorId: 1);

        // Assert
        Assert.AreEqual(1, books.Count());
        var book = books.First();
        Assert.AreEqual("Great Book", book.Title);
        Assert.AreEqual("Author One", book.Author);
        Assert.AreEqual("Fiction", book.Genre);
    }

    [TestMethod]
    public async Task GetOwnedBooksAsync_WithZeroGenreId_IgnoresGenreFilter()
    {
        // Arrange
        _context!.Books.AddRange(
            new Book { Id = 1, Title = "Book A", GenreId = 1, AuthorId = 1, BookStatusId = BookStatuses.Owned.Id },
            new Book { Id = 2, Title = "Book B", GenreId = 2, AuthorId = 1, BookStatusId = BookStatuses.Owned.Id }
        );
        await _context.SaveChangesAsync();

        // Act
        var books = await _manager!.GetOwnedBooksAsync(genreId: 0);

        // Assert
        Assert.AreEqual(2, books.Count());
    }

    [TestMethod]
    public async Task GetOwnedBooksAsync_WithZeroAuthorId_IgnoresAuthorFilter()
    {
        // Arrange
        _context!.Books.AddRange(
            new Book { Id = 1, Title = "Book A", GenreId = 1, AuthorId = 1, BookStatusId = BookStatuses.Owned.Id },
            new Book { Id = 2, Title = "Book B", GenreId = 1, AuthorId = 2, BookStatusId = BookStatuses.Owned.Id }
        );
        await _context.SaveChangesAsync();

        // Act
        var books = await _manager!.GetOwnedBooksAsync(authorId: 0);

        // Assert
        Assert.AreEqual(2, books.Count());
    }

    [TestMethod]
    public async Task GetOwnedBooksAsync_OrdersByTitle()
    {
        // Arrange
        _context!.Books.AddRange(
            new Book { Id = 1, Title = "Zebra", GenreId = 1, AuthorId = 1, BookStatusId = BookStatuses.Owned.Id },
            new Book { Id = 2, Title = "Apple", GenreId = 1, AuthorId = 1, BookStatusId = BookStatuses.Owned.Id },
            new Book { Id = 3, Title = "Mango", GenreId = 1, AuthorId = 1, BookStatusId = BookStatuses.Owned.Id }
        );
        await _context.SaveChangesAsync();

        // Act
        var books = await _manager!.GetOwnedBooksAsync();
        var bookList = books.ToList();

        // Assert
        Assert.AreEqual("Apple", bookList[0].Title);
        Assert.AreEqual("Mango", bookList[1].Title);
        Assert.AreEqual("Zebra", bookList[2].Title);
    }

    [TestMethod]
    public async Task GetOwnedBooksAsync_WithNoMatches_ReturnsEmptyList()
    {
        // Arrange
        _context!.Books.AddRange(
            new Book { Id = 1, Title = "Book A", GenreId = 1, AuthorId = 1, BookStatusId = BookStatuses.Owned.Id }
        );
        await _context.SaveChangesAsync();

        // Act
        var books = await _manager!.GetOwnedBooksAsync(searchTerm: "Nonexistent");

        // Assert
        Assert.AreEqual(0, books.Count());
    }

    [TestMethod]
    public async Task GetOwnedBooksAsync_WithNullAuthor_HandlesCorrectly()
    {
        // Arrange
        _context!.Books.AddRange(
            new Book { Id = 1, Title = "Book A", GenreId = 1, AuthorId = null, BookStatusId = BookStatuses.Owned.Id },
            new Book { Id = 2, Title = "Book B", GenreId = 1, AuthorId = 1, BookStatusId = BookStatuses.Owned.Id }
        );
        await _context.SaveChangesAsync();

        // Act
        var books = await _manager!.GetOwnedBooksAsync();
        var bookList = books.ToList();

        // Assert
        Assert.AreEqual(2, bookList.Count);
        Assert.IsNull(bookList.First(b => b.Id == 1).Author);
        Assert.IsNotNull(bookList.First(b => b.Id == 2).Author);
    }

    #endregion

    #region GetWishlistBooksAsync Tests

    [TestMethod]
    public async Task GetWishlistBooksAsync_WithNoSearchTerm_ReturnsAllWishlistBooks()
    {
        // Arrange
        _context!.Books.AddRange(
            new Book { Id = 1, Title = "Book A", GenreId = 1, AuthorId = 1, BookStatusId = BookStatuses.Wishlist.Id },
            new Book { Id = 2, Title = "Book B", GenreId = 1, AuthorId = 1, BookStatusId = BookStatuses.Wishlist.Id },
            new Book { Id = 3, Title = "Book C", GenreId = 1, AuthorId = 1, BookStatusId = BookStatuses.Owned.Id }
        );
        await _context.SaveChangesAsync();

        // Act
        var books = await _manager!.GetWishlistBooksAsync();

        // Assert
        Assert.AreEqual(2, books.Count());
        Assert.IsTrue(books.All(b => b.Id == 1 || b.Id == 2));
    }

    [TestMethod]
    public async Task GetWishlistBooksAsync_WithSearchTerm_ReturnsMatchingBooks()
    {
        // Arrange
        _context!.Books.AddRange(
            new Book { Id = 1, Title = "The Great Gatsby", GenreId = 1, AuthorId = 1, BookStatusId = BookStatuses.Wishlist.Id },
            new Book { Id = 2, Title = "Great Expectations", GenreId = 1, AuthorId = 1, BookStatusId = BookStatuses.Wishlist.Id },
            new Book { Id = 3, Title = "To Kill a Mockingbird", GenreId = 1, AuthorId = 1, BookStatusId = BookStatuses.Wishlist.Id }
        );
        await _context.SaveChangesAsync();

        // Act
        var books = await _manager!.GetWishlistBooksAsync(searchTerm: "Great");

        // Assert
        Assert.AreEqual(2, books.Count());
        Assert.IsTrue(books.All(b => b.Title.Contains("Great")));
    }

    [TestMethod]
    public async Task GetWishlistBooksAsync_OrdersByTitle()
    {
        // Arrange
        _context!.Books.AddRange(
            new Book { Id = 1, Title = "Zebra", GenreId = 1, AuthorId = 1, BookStatusId = BookStatuses.Wishlist.Id },
            new Book { Id = 2, Title = "Apple", GenreId = 1, AuthorId = 1, BookStatusId = BookStatuses.Wishlist.Id },
            new Book { Id = 3, Title = "Mango", GenreId = 1, AuthorId = 1, BookStatusId = BookStatuses.Wishlist.Id }
        );
        await _context.SaveChangesAsync();

        // Act
        var books = await _manager!.GetWishlistBooksAsync();
        var bookList = books.ToList();

        // Assert
        Assert.AreEqual("Apple", bookList[0].Title);
        Assert.AreEqual("Mango", bookList[1].Title);
        Assert.AreEqual("Zebra", bookList[2].Title);
    }

    [TestMethod]
    public async Task GetWishlistBooksAsync_WithEmptyDatabase_ReturnsEmptyList()
    {
        // Act
        var books = await _manager!.GetWishlistBooksAsync();

        // Assert
        Assert.AreEqual(0, books.Count());
    }

    [TestMethod]
    public async Task GetWishlistBooksAsync_WithNoMatches_ReturnsEmptyList()
    {
        // Arrange
        _context!.Books.AddRange(
            new Book { Id = 1, Title = "Book A", GenreId = 1, AuthorId = 1, BookStatusId = BookStatuses.Wishlist.Id }
        );
        await _context.SaveChangesAsync();

        // Act
        var books = await _manager!.GetWishlistBooksAsync(searchTerm: "Nonexistent");

        // Assert
        Assert.AreEqual(0, books.Count());
    }

    [TestMethod]
    public async Task GetWishlistBooksAsync_WithNullAuthor_HandlesCorrectly()
    {
        // Arrange
        _context!.Books.AddRange(
            new Book { Id = 1, Title = "Book A", GenreId = 1, AuthorId = null, BookStatusId = BookStatuses.Wishlist.Id },
            new Book { Id = 2, Title = "Book B", GenreId = 1, AuthorId = 1, BookStatusId = BookStatuses.Wishlist.Id }
        );
        await _context.SaveChangesAsync();

        // Act
        var books = await _manager!.GetWishlistBooksAsync();
        var bookList = books.ToList();

        // Assert
        Assert.AreEqual(2, bookList.Count);
        Assert.IsNull(bookList.First(b => b.Id == 1).Author);
        Assert.IsNotNull(bookList.First(b => b.Id == 2).Author);
    }

    #endregion

    #region AddBookAsync Tests

    [TestMethod]
    public async Task AddBookAsync_AddsBookToDatabase()
    {
        // Arrange
        var newBook = new Book
        {
            Title = "New Book",
            GenreId = 1,
            AuthorId = 1,
            PublisherId = 1,
            BookStatusId = BookStatuses.Owned.Id,
            PublishedYear = 2024,
            Isbn = "1234567890"
        };

        // Act
        var bookId = await _manager!.AddBookAsync(newBook);

        // Assert
        Assert.IsTrue(bookId > 0);
        var addedBook = await _context!.Books.FindAsync(bookId);
        Assert.IsNotNull(addedBook);
        Assert.AreEqual("New Book", addedBook.Title);
        Assert.AreEqual(1, addedBook.GenreId);
        Assert.AreEqual(1, addedBook.AuthorId);
    }

    [TestMethod]
    public async Task AddBookAsync_ReturnsGeneratedId()
    {
        // Arrange
        var newBook = new Book
        {
            Title = "Test Book",
            GenreId = 1,
            BookStatusId = BookStatuses.Owned.Id
        };

        // Act
        var bookId = await _manager!.AddBookAsync(newBook);

        // Assert
        Assert.IsTrue(bookId > 0);
        Assert.AreEqual(newBook.Id, bookId);
    }

    [TestMethod]
    public async Task AddBookAsync_WithMinimalData_Succeeds()
    {
        // Arrange
        var newBook = new Book
        {
            Title = "Minimal Book",
            GenreId = 1,
            BookStatusId = BookStatuses.Owned.Id
        };

        // Act
        var bookId = await _manager!.AddBookAsync(newBook);

        // Assert
        Assert.IsTrue(bookId > 0);
        var addedBook = await _context!.Books.FindAsync(bookId);
        Assert.IsNotNull(addedBook);
        Assert.IsNull(addedBook.AuthorId);
        Assert.IsNull(addedBook.PublisherId);
    }

    [TestMethod]
    public async Task AddBookAsync_WithAllOptionalFields_Succeeds()
    {
        // Arrange
        var newBook = new Book
        {
            Title = "Complete Book",
            GenreId = 1,
            AuthorId = 1,
            PublisherId = 1,
            BookStatusId = BookStatuses.Owned.Id,
            PublishedYear = 2024,
            Isbn = "1234567890123",
            CoverImageUrl = "https://example.com/cover.jpg",
            Notes = "Great book to read"
        };

        // Act
        var bookId = await _manager!.AddBookAsync(newBook);

        // Assert
        var addedBook = await _context!.Books.FindAsync(bookId);
        Assert.IsNotNull(addedBook);
        Assert.AreEqual("Complete Book", addedBook.Title);
        Assert.AreEqual(2024, addedBook.PublishedYear);
        Assert.AreEqual("1234567890123", addedBook.Isbn);
        Assert.AreEqual("https://example.com/cover.jpg", addedBook.CoverImageUrl);
        Assert.AreEqual("Great book to read", addedBook.Notes);
    }

    #endregion

    #region GetBookByIdAsync Tests

    [TestMethod]
    public async Task GetBookByIdAsync_WithValidId_ReturnsBook()
    {
        // Arrange
        var book = new Book
        {
            Title = "Test Book",
            GenreId = 1,
            AuthorId = 1,
            PublisherId = 1,
            BookStatusId = BookStatuses.Owned.Id
        };
        _context!.Books.Add(book);
        await _context.SaveChangesAsync();

        // Act
        var result = await _manager!.GetBookByIdAsync(book.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(book.Id, result.Id);
        Assert.AreEqual("Test Book", result.Title);
    }

    [TestMethod]
    public async Task GetBookByIdAsync_IncludesRelatedEntities()
    {
        // Arrange
        var book = new Book
        {
            Title = "Test Book",
            GenreId = 1,
            AuthorId = 1,
            PublisherId = 1,
            BookStatusId = BookStatuses.Owned.Id
        };
        _context!.Books.Add(book);
        await _context.SaveChangesAsync();

        // Act
        var result = await _manager!.GetBookByIdAsync(book.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Author);
        Assert.AreEqual("Author One", result.Author.Name);
        Assert.IsNotNull(result.Genre);
        Assert.AreEqual("Fiction", result.Genre.Name);
        Assert.IsNotNull(result.Publisher);
        Assert.AreEqual("Penguin", result.Publisher.Name);
        Assert.IsNotNull(result.BookStatus);
        Assert.AreEqual("Owned", result.BookStatus.Name);
    }

    [TestMethod]
    public async Task GetBookByIdAsync_WithInvalidId_ReturnsNull()
    {
        // Act
        var result = await _manager!.GetBookByIdAsync(999);

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task GetBookByIdAsync_WithNullAuthorAndPublisher_HandlesCorrectly()
    {
        // Arrange
        var book = new Book
        {
            Title = "Book Without Author",
            GenreId = 1,
            AuthorId = null,
            PublisherId = null,
            BookStatusId = BookStatuses.Owned.Id
        };
        _context!.Books.Add(book);
        await _context.SaveChangesAsync();

        // Act
        var result = await _manager!.GetBookByIdAsync(book.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNull(result.Author);
        Assert.IsNull(result.Publisher);
        Assert.IsNotNull(result.Genre);
        Assert.IsNotNull(result.BookStatus);
    }

    #endregion

    #region UpdateBookAsync Tests

    [TestMethod]
    public async Task UpdateBookAsync_UpdatesExistingBook()
    {
        // Arrange
        var book = new Book
        {
            Title = "Original Title",
            GenreId = 1,
            AuthorId = 1,
            BookStatusId = BookStatuses.Owned.Id,
            PublishedYear = 2020
        };
        _context!.Books.Add(book);
        await _context.SaveChangesAsync();

        // Act
        book.Title = "Updated Title";
        book.PublishedYear = 2024;
        await _manager!.UpdateBookAsync(book);

        // Assert
        var updatedBook = await _context.Books.FindAsync(book.Id);
        Assert.IsNotNull(updatedBook);
        Assert.AreEqual("Updated Title", updatedBook.Title);
        Assert.AreEqual(2024, updatedBook.PublishedYear);
    }

    [TestMethod]
    public async Task UpdateBookAsync_UpdatesAllFields()
    {
        // Arrange
        var book = new Book
        {
            Title = "Original",
            GenreId = 1,
            AuthorId = 1,
            PublisherId = 1,
            BookStatusId = BookStatuses.Owned.Id,
            PublishedYear = 2020,
            Isbn = "1234567890",
            CoverImageUrl = "https://example.com/old.jpg",
            Notes = "Old notes"
        };
        _context!.Books.Add(book);
        await _context.SaveChangesAsync();

        // Act
        book.Title = "Updated";
        book.GenreId = 2;
        book.AuthorId = 2;
        book.PublisherId = 2;
        book.BookStatusId = BookStatuses.Wishlist.Id;
        book.PublishedYear = 2024;
        book.Isbn = "9876543210";
        book.CoverImageUrl = "https://example.com/new.jpg";
        book.Notes = "New notes";
        await _manager!.UpdateBookAsync(book);

        // Assert
        var updatedBook = await _context.Books.FindAsync(book.Id);
        Assert.IsNotNull(updatedBook);
        Assert.AreEqual("Updated", updatedBook.Title);
        Assert.AreEqual(2, updatedBook.GenreId);
        Assert.AreEqual(2, updatedBook.AuthorId);
        Assert.AreEqual(2, updatedBook.PublisherId);
        Assert.AreEqual(BookStatuses.Wishlist.Id, updatedBook.BookStatusId);
        Assert.AreEqual(2024, updatedBook.PublishedYear);
        Assert.AreEqual("9876543210", updatedBook.Isbn);
        Assert.AreEqual("https://example.com/new.jpg", updatedBook.CoverImageUrl);
        Assert.AreEqual("New notes", updatedBook.Notes);
    }

    [TestMethod]
    public async Task UpdateBookAsync_CanSetOptionalFieldsToNull()
    {
        // Arrange
        var book = new Book
        {
            Title = "Test Book",
            GenreId = 1,
            AuthorId = 1,
            PublisherId = 1,
            BookStatusId = BookStatuses.Owned.Id,
            PublishedYear = 2020,
            Isbn = "1234567890"
        };
        _context!.Books.Add(book);
        await _context.SaveChangesAsync();

        // Act
        book.AuthorId = null;
        book.PublisherId = null;
        book.PublishedYear = null;
        book.Isbn = null;
        await _manager!.UpdateBookAsync(book);

        // Assert
        var updatedBook = await _context.Books.FindAsync(book.Id);
        Assert.IsNotNull(updatedBook);
        Assert.IsNull(updatedBook.AuthorId);
        Assert.IsNull(updatedBook.PublisherId);
        Assert.IsNull(updatedBook.PublishedYear);
        Assert.IsNull(updatedBook.Isbn);
    }

    [TestMethod]
    public async Task UpdateBookAsync_ChangeBookStatus_UpdatesCorrectly()
    {
        // Arrange
        var book = new Book
        {
            Title = "Test Book",
            GenreId = 1,
            BookStatusId = BookStatuses.Wishlist.Id
        };
        _context!.Books.Add(book);
        await _context.SaveChangesAsync();

        // Act
        book.BookStatusId = BookStatuses.Owned.Id;
        await _manager!.UpdateBookAsync(book);

        // Assert
        var updatedBook = await _context.Books.FindAsync(book.Id);
        Assert.IsNotNull(updatedBook);
        Assert.AreEqual(BookStatuses.Owned.Id, updatedBook.BookStatusId);
    }

    #endregion

    #region Seed Methods

    private void SeedGenres(HomeLibrarySqlContext context)
    {
        context.Genres.AddRange(
            new Genre { Id = 1, Name = "Fiction" },
            new Genre { Id = 2, Name = "Non-Fiction" },
            new Genre { Id = 3, Name = "Science Fiction" }
        );
        context.SaveChanges();
    }

    private void SeedPublishers(HomeLibrarySqlContext context)
    {
        context.Publishers.AddRange(
            new Publisher { Id = 1, Name = "Penguin" },
            new Publisher { Id = 2, Name = "HarperCollins" },
            new Publisher { Id = 3, Name = "Simon & Schuster" }
        );
        context.SaveChanges();
    }
    
    private void SeedAuthors(HomeLibrarySqlContext context)
    {
        context.Authors.AddRange(
            new Author { Id = 1, Name = "Author One" },
            new Author { Id = 2, Name = "Author Two" },
            new Author { Id = 3, Name = "Author Three" }
        );
        context.SaveChanges();
    }
    
    private void SeedBookStatuses(HomeLibrarySqlContext context)
    {
        context.BookStatuses.AddRange(
            new BookStatus { Id = 1, Name = "Owned" },
            new BookStatus { Id = 2, Name = "Wishlist" },
            new BookStatus { Id = 3, Name = "Read" }
        );
        
        context.SaveChanges();
    }
    
    #endregion
}