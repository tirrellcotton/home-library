using HomeLibrary.Core.DataTransferObjects;
using HomeLibrary.Core.Interfaces;
using HomeLibrary.Core.Models;
using HomeLibrary.Core.Records;
using HomeLibrary.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;

namespace HomeLibrary.BusinessLogic.Managers;

public class BookManager(HomeLibrarySqlContext context) : IBookManager
{
    public async Task<int> GetOwnedBookCountAsync()
    {
        return await context.Books
            .AsNoTracking()
            .Where(b => b.BookStatusId == BookStatuses.Owned.Id)
            .CountAsync(); 
    }

    public async Task<int> GetWishlistBookCountAsync()
    {
        return await context.Books
            .AsNoTracking()
            .Where(b => b.BookStatusId == BookStatuses.Wishlist.Id)
            .CountAsync();
    }

    public async Task<IEnumerable<BookDto>> GetOwnedBooksAsync(string? searchTerm = null, int? genreId = null, int? authorId = null)
    {
        var query = context.Books
            .AsNoTracking()
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .Where(b => b.BookStatusId == BookStatuses.Owned.Id);

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(b => b.Title.Contains(searchTerm));
        }

        // Apply genre filter
        if (genreId.HasValue && genreId.Value > 0)
        {
            query = query.Where(b => b.GenreId == genreId.Value);
        }

        // Apply author filter
        if (authorId.HasValue && authorId.Value > 0)
        {
            query = query.Where(b => b.AuthorId == authorId.Value);
        }

        return await query
            .OrderBy(b => b.Title)
            .Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author != null ? b.Author.Name : null,
                Genre = b.Genre.Name,
                CoverImageUrl = b.CoverImageUrl,
                PublishedYear = b.PublishedYear
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<BookDto>> GetWishlistBooksAsync(string? searchTerm = null)
    {
        var query = context.Books
            .AsNoTracking()
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .Where(b => b.BookStatusId == BookStatuses.Wishlist.Id);

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(b => b.Title.Contains(searchTerm));
        }

        return await query
            .OrderBy(b => b.Title)
            .Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author != null ? b.Author.Name : null,
                Genre = b.Genre.Name,
                CoverImageUrl = b.CoverImageUrl,
                PublishedYear = b.PublishedYear
            })
            .ToListAsync();
    }

    public async Task<int> AddBookAsync(Book book)
    {
        context.Books.Add(book);
        await context.SaveChangesAsync();
        return book.Id;
    }

    public async Task<Book?> GetBookByIdAsync(int id)
    {
        return await context.Books
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .Include(b => b.Publisher)
            .Include(b => b.BookStatus)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task UpdateBookAsync(Book book)
    {
        context.Books.Update(book);
        await context.SaveChangesAsync();
    }
}