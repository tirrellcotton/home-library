using HomeLibrary.Core.DataTransferObjects;

namespace HomeLibrary.Web.Mvc.Models;

public class LibraryViewModel
{
    public IEnumerable<BookDto> Books { get; set; } = new List<BookDto>();
    public IEnumerable<GenreDto> Genres { get; set; } = new List<GenreDto>();
    public IEnumerable<AuthorDto> Authors { get; set; } = new List<AuthorDto>();
    public string? SearchTerm { get; set; }
    public int? SelectedGenreId { get; set; }
    public int? SelectedAuthorId { get; set; }
}
