using CSharpFunctionalExtensions;
using TasksService.DataAccess;
using TasksService.Entities;
using TasksService.Utils;

namespace TasksService.Interactors.Task.Delete;

public class DeleteTaskInteractor(ApplicationContext context, TaskHistoryLogger logger)
    : IBaseInteractor<DeleteTaskParams, bool>
{
    public async Task<Result<bool, ErrorsContainer>> ExecuteAsync(DeleteTaskParams param)
    {
        var task = await context.Tasks.FindAsync(param.TaskId);
        if (task == null)
        {
            var errors = new ErrorsContainer();
            errors.AddError("Task", "Задача не найдена");
            return Result.Failure<bool, ErrorsContainer>(errors);
        }

        context.Tasks.Remove(task);
        await context.SaveChangesAsync();

        await logger.Log(param.TaskId, param.RedactorId, TaskChangeType.Deleted, "Удаление задачи");

        return Result.Success<bool, ErrorsContainer>(true);
    }
}