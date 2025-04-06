using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TasksService.Entities;

namespace TaskService.Configuration;

public class TaskHistoryConfiguration : IEntityTypeConfiguration<TaskHistory>
{
    public void Configure(EntityTypeBuilder<TaskHistory> builder)
    {
        builder.HasKey(h => h.Id);

        builder.Property(h => h.TaskId).IsRequired();
        builder.Property(h => h.ChangedByUserId).IsRequired();
        builder.Property(h => h.ChangedAt).IsRequired();
        builder.Property(h => h.ChangeType).HasConversion<string>().IsRequired();
        builder.Property(h => h.Description).IsRequired().HasMaxLength(500);
    }
}
