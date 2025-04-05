using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using TasksService.Contracts.Task;
using TasksService.DataAccess;
using TasksService.Entities;
using TasksService.Utils;
using StatusEnum = TasksService.Entities.TaskStatus;

namespace TasksService.Interactors.Task.GetByStatus;

public class GetTasksByStatusInteractor : IBaseInteractor<string, List<TaskResponse>>
{
    private readonly ApplicationContext _context;

    public GetTasksByStatusInteractor(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<Result<List<TaskResponse>, ErrorsContainer>> ExecuteAsync(string status)
    {
        if (!Enum.TryParse<StatusEnum>(status, true, out var parsedStatus))
        {
            var errors = new ErrorsContainer();
            errors.AddError("Status", "Недопустимый статус задачи.");
            return Result.Failure<List<TaskResponse>, ErrorsContainer>(errors);
        }

        var tasks = await _context.Tasks
            .Where(t => t.Status == parsedStatus)
            .Select(t => new TaskResponse
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                CreatedAt = t.CreatedAt,
                Priority = t.Priority.ToString(),
                Status = t.Status.ToString(),
                AssignedUserId = t.AssignedUserId
            })
            .ToListAsync();

        return Result.Success<List<TaskResponse>, ErrorsContainer>(tasks);
    }
}