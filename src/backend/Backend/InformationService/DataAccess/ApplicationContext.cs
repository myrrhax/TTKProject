using InformationService.Configuration;
using InformationService.Entities;
using Microsoft.EntityFrameworkCore;

namespace InformationService.DataAccess;

public class ApplicationContext(DbContextOptions<ApplicationContext> options): DbContext(options)
{
    public DbSet<Post> Posts { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new PostsConfiguration());
    }
}
