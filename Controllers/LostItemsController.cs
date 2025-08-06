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
    public class LostItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LostItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LostItems
        public async Task<IActionResult> Index(string sortOrder, string searchString, string statusFilter, string lineCodeFilter, int page = 1)
{
    int pageSize = 4;// Her sayfada 4 kayıt göster

    ViewData["DateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
    ViewData["CurrentFilter"] = searchString;
    ViewData["CurrentStatus"] = statusFilter;
    ViewData["CurrentLineCode"] = lineCodeFilter;
    ViewData["CurrentPage"] = page;

    // LineCodes for dropdown
    ViewBag.LineCodes = await _context.LineCodes.ToListAsync();

    var items = _context.LostItems
        .Include(i => i.LineCode)
        .AsQueryable();

    if (!string.IsNullOrEmpty(searchString))
    {
        items = items.Where(s => s.ItemName.Contains(searchString) || s.Description.Contains(searchString));
    }

    if (!string.IsNullOrEmpty(statusFilter))
    {
        if (Enum.TryParse(statusFilter, out LostItemStatus statusEnum))
        {
            items = items.Where(s => s.Status == statusEnum);
        }
    }

    if (!string.IsNullOrEmpty(lineCodeFilter) && int.TryParse(lineCodeFilter, out int lineCodeId))
    {
        items = items.Where(s => s.LineCodeId == lineCodeId);
    }

    items = sortOrder switch
    {
        "date_desc" => items.OrderByDescending(s => s.FoundDate),
        _ => items.OrderBy(s => s.FoundDate),
    };

    // Pagination işlemi
    var totalItems = await items.CountAsync();
    var itemsOnPage = await items.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

    ViewData["TotalPages"] = (int)Math.Ceiling(totalItems / (double)pageSize);

    return View(itemsOnPage);
}


        // GET: LostItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lostItem = await _context.LostItems
                .Include(l => l.LineCode)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lostItem == null)
            {
                return NotFound();
            }

            return View(lostItem);
        }

        // GET: LostItems/Create
        public IActionResult Create()
        {
            ViewData["LineCodeId"] = new SelectList(_context.LineCodes, "Id", "Line");
            return View();
        }

        // POST: LostItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemName,Description,FoundDate,Status,LineCodeId,Location,FoundBy")] LostItem lostItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lostItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["LineCodeId"] = new SelectList(_context.LineCodes, "Id", "Line", lostItem.LineCodeId);
            return View(lostItem);
        }

        // GET: LostItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lostItem = await _context.LostItems.FindAsync(id);
            if (lostItem == null)
            {
                return NotFound();
            }

            ViewData["LineCodeId"] = new SelectList(_context.LineCodes, "Id", "Line", lostItem.LineCodeId);
            return View(lostItem);
        }

        // POST: LostItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ItemName,Description,FoundDate,Status,LineCodeId,Location,FoundBy")] LostItem lostItem)
        {
            if (id != lostItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lostItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LostItemExists(lostItem.Id))
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

            ViewData["LineCodeId"] = new SelectList(_context.LineCodes, "Id", "Line", lostItem.LineCodeId);
            return View(lostItem);
        }

        // GET: LostItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lostItem = await _context.LostItems
                .Include(l => l.LineCode)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lostItem == null)
            {
                return NotFound();
            }

            return View(lostItem);
        }

        // POST: LostItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lostItem = await _context.LostItems.FindAsync(id);
            if (lostItem != null)
            {
                _context.LostItems.Remove(lostItem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // AJAX Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAjax([Bind("ItemName,Description,FoundDate,Status,LineCodeId,Location,FoundBy")] LostItem lostItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lostItem);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Kayıp eşya başarıyla eklendi." });
            }
            
            // Model validation hatalarını topla
            var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { Field = x.Key, Errors = x.Value.Errors.Select(e => e.ErrorMessage) })
                .ToList();
            
            var errorMessage = "Lütfen aşağıdaki hataları düzeltin:\n";
            foreach (var error in errors)
            {
                errorMessage += $"- {error.Field}: {string.Join(", ", error.Errors)}\n";
            }
            
            return Json(new { success = false, message = errorMessage, errors = errors });
        }

        // AJAX Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAjax(int id, [Bind("Id,ItemName,Description,FoundDate,Status,LineCodeId,Location,FoundBy")] LostItem lostItem)
        {
            if (id != lostItem.Id)
            {
                return Json(new { success = false, message = "Geçersiz kayıt." });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lostItem);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Kayıp eşya başarıyla güncellendi." });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LostItemExists(lostItem.Id))
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
            var lostItem = await _context.LostItems.FindAsync(id);
            if (lostItem != null)
            {
                _context.LostItems.Remove(lostItem);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Kayıp eşya başarıyla silindi." });
            }
            return Json(new { success = false, message = "Kayıt bulunamadı." });
        }

        // Get dropdown data for modals
        [HttpGet]
        public async Task<IActionResult> GetDropdownData()
        {
            var lineCodes = await _context.LineCodes.Select(l => new { l.Id, l.Line }).ToListAsync();
            
            return Json(new { lineCodes });
        }

        private bool LostItemExists(int id)
        {
            return _context.LostItems.Any(e => e.Id == id);
        }
    }
}
