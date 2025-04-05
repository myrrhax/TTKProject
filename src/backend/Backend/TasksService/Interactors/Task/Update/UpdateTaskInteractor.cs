using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using TasksService.Contracts.Task;
using TasksService.DataAccess;
using TasksService.Entities;
using TasksService.Utils;
using TaskStatusEnum = TasksService.Entities.TaskStatus;

namespace TasksService.Interactors.Task.Update;

public class UpdateTaskInteractor(ApplicationContext context, TaskHistoryLogger logger)
    : IBaseInteractor<UpdateTaskRequest, TaskResponse>
{
    public async Task<Result<TaskResponse, ErrorsContainer>> ExecuteAsync(UpdateTaskRequest param)
    {
        var task = await context.Tasks.FindAsync(param.Id);
        if (task == null)
        {
            var errors = new ErrorsContainer();
            errors.AddError("Task", "Задача не найдена");
            return Result.Failure<TaskResponse, ErrorsContainer>(errors);
        }

        task.Title = param.Title;
        task.Description = param.Description;
        task.DueDate = param.DueDate;
        task.Priority = Enum.Parse<TaskPriority>(param.Priority, true);
        task.Status = Enum.Parse<TaskStatusEnum>(param.Status, true);
        task.AssignedUserId = param.AssignedUserId;

        await context.SaveChangesAsync();
        await logger.Log(task.Id, task.AssignedUserId, TaskChangeType.Updated, "Редактирование задачи");

        return Result.Success<TaskResponse, ErrorsContainer>(new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt,
            Priority = task.Priority.ToString(),
            Status = task.Status.ToString(),
            AssignedUserId = task.AssignedUserId
        });
    }
}
