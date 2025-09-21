using System.Diagnostics;
using HomeLibrary.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using HomeLibrary.Web.Mvc.Models;

namespace HomeLibrary.Web.Mvc.Controllers;

public class HomeController(ILogger<HomeController> logger, 
    IGenreManager genreManager,
    IAuthorManager authorManager) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;

    public async Task<IActionResult> Index()
    {
        var genres = await genreManager.GetGenresAsync();
        var authors = await authorManager.GetAuthorsAsync();
        return View();
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