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
    public class LineCodesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LineCodesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LineCodes
        public async Task<IActionResult> Index(int page = 1)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            int pageSize = 10;

            var totalItems = await _context.LineCodes.CountAsync();
            var lineCodes = await _context.LineCodes
                .OrderBy(l => l.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = (int)Math.Ceiling(totalItems / (double)pageSize);

            return View(lineCodes);
        }

        // GET: LineCodes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var lineCode = await _context.LineCodes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lineCode == null)
            {
                return NotFound();
            }

            return View(lineCode);
        }

        // GET: LineCodes/Create
        public IActionResult Create()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: LineCodes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Line")] LineCode lineCode)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                _context.Add(lineCode);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lineCode);
        }

        // GET: LineCodes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var lineCode = await _context.LineCodes.FindAsync(id);
            if (lineCode == null)
            {
                return NotFound();
            }
            return View(lineCode);
        }

        // POST: LineCodes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Line")] LineCode lineCode)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            if (id != lineCode.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lineCode);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LineCodeExists(lineCode.Id))
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
            return View(lineCode);
        }

        // GET: LineCodes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var lineCode = await _context.LineCodes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lineCode == null)
            {
                return NotFound();
            }

            return View(lineCode);
        }

        // POST: LineCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var lineCode = await _context.LineCodes.FindAsync(id);
            if (lineCode != null)
            {
                _context.LineCodes.Remove(lineCode);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // AJAX Methods
        [HttpPost]
        public async Task<IActionResult> CreateAjax([Bind("Id,Line")] LineCode lineCode)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return Json(new { success = false, message = "Yetkisiz erişim." });
            }

            if (ModelState.IsValid)
            {
                _context.Add(lineCode);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Hat başarıyla eklendi." });
            }
            return Json(new { success = false, message = "Geçersiz veri." });
        }

        [HttpPost]
        public async Task<IActionResult> EditAjax(int id, [Bind("Id,Line")] LineCode lineCode)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return Json(new { success = false, message = "Yetkisiz erişim." });
            }

            if (id != lineCode.Id)
            {
                return Json(new { success = false, message = "Geçersiz ID." });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lineCode);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Hat başarıyla güncellendi." });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LineCodeExists(lineCode.Id))
                    {
                        return Json(new { success = false, message = "Hat bulunamadı." });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Güncelleme sırasında hata oluştu." });
                    }
                }
            }
            return Json(new { success = false, message = "Geçersiz veri." });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAjax(int id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return Json(new { success = false, message = "Yetkisiz erişim." });
            }

            var lineCode = await _context.LineCodes.FindAsync(id);
            if (lineCode != null)
            {
                _context.LineCodes.Remove(lineCode);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Hat başarıyla silindi." });
            }
            return Json(new { success = false, message = "Hat bulunamadı." });
        }

        // API endpoint for getting line codes
        public async Task<IActionResult> GetLineCodes()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return Json(new { success = false, message = "Yetkisiz erişim." });
            }

            var lineCodes = await _context.LineCodes
                .Select(l => new { id = l.Id, line = l.Line })
                .ToListAsync();
            return Json(lineCodes);
        }

        private bool LineCodeExists(int id)
        {
            return _context.LineCodes.Any(e => e.Id == id);
        }
    }
}