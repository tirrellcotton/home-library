using HomeLibrary.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using HomeLibrary.Web.Mvc.Models;

namespace HomeLibrary.Web.Mvc.Controllers;

public class LibraryController(
    ILogger<LibraryController> logger,
    IBookManager bookManager,
    IGenreManager genreManager,
    IAuthorManager authorManager) : Controller
{
    private readonly ILogger<LibraryController> _logger = logger;

    public async Task<IActionResult> Index(string? searchTerm = null, int? genreId = null, int? authorId = null)
    {
        var viewModel = new LibraryViewModel
        {
            Books = await bookManager.GetOwnedBooksAsync(searchTerm, genreId, authorId),
            Genres = await genreManager.GetGenresAsync(),
            Authors = await authorManager.GetAuthorsAsync(),
            SearchTerm = searchTerm,
            SelectedGenreId = genreId,
            SelectedAuthorId = authorId
        };

        return View(viewModel);
    }
}
