using Microsoft.EntityFrameworkCore;
using TasksService.Entities;
using TasksService.Configuration;
using TaskService.Configuration;

namespace TasksService.DataAccess;

public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
{
    public DbSet<TasksService.Entities.Task> Tasks { get; set; } = null!;
    public DbSet<TaskHistory> TaskHistories { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new TaskConfiguration());
        modelBuilder.ApplyConfiguration(new TaskHistoryConfiguration());
    }
}
