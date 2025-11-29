using HomeLibrary.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using HomeLibrary.Web.Mvc.Models;

namespace HomeLibrary.Web.Mvc.Controllers;

public class WishlistController(
    ILogger<WishlistController> logger,
    IBookManager bookManager) : Controller
{
    private readonly ILogger<WishlistController> _logger = logger;

    public async Task<IActionResult> Index(string? searchTerm = null)
    {
        var viewModel = new WishlistViewModel
        {
            Books = await bookManager.GetWishlistBooksAsync(searchTerm),
            SearchTerm = searchTerm
        };

        return View(viewModel);
    }
}
