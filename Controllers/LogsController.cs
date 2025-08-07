using Microsoft.AspNetCore.Mvc;
using LostAndFoundApp.Services;
using LostAndFoundApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace LostAndFoundApp.Controllers
{
    public class LogsController : Controller
    {
        private readonly LogService _logService;

        public LogsController(LogService logService)
        {
            _logService = logService;
        }

        // GET: Logs
        public async Task<IActionResult> Index(string logType = "")
        {
            // Sadece Admin rolündeki kullanıcılar sistem loglarını görebilir
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            List<Logs> logs;
            
            if (string.IsNullOrEmpty(logType))
            {
                logs = await _logService.GetRecentLogsAsync(100);
            }
            else
            {
                logs = await _logService.GetLogsByTypeAsync(logType);
            }

            ViewBag.LogType = logType;
            return View(logs);
        }
    }
}