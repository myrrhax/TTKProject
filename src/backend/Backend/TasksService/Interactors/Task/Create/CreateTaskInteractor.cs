using CSharpFunctionalExtensions;
using TasksService.Contracts.Task;
using TasksService.DataAccess;
using TasksService.Entities;
using TasksService.Utils;
using StatusEnum = TasksService.Entities.TaskStatus;

namespace TasksService.Interactors.Task.Create;

public class CreateTaskInteractor(ApplicationContext context, TaskHistoryLogger logger)
    : IBaseInteractor<CreateTaskRequest, TaskResponse>
{
    public async Task<Result<TaskResponse, ErrorsContainer>> ExecuteAsync(CreateTaskRequest param)
    {
        try
        {
            var entity = new Entities.Task
            {
                Id = Guid.NewGuid(),
                Title = param.Title,
                Description = param.Description,
                DueDate = param.DueDate,
                Priority = Enum.Parse<TaskPriority>(param.Priority, true),
                Status = StatusEnum.Current, 
                CreatedAt = DateTime.UtcNow,
                AssignedUserId = param.AssignedUserId
            };

            await context.Tasks.AddAsync(entity);
            await context.SaveChangesAsync();

            await logger.LogCreatedAsync(entity.Id, entity.AssignedUserId);

            return Result.Success<TaskResponse, ErrorsContainer>(new TaskResponse
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                DueDate = entity.DueDate,
                CreatedAt = entity.CreatedAt,
                Priority = entity.Priority.ToString(),
                Status = entity.Status.ToString(),
                AssignedUserId = entity.AssignedUserId
            });
        }
        catch (Exception ex)
        {
            var errors = new ErrorsContainer();
            errors.AddError("Task", "Не удалось создать задачу: " + ex.Message);
            return Result.Failure<TaskResponse, ErrorsContainer>(errors);
        }
    }
}
