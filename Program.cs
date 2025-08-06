using Microsoft.EntityFrameworkCore;
using LostAndFoundApp.Models;
using LostAndFoundApp.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ✅ 1. Servisleri ekle
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=lostitems.db"));

// ✅ Session'ı build'den ÖNCE ekle!
builder.Services.AddSession();

// ✅ Authentication ve Authorization servislerini ekle
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
    });
builder.Services.AddAuthorization();

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
