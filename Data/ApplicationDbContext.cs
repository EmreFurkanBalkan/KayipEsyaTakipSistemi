using Microsoft.EntityFrameworkCore;
using LostAndFoundApp.Models;


public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<LostItem> LostItems { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Kullanici> Kullanicilar { get; set; }
    public DbSet<LineCode> LineCodes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed data for Vehicles
        modelBuilder.Entity<Vehicle>().HasData(
            new Vehicle { Id = 1, PlateNumber = "34 ABC 123", LineCode = "M1" },
            new Vehicle { Id = 2, PlateNumber = "34 DEF 456", LineCode = "M2" },
            new Vehicle { Id = 3, PlateNumber = "34 GHI 789", LineCode = "M3" },
            new Vehicle { Id = 4, PlateNumber = "34 JKL 012", LineCode = "M4" },
            new Vehicle { Id = 5, PlateNumber = "34 MNO 345", LineCode = "M5" },
            new Vehicle { Id = 6, PlateNumber = "34 PQR 678", LineCode = "M6" },
            new Vehicle { Id = 7, PlateNumber = "34 STU 901", LineCode = "M7" },
            new Vehicle { Id = 8, PlateNumber = "34 VWX 234", LineCode = "M8" },
            new Vehicle { Id = 9, PlateNumber = "34 YZA 567", LineCode = "M9" },
            new Vehicle { Id = 10, PlateNumber = "34 BCD 890", LineCode = "M10" }
        );

        // Seed data for Locations
        modelBuilder.Entity<Location>().HasData(
            new Location { Id = 1, LineCodeId = 1, LostItemId = null, KapiID = "A1" },
            new Location { Id = 2, LineCodeId = 2, LostItemId = null, KapiID = "B2" },
            new Location { Id = 3, LineCodeId = 3, LostItemId = null, KapiID = "C3" },
            new Location { Id = 4, LineCodeId = 1, LostItemId = null, KapiID = "D4" },
            new Location { Id = 5, LineCodeId = 2, LostItemId = null, KapiID = "E5" }
        );

        // Seed data for Users
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, UserName = "admin", Password = "admin123" },
            new User { Id = 2, UserName = "user1", Password = "password1" },
            new User { Id = 3, UserName = "user2", Password = "password2" },
            new User { Id = 4, UserName = "test", Password = "test123" },
            new User { Id = 5, UserName = "demo", Password = "demo123" }
        );

        // Seed data for Kullanici
        modelBuilder.Entity<Kullanici>().HasData(
            new Kullanici { Id = 1, AdSoyad = "admin", Sifre = "admin123", Rol = "Admin" },
            new Kullanici { Id = 2, AdSoyad = "Ahmet Yılmaz", Sifre = "123456", Rol = "Kullanıcı" },
            new Kullanici { Id = 3, AdSoyad = "Mehmet Demir", Sifre = "password", Rol = "Kullanıcı" },
            new Kullanici { Id = 4, AdSoyad = "Ayşe Kaya", Sifre = "test123", Rol = "Moderatör" },
            new Kullanici { Id = 5, AdSoyad = "Fatma Özkan", Sifre = "demo123", Rol = "Kullanıcı" }
        );

        // Seed data for LineCodes
        modelBuilder.Entity<LineCode>().HasData(
            new LineCode { Id = 1, Line = "M1" },
            new LineCode { Id = 2, Line = "M2" },
            new LineCode { Id = 3, Line = "M3" },
            new LineCode { Id = 4, Line = "M4" },
            new LineCode { Id = 5, Line = "M5" },
            new LineCode { Id = 6, Line = "M6" },
            new LineCode { Id = 7, Line = "M7" },
            new LineCode { Id = 8, Line = "M8" },
            new LineCode { Id = 9, Line = "M9" },
            new LineCode { Id = 10, Line = "M10" }
        );
        
        // Configure Location relationships
        modelBuilder.Entity<Location>()
            .HasOne(l => l.LineCode)
            .WithMany(lc => lc.Locations)
            .HasForeignKey(l => l.LineCodeId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<Location>()
            .HasOne(l => l.LostItem)
            .WithMany(li => li.Locations)
            .HasForeignKey(l => l.LostItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
