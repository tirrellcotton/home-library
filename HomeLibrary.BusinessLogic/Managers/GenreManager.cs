using HomeLibrary.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeLibrary.BusinessLogic.Managers;

public class GenreManager(HomeLibrarySqlContext context) : IGenreManager
{
    public async Task<IEnumerable<GenreDto>> GetGenresAsync()
    {
        return await context.Genres
            .AsNoTracking()
            .OrderBy(g => g.Name)
            .Select(g => new GenreDto
        {
            Id = g.Id,
            Name = g.Name
        }).ToListAsync();
    }

    public GenreDto? GetGenreById(int id)
    {
        return context.Genres.Where(g => g.Id == id)
            .AsNoTracking()
            .Select(g => new GenreDto
            {
                Id = g.Id,
                Name = g.Name
            }).FirstOrDefault();
    }
}