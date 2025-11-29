using HomeLibrary.Core.DataTransferObjects;

namespace HomeLibrary.Core.Interfaces;

public interface IGenreManager
{
    Task<IEnumerable<GenreDto>> GetGenresAsync();
    GenreDto? GetGenreById(int id);
}