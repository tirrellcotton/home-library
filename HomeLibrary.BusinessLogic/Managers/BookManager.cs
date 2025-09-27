using HomeLibrary.Core.Records;
using Microsoft.EntityFrameworkCore;

namespace HomeLibrary.BusinessLogic.Managers;

public class BookManager(HomeLibrarySqlContext context)
{
    public async Task<int> GetOwnedBookCountAsync()
    {
        return await context.Books
            .AsNoTracking()
            .Where(b => b.BookStatusId == BookStatuses.Owned.Id)
            .CountAsync(); 
    }
}