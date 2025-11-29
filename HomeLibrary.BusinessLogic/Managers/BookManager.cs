using HomeLibrary.Core.Interfaces;
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
}