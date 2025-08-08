using Microsoft.EntityFrameworkCore;
using LostAndFoundApp.Models;
using LostAndFoundApp.Middleware;
using LostAndFoundApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ✅ 1. Servisleri ekle
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=lostitems.db"));

// LogService'i ekle
builder.Services.AddScoped<LogService>();

// ✅ Session'ı build'den ÖNCE ekle!
builder.Services.AddSession();

// ✅ Authentication ve Authorization servislerini ekle
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddCookie("Cookies", options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/Login";
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "LostAndFoundApp",
        ValidAudience = "LostAndFoundApp",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("LostAndFoundApp_SecretKey_2024_VeryLongSecretKey123456789"))
    };
});
builder.Services.AddAuthorization();

// CORS politikası ekle
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// ✅ 2. Middleware'leri sırayla tanımla
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// CORS middleware
app.UseCors("AllowAll");

// ✅ Session middleware burada kullanılmalı
app.UseSession();

// ✅ Authentication middleware'i ekle
app.UseAuthentication();
app.UseMiddleware<AuthenticationMiddleware>();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
