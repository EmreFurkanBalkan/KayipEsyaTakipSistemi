using Microsoft.AspNetCore.Mvc;
using LostAndFoundApp.Models;
using LostAndFoundApp.Services;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace LostAndFoundApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly LogService _logService;

        public AccountController(ApplicationDbContext context, LogService logService)
        {
            _context = context;
            _logService = logService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [Route("/login")]
        public IActionResult LoginAlias()
        {
            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password)
        {
            // Users tablosundan kontrol et
            var user = _context.Users.FirstOrDefault(u => u.UserName == userName && u.Password == password);
            if (user != null)
            {
                HttpContext.Session.SetString("UserName", user.UserName);
                HttpContext.Session.SetString("UserRole", user.Rol);
                HttpContext.Session.SetInt32("UserId", user.Id);
                
                // Log kaydı
                await _logService.LogAsync("LOGIN", user.UserName, $"{user.UserName} sisteme giriş yaptı ({user.Rol})");
                
                return RedirectToAction("Index", "Home");
            }

            ViewBag.m = "Geçersiz kullanıcı adı veya şifre.";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            var userName = HttpContext.Session.GetString("UserName");
            var userRole = HttpContext.Session.GetString("UserRole");
            await _logService.LogAsync("Logout", userName, $"{userName} sistemden çıkış yaptı ({userRole})");

            HttpContext.Session.Clear();

            return RedirectToAction("Login");

        }
    }
}
