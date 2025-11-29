using System.Diagnostics;
using HomeLibrary.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using HomeLibrary.Web.Mvc.Models;

namespace HomeLibrary.Web.Mvc.Controllers;

public class HomeController(ILogger<HomeController> logger, 
    IGenreManager genreManager,
    IAuthorManager authorManager,
    IBookManager bookManager) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;

    public async Task<IActionResult> Index()
    {
        var viewModel = new HomeViewModel
        {
            TotalBooksOwned = await bookManager.GetOwnedBookCountAsync(),
            TotalWishlist = await bookManager.GetWishlistBookCountAsync()
        };
        
        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}