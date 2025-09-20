using System.Diagnostics;
using HomeLibrary.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using HomeLibrary.Web.Mvc.Models;

namespace HomeLibrary.Web.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IGenreManager _genreManager;

    public HomeController(ILogger<HomeController> logger, IGenreManager genreManager)
    {
        _logger = logger;
        _genreManager = genreManager;
    }

    public async Task<IActionResult> Index()
    {
        var genres = await _genreManager.GetGenresAsync();
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