using AuthService.Configuration;
using AuthService.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthService.DataAccess;

public class ApplicationContext(DbContextOptions<ApplicationContext> context): DbContext(context)
{
    public DbSet<ApplicationUser> Users { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());

        modelBuilder.Entity<ApplicationUser>()
            .HasData(new ApplicationUser
            {
                Name = "Иван",
                Surname = "Запара",
                SecondName = "Иванович-Запарович",
                Login = "ivan",

            },
            new ApplicationUser
            {

            });
    }
}
