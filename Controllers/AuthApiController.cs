using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LostAndFoundApp.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LostAndFoundApp.Services;

namespace LostAndFoundApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly LogService _logService;
        private readonly IConfiguration _configuration;

        public AuthApiController(ApplicationDbContext context, LogService logService, IConfiguration configuration)
        {
            _context = context;
            _logService = logService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest(new { message = "Kullanıcı adı ve şifre gereklidir." });
                }

                // Kullanıcıyı veritabanından bul
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserName == request.UserName && u.Password == request.Password);

                if (user == null)
                {
                    await _logService.LogAsync("API Login Failed", request.UserName, "Başarısız giriş denemesi");
                    return Unauthorized(new { message = "Geçersiz kullanıcı adı veya şifre." });
                }

                // JWT token oluştur
                var token = GenerateJwtToken(user);

                await _logService.LogAsync("API Login Success", user.UserName, "Kullanıcı API ile giriş yaptı");

                return Ok(new
                {
                    token = token,
                    userId = user.Id,
                    userName = user.UserName,
                    role = user.Rol,
                    expiresAt = DateTime.UtcNow.AddHours(24)
                });
            }
            catch (Exception ex)
            {
                await _logService.LogAsync("API Login Error", "System", $"Hata: {ex.Message}");
                return StatusCode(500, new { message = "Sunucu hatası oluştu." });
            }
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("LostAndFoundApp_SecretKey_2024_VeryLongSecretKey123456789");
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Rol)
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                Issuer = "LostAndFoundApp",
                Audience = "LostAndFoundApp",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}