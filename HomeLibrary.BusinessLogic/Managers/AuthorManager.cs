using HomeLibrary.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeLibrary.BusinessLogic.Managers;

public class AuthorManager(HomeLibrarySqlContext context): IAuthorManager
{
    public async Task<IEnumerable<AuthorDto>> GetAuthorsAsync()
    {
        return await context.Authors
            .AsNoTracking()
            .OrderBy(g => g.Name)
            .Select(g => new AuthorDto
            {
                Id = g.Id,
                Name = g.Name
            }).ToListAsync();
    }

    public AuthorDto? GetAuthorById(int id)
    {
        return context.Authors.Where(g => g.Id == id)
            .AsNoTracking()
            .Select(g => new AuthorDto
            {
                Id = g.Id,
                Name = g.Name
            }).FirstOrDefault();
    }
}