using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LostAndFoundApp.Models;
using LostAndFoundApp.Services;
using System.ComponentModel.DataAnnotations;

namespace LostAndFoundApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LostItemsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly LogService _logService;

        public LostItemsApiController(ApplicationDbContext context, LogService logService)
        {
            _context = context;
            _logService = logService;
        }

        // GET: api/LostItemsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LostItemDto>>> GetLostItems()
        {
            var items = await _context.LostItems
                .Include(l => l.LineCode)
                .Select(l => new LostItemDto
                {
                    Id = l.Id,
                    ItemName = l.ItemName,
                    Description = l.Description,
                    FoundDate = l.FoundDate,
                    Status = l.Status.ToString(),
                    LineCodeId = l.LineCodeId,
                    LineCodeName = l.LineCode != null ? l.LineCode.Line : null,
                    Location = l.Location,
                    FoundBy = l.FoundBy
                })
                .ToListAsync();

            return Ok(items);
        }

        // GET: api/LostItemsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LostItemDto>> GetLostItem(int id)
        {
            var lostItem = await _context.LostItems
                .Include(l => l.LineCode)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lostItem == null)
            {
                return NotFound(new { message = "Kayıp eşya bulunamadı." });
            }

            var dto = new LostItemDto
            {
                Id = lostItem.Id,
                ItemName = lostItem.ItemName,
                Description = lostItem.Description,
                FoundDate = lostItem.FoundDate,
                Status = lostItem.Status.ToString(),
                LineCodeId = lostItem.LineCodeId,
                LineCodeName = lostItem.LineCode?.Line,
                Location = lostItem.Location,
                FoundBy = lostItem.FoundBy
            };

            return Ok(dto);
        }

        // POST: api/LostItemsApi
        [HttpPost]
        public async Task<ActionResult<LostItemDto>> CreateLostItem(CreateLostItemDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // LineCode kontrolü
            if (createDto.LineCodeId.HasValue)
            {
                var lineCodeExists = await _context.LineCodes.AnyAsync(lc => lc.Id == createDto.LineCodeId.Value);
                if (!lineCodeExists)
                {
                    return BadRequest(new { message = "Geçersiz hat kodu." });
                }
            }

            // Status enum kontrolü
            if (!Enum.TryParse<LostItemStatus>(createDto.Status, out var statusEnum))
            {
                return BadRequest(new { message = "Geçersiz durum değeri. Kullanılabilir değerler: TeslimEdilmedi, TeslimEdildi" });
            }

            var lostItem = new LostItem
            {
                ItemName = createDto.ItemName,
                Description = createDto.Description,
                FoundDate = createDto.FoundDate,
                Status = statusEnum,
                LineCodeId = createDto.LineCodeId,
                Location = createDto.Location,
                FoundBy = createDto.FoundBy
            };

            _context.LostItems.Add(lostItem);
            await _context.SaveChangesAsync();

            // Log kaydı
            await _logService.LogAsync("CREATE", $"Yeni kayıp eşya eklendi: {lostItem.ItemName}", "API_USER");

            // Oluşturulan item'ı LineCode ile birlikte getir
            var createdItem = await _context.LostItems
                .Include(l => l.LineCode)
                .FirstOrDefaultAsync(l => l.Id == lostItem.Id);

            var responseDto = new LostItemDto
            {
                Id = createdItem.Id,
                ItemName = createdItem.ItemName,
                Description = createdItem.Description,
                FoundDate = createdItem.FoundDate,
                Status = createdItem.Status.ToString(),
                LineCodeId = createdItem.LineCodeId,
                LineCodeName = createdItem.LineCode?.Line,
                Location = createdItem.Location,
                FoundBy = createdItem.FoundBy
            };

            return CreatedAtAction(nameof(GetLostItem), new { id = lostItem.Id }, responseDto);
        }

        // PUT: api/LostItemsApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLostItem(int id, UpdateLostItemDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var lostItem = await _context.LostItems.FindAsync(id);
            if (lostItem == null)
            {
                return NotFound(new { message = "Kayıp eşya bulunamadı." });
            }

            // LineCode kontrolü
            if (updateDto.LineCodeId.HasValue)
            {
                var lineCodeExists = await _context.LineCodes.AnyAsync(lc => lc.Id == updateDto.LineCodeId.Value);
                if (!lineCodeExists)
                {
                    return BadRequest(new { message = "Geçersiz hat kodu." });
                }
            }

            // Status enum kontrolü
            if (!Enum.TryParse<LostItemStatus>(updateDto.Status, out var statusEnum))
            {
                return BadRequest(new { message = "Geçersiz durum değeri. Kullanılabilir değerler: TeslimEdilmedi, TeslimEdildi" });
            }

            // Güncelleme
            lostItem.ItemName = updateDto.ItemName;
            lostItem.Description = updateDto.Description;
            lostItem.FoundDate = updateDto.FoundDate;
            lostItem.Status = statusEnum;
            lostItem.LineCodeId = updateDto.LineCodeId;
            lostItem.Location = updateDto.Location;
            lostItem.FoundBy = updateDto.FoundBy;

            try
            {
                await _context.SaveChangesAsync();
                
                // Log kaydı
                await _logService.LogAsync("UPDATE", $"Kayıp eşya güncellendi: {lostItem.ItemName}", "API_USER");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LostItemExists(id))
                {
                    return NotFound(new { message = "Kayıp eşya bulunamadı." });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/LostItemsApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLostItem(int id)
        {
            var lostItem = await _context.LostItems.FindAsync(id);
            if (lostItem == null)
            {
                return NotFound(new { message = "Kayıp eşya bulunamadı." });
            }

            _context.LostItems.Remove(lostItem);
            await _context.SaveChangesAsync();

            // Log kaydı
            await _logService.LogAsync("DELETE", $"Kayıp eşya silindi: {lostItem.ItemName}", "API_USER");

            return NoContent();
        }

        // GET: api/LostItemsApi/linecodes
        [HttpGet("linecodes")]
        public async Task<ActionResult<IEnumerable<object>>> GetLineCodes()
        {
            var lineCodes = await _context.LineCodes
                .Select(lc => new { id = lc.Id, line = lc.Line })
                .ToListAsync();

            return Ok(lineCodes);
        }

        private bool LostItemExists(int id)
        {
            return _context.LostItems.Any(e => e.Id == id);
        }
    }

    // DTO Classes
    public class LostItemDto
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public DateTime FoundDate { get; set; }
        public string Status { get; set; }
        public int? LineCodeId { get; set; }
        public string LineCodeName { get; set; }
        public string Location { get; set; }
        public string FoundBy { get; set; }
    }

    public class CreateLostItemDto
    {
        [Required(ErrorMessage = "Eşya adı gereklidir")]
        public string ItemName { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Bulunma tarihi gereklidir")]
        public DateTime FoundDate { get; set; }

        [Required(ErrorMessage = "Durum gereklidir")]
        public string Status { get; set; }

        public int? LineCodeId { get; set; }

        [Required(ErrorMessage = "Araç kapı numarası gereklidir")]
        public string Location { get; set; }

        public string FoundBy { get; set; }
    }

    public class UpdateLostItemDto
    {
        [Required(ErrorMessage = "Eşya adı gereklidir")]
        public string ItemName { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Bulunma tarihi gereklidir")]
        public DateTime FoundDate { get; set; }

        [Required(ErrorMessage = "Durum gereklidir")]
        public string Status { get; set; }

        public int? LineCodeId { get; set; }

        [Required(ErrorMessage = "Araç kapı numarası gereklidir")]
        public string Location { get; set; }

        public string FoundBy { get; set; }
    }
}