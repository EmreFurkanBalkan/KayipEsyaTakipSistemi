using Microsoft.EntityFrameworkCore;
using LostAndFoundApp.Models;


public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<LostItem> LostItems { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<LineCode> LineCodes { get; set; }
    public DbSet<Logs> Logs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);



        // Seed data for Users
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, UserName = "admin", Password = "Admin123", Rol = "Admin" },
            new User { Id = 2, UserName = "user1", Password = "Password1", Rol = "Moderatör" },
            new User { Id = 3, UserName = "user2", Password = "Password2", Rol = "Moderatör" },
            new User { Id = 4, UserName = "test", Password = "Test123", Rol = "Moderatör" },
            new User { Id = 5, UserName = "demo", Password = "Demo123", Rol = "Moderatör" }
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
        

    }
}
