using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LostAndFoundApp.Models;

namespace LostAndFoundApp.Controllers
{
    public class KullaniciController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KullaniciController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Kullanici
        public async Task<IActionResult> Index(string sortOrder, string searchString, string roleFilter, int page = 1)
        {
            int pageSize = 10; // Her sayfada 10 kullanıcı göster

            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentRole"] = roleFilter;
            ViewData["CurrentPage"] = page;

            var kullanicilar = _context.Kullanicilar.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                kullanicilar = kullanicilar.Where(s => s.AdSoyad.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(roleFilter))
            {
                kullanicilar = kullanicilar.Where(s => s.Rol == roleFilter);
            }

            kullanicilar = sortOrder switch
            {
                "name_desc" => kullanicilar.OrderByDescending(s => s.AdSoyad),
                _ => kullanicilar.OrderBy(s => s.AdSoyad),
            };

            // Pagination işlemi
            var totalItems = await kullanicilar.CountAsync();
            var itemsOnPage = await kullanicilar.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            ViewData["TotalPages"] = (int)Math.Ceiling(totalItems / (double)pageSize);

            return View(itemsOnPage);
        }

        // GET: Kullanici/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kullanici = await _context.Kullanicilar
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kullanici == null)
            {
                return NotFound();
            }

            return View(kullanici);
        }

        // GET: Kullanici/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Kullanici/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdSoyad,Sifre,Rol")] Kullanici kullanici)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kullanici);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kullanici);
        }

        // GET: Kullanici/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kullanici = await _context.Kullanicilar.FindAsync(id);
            if (kullanici == null)
            {
                return NotFound();
            }
            return View(kullanici);
        }

        // POST: Kullanici/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AdSoyad,Sifre,Rol")] Kullanici kullanici)
        {
            if (id != kullanici.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kullanici);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KullaniciExists(kullanici.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(kullanici);
        }

        // GET: Kullanici/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kullanici = await _context.Kullanicilar
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kullanici == null)
            {
                return NotFound();
            }

            return View(kullanici);
        }

        // POST: Kullanici/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kullanici = await _context.Kullanicilar.FindAsync(id);
            if (kullanici != null)
            {
                _context.Kullanicilar.Remove(kullanici);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KullaniciExists(int id)
        {
            return _context.Kullanicilar.Any(e => e.Id == id);
        }
    }
}