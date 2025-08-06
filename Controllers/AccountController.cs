using Microsoft.AspNetCore.Mvc;
using LostAndFoundApp.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace LostAndFoundApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
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
        public IActionResult Login(string userName, string password)
        {
            // Kullanici tablosundan kontrol et
            var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.AdSoyad == userName && k.Sifre == password);
            if (kullanici != null)
            {
                HttpContext.Session.SetString("UserName", kullanici.AdSoyad);
                HttpContext.Session.SetString("UserRole", kullanici.Rol);
                HttpContext.Session.SetInt32("UserId", kullanici.Id);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.m = "Geçersiz kullanıcı adı veya şifre.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
