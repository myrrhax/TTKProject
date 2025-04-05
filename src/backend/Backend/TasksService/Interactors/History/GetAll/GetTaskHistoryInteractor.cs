using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using TasksService.Contracts.History;
using TasksService.DataAccess;
using TasksService.Utils;
using TasksService.Entities;

namespace TasksService.Interactors.History.GetAll;

public class GetTaskHistoryInteractor : IBaseInteractor<Guid?, List<TaskHistoryResponse>>
{
    private readonly ApplicationContext _context;

    public GetTaskHistoryInteractor(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<Result<List<TaskHistoryResponse>, ErrorsContainer>> ExecuteAsync(Guid? taskId)
    {
        var query = _context.TaskHistories.AsQueryable();

        if (taskId.HasValue)
        {
            query = query.Where(h => h.TaskId == taskId.Value);
        }

        var history = await query
            .OrderByDescending(h => h.ChangedAt)
            .Select(h => new TaskHistoryResponse
            {
                Id = h.Id,
                TaskId = h.TaskId,
                ChangedByUserId = h.ChangedByUserId,
                ChangedAt = h.ChangedAt,
                ChangeType = h.ChangeType.ToString(),
                Description = h.Description
            })
            .ToListAsync();

        return Result.Success<List<TaskHistoryResponse>, ErrorsContainer>(history);
    }
}