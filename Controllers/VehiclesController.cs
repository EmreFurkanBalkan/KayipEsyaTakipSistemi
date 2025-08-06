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
    public class VehiclesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VehiclesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vehicles
        public async Task<IActionResult> Index(int page = 1)
{
    int pageSize = 10;

    var totalItems = await _context.Vehicles.CountAsync();
    var vehicles = await _context.Vehicles
        .OrderBy(v => v.Id)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    ViewData["CurrentPage"] = page;
    ViewData["TotalPages"] = (int)Math.Ceiling(totalItems / (double)pageSize);

    return View(vehicles);
}


        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create([Bind("PlateNumber,LineCode")] Vehicle vehicle)
{
    if (ModelState.IsValid)
    {
        _context.Add(vehicle);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    return View(vehicle);
}


        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PlateNumber,LineCode")] Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.Id))
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
            return View(vehicle);
        }

        // AJAX Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAjax([Bind("PlateNumber,Type,Capacity")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Araç başarıyla eklendi." });
            }
            return Json(new { success = false, message = "Lütfen tüm alanları doğru şekilde doldurun." });
        }

        // AJAX Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAjax(int id, [Bind("Id,PlateNumber,Type,Capacity")] Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return Json(new { success = false, message = "Geçersiz kayıt." });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Araç başarıyla güncellendi." });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.Id))
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
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Araç başarıyla silindi." });
            }
            return Json(new { success = false, message = "Kayıt bulunamadı." });
        }

        // Get vehicles for dropdown
        [HttpGet]
        public async Task<IActionResult> GetVehicles()
        {
            var vehicles = await _context.Vehicles
                .Select(v => new { id = v.Id, plateNumber = v.PlateNumber, type = v.LineCode })
                .ToListAsync();
            return Json(vehicles);
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicles.Any(e => e.Id == id);
        }
    }
}
