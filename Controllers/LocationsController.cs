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
    public class LocationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LocationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Locations
        public async Task<IActionResult> Index(int page = 1)
{
    int pageSize = 10;

    var totalItems = await _context.Locations.CountAsync();
    var locations = await _context.Locations
        .Include(l => l.LineCode)
        .Include(l => l.LostItem)
        .OrderBy(l => l.Id)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    ViewData["CurrentPage"] = page;
    ViewData["TotalPages"] = (int)Math.Ceiling(totalItems / (double)pageSize);

    return View(locations);
}


        // GET: Locations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Locations
                .Include(l => l.LineCode)
                .Include(l => l.LostItem)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // GET: Locations/Create
        public IActionResult Create()
        {
            ViewBag.LineCodes = _context.LineCodes.ToList();
            ViewBag.LostItems = _context.LostItems.ToList();
            return View();
        }

        // POST: Locations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HttpPost]
        [HttpPost]

        [HttpPost]

        public async Task<IActionResult> Create([Bind("LineCodeId,LostItemId,KapiID")] Location location)
        {
            if (ModelState.IsValid)
            {
                _context.Add(location);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        // AJAX Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAjax([Bind("LineCodeId,LostItemId,KapiID")] Location location)
        {
            if (ModelState.IsValid)
            {
                _context.Add(location);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Lokasyon başarıyla eklendi." });
            }
            return Json(new { success = false, message = "Lütfen tüm alanları doğru şekilde doldurun." });
        }

        // GET: Locations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }
            
            ViewBag.LineCodes = _context.LineCodes.ToList();
            ViewBag.LostItems = _context.LostItems.ToList();
            return View(location);
        }

        // POST: Locations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LineCodeId,LostItemId,KapiID")] Location location)
        {
            if (id != location.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(location);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationExists(location.Id))
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
            
            ViewBag.LineCodes = _context.LineCodes.ToList();
            ViewBag.LostItems = _context.LostItems.ToList();
            return View(location);
        }

        // AJAX Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAjax(int id, [Bind("Id,LineCodeId,LostItemId,KapiID")] Location location)
        {
            if (id != location.Id)
            {
                return Json(new { success = false, message = "Geçersiz kayıt." });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(location);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Lokasyon başarıyla güncellendi." });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationExists(location.Id))
                    {
                        return Json(new { success = false, message = "Kayıt bulunamadı." });
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return Json(new { success = false, message = "Lütfen tüm alanları doğru şekilde doldurun." });
        }

        // AJAX Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAjax(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location != null)
            {
                _context.Locations.Remove(location);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Lokasyon başarıyla silindi." });
            }
            return Json(new { success = false, message = "Kayıt bulunamadı." });
        }

        // Get locations for dropdown
        [HttpGet]
        public async Task<IActionResult> GetLocations()
        {
            var locations = await _context.Locations
                .Include(l => l.LineCode)
                .Include(l => l.LostItem)
                .Select(l => new { id = l.Id, lineCode = l.LineCode.Line, lostItem = l.LostItem.ItemName, kapiId = l.KapiID })
                .ToListAsync();
            return Json(locations);
        }

        private bool LocationExists(int id)
        {
            return _context.Locations.Any(e => e.Id == id);
        }
    }
}
