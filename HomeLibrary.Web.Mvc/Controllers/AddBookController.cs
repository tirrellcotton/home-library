using HomeLibrary.Core.Interfaces;
using HomeLibrary.Core.Models;
using HomeLibrary.Core.Records;
using Microsoft.AspNetCore.Mvc;
using HomeLibrary.Web.Mvc.Models;

namespace HomeLibrary.Web.Mvc.Controllers;

public class AddBookController(
    ILogger<AddBookController> logger,
    IBookManager bookManager,
    IGenreManager genreManager,
    IAuthorManager authorManager,
    IPublisherManager publisherManager) : Controller
{
    private readonly ILogger<AddBookController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var viewModel = new AddBookViewModel
        {
            Genres = await genreManager.GetGenresAsync(),
            Authors = await authorManager.GetAuthorsAsync(),
            Publishers = await publisherManager.GetPublishersAsync()
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(AddBookViewModel model)
    {
        if (!ModelState.IsValid)
        {
            // Reload dropdown data
            model.Genres = await genreManager.GetGenresAsync();
            model.Authors = await authorManager.GetAuthorsAsync();
            model.Publishers = await publisherManager.GetPublishersAsync();
            return View(model);
        }

        try
        {
            var book = new Book
            {
                Title = model.Title!,
                Isbn = model.Isbn,
                AuthorId = model.AuthorId > 0 ? model.AuthorId : null,
                GenreId = model.GenreId,
                PublisherId = model.PublisherId > 0 ? model.PublisherId : null,
                PublishedYear = model.PublishedYear,
                BookStatusId = model.BookStatusId,
                CoverImageUrl = model.CoverImageUrl,
                Notes = model.Notes
            };

            await bookManager.AddBookAsync(book);

            TempData["SuccessMessage"] = "Book added successfully!";
            
            // Redirect based on status
            if (model.BookStatusId == BookStatuses.Owned.Id)
            {
                return RedirectToAction("Index", "Library");
            }
            else if (model.BookStatusId == BookStatuses.Wishlist.Id)
            {
                return RedirectToAction("Index", "Wishlist");
            }

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding book");
            ModelState.AddModelError("", "An error occurred while adding the book. Please try again.");
            
            // Reload dropdown data
            model.Genres = await genreManager.GetGenresAsync();
            model.Authors = await authorManager.GetAuthorsAsync();
            model.Publishers = await publisherManager.GetPublishersAsync();
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var book = await bookManager.GetBookByIdAsync(id);
        
        if (book == null)
        {
            return NotFound();
        }

        var viewModel = new EditBookViewModel
        {
            Id = book.Id,
            Title = book.Title,
            Isbn = book.Isbn,
            AuthorId = book.AuthorId,
            GenreId = book.GenreId,
            PublisherId = book.PublisherId,
            PublishedYear = book.PublishedYear,
            BookStatusId = book.BookStatusId,
            CoverImageUrl = book.CoverImageUrl,
            Notes = book.Notes,
            Genres = await genreManager.GetGenresAsync(),
            Authors = await authorManager.GetAuthorsAsync(),
            Publishers = await publisherManager.GetPublishersAsync()
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditBookViewModel model)
    {
        if (!ModelState.IsValid)
        {
            // Reload dropdown data
            model.Genres = await genreManager.GetGenresAsync();
            model.Authors = await authorManager.GetAuthorsAsync();
            model.Publishers = await publisherManager.GetPublishersAsync();
            return View(model);
        }

        try
        {
            var book = await bookManager.GetBookByIdAsync(model.Id);
            
            if (book == null)
            {
                return NotFound();
            }

            // Update book properties
            book.Title = model.Title!;
            book.Isbn = model.Isbn;
            book.AuthorId = model.AuthorId > 0 ? model.AuthorId : null;
            book.GenreId = model.GenreId;
            book.PublisherId = model.PublisherId > 0 ? model.PublisherId : null;
            book.PublishedYear = model.PublishedYear;
            book.BookStatusId = model.BookStatusId;
            book.CoverImageUrl = model.CoverImageUrl;
            book.Notes = model.Notes;

            await bookManager.UpdateBookAsync(book);

            TempData["SuccessMessage"] = "Book updated successfully!";
            
            // Redirect based on status
            if (model.BookStatusId == BookStatuses.Owned.Id)
            {
                return RedirectToAction("Index", "Library");
            }
            else if (model.BookStatusId == BookStatuses.Wishlist.Id)
            {
                return RedirectToAction("Index", "Wishlist");
            }

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating book {BookId}", model.Id);
            ModelState.AddModelError("", "An error occurred while updating the book. Please try again.");
            
            // Reload dropdown data
            model.Genres = await genreManager.GetGenresAsync();
            model.Authors = await authorManager.GetAuthorsAsync();
            model.Publishers = await publisherManager.GetPublishersAsync();
            return View(model);
        }
    }
}
