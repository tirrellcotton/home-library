using HomeLibrary.Core.DataTransferObjects;

namespace HomeLibrary.Core.Interfaces;

public interface IBookManager
{
    Task<int> GetOwnedBookCountAsync();
    Task<int> GetWishlistBookCountAsync();
    Task<IEnumerable<BookDto>> GetOwnedBooksAsync(string? searchTerm = null, int? genreId = null, int? authorId = null);
    Task<IEnumerable<BookDto>> GetWishlistBooksAsync(string? searchTerm = null);
}
