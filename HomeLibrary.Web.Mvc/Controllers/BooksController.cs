using HomeLibrary.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HomeLibrary.Web.Mvc.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController(IBookManager bookManager) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBook(int id)
    {
        var book = await bookManager.GetBookByIdAsync(id);
        
        if (book == null)
        {
            return NotFound();
        }

        var bookData = new
        {
            id = book.Id,
            title = book.Title,
            author = book.Author?.Name,
            genre = book.Genre.Name,
            publisher = book.Publisher?.Name,
            publishedYear = book.PublishedYear,
            isbn = book.Isbn,
            coverImageUrl = book.CoverImageUrl,
            notes = book.Notes
        };

        return Ok(bookData);
    }
}
