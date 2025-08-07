using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LostAndFoundApp.Models;
using LostAndFoundApp.Services;

namespace LostAndFoundApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly LogService _logService;

    public HomeController(ILogger<HomeController> logger, LogService logService)
    {
        _logger = logger;
        _logService = logService;
    }

    public async Task<IActionResult> Index()
    {
        // Son 10 sistem logunu al (LOGIN ve LOGOUT hariç)
        var recentLogs = await _logService.GetRecentLogsExcludingLoginLogoutAsync(10);
        
        ViewBag.RecentActivities = recentLogs;
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
