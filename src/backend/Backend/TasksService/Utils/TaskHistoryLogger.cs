using System;
using System.Threading.Tasks;
using TasksService.DataAccess;
using TasksService.Entities;


using TaskEntity = TasksService.Entities.Task;

namespace TasksService.Utils;

public class TaskHistoryLogger
{
    private readonly ApplicationContext _context;

    public TaskHistoryLogger(ApplicationContext context)
    {
        _context = context;
    }

    public async System.Threading.Tasks.Task LogCreatedAsync(Guid taskId, Guid userId)
    {
        await Log(taskId, userId, TaskChangeType.Created, "Создание задачи");
    }

    public async System.Threading.Tasks.Task Log(Guid taskId, Guid userId, TaskChangeType type, string description)
    {
        var entry = new TaskHistory
        {
            Id = Guid.NewGuid(),
            TaskId = taskId,
            ChangedByUserId = userId,
            ChangedAt = DateTime.UtcNow,
            ChangeType = type,
            Description = description
        };

        await _context.TaskHistories.AddAsync(entry);
        await _context.SaveChangesAsync();
    }
}
