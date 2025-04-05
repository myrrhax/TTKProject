using CSharpFunctionalExtensions;
using TasksService.Contracts.Task;
using TasksService.DataAccess;
using TasksService.Entities;
using TasksService.Utils;
using StatusEnum = TasksService.Entities.TaskStatus;

namespace TasksService.Interactors.Task.ChangeStatus;

public class ChangeTaskStatusInteractor(ApplicationContext context, TaskHistoryLogger logger)
    : IBaseInteractor<ChangeTaskStatusRequest, TaskResponse>
{
    public async Task<Result<TaskResponse, ErrorsContainer>> ExecuteAsync(ChangeTaskStatusRequest param)
    {
        var task = await context.Tasks.FindAsync(param.TaskId);
        if (task == null)
        {
            var errors = new ErrorsContainer();
            errors.AddError("Task", "Задача не найдена");
            return Result.Failure<TaskResponse, ErrorsContainer>(errors);
        }

        var newStatus = Enum.Parse<StatusEnum>(param.NewStatus, true);
        var oldStatus = task.Status;

        if ((oldStatus == StatusEnum.Current && newStatus == StatusEnum.Deferred) ||
            (oldStatus == StatusEnum.Current && newStatus == StatusEnum.Done) ||
            (oldStatus == StatusEnum.Deferred && newStatus == StatusEnum.Done) ||
            ((oldStatus == StatusEnum.Deferred || oldStatus == StatusEnum.Done) && newStatus == StatusEnum.Current))
        {
            task.Status = newStatus;
            await context.SaveChangesAsync();

            await logger.Log(task.Id, task.AssignedUserId, TaskChangeType.StatusChanged,
                $"Изменён статус: {oldStatus} → {newStatus}");

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

        else
        {
            var errors = new ErrorsContainer();
            errors.AddError("Status", "Недопустимое изменение статуса");
            return Result.Failure<TaskResponse, ErrorsContainer>(errors);
        }
    }
}