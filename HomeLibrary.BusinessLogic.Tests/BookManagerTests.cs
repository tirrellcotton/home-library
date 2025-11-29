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

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<HomeLibrarySqlContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new HomeLibrarySqlContext(options);
        
        // Seed data
        SeedPublishers(_context);
        SeedAuthors(_context);
        SeedBookStatuses(_context);
    }

    [TestMethod]
    public async Task GetOwnedBookCountAsync_ReturnsCorrectCount()
    {
        if (_context != null)
        {
            var manager = new BookManager(_context);
        
            // Arrange, add books with different statuses
            _context.Books.AddRange(
                new Book { Id = 1, Title = "Book One", AuthorId = 1, PublisherId = 1, BookStatusId = BookStatuses.Owned.Id },
                new Book { Id = 2, Title = "Book Two", AuthorId = 2, PublisherId = 2, BookStatusId = BookStatuses.Wishlist.Id },
                new Book { Id = 3, Title = "Book Three", AuthorId = 3, PublisherId = 3, BookStatusId = BookStatuses.Owned.Id }
            );
            await _context.SaveChangesAsync();
        
            var count = await manager.GetOwnedBookCountAsync();
            Assert.AreEqual(2, count);
        }
    }
    
    #region Seed Methods
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