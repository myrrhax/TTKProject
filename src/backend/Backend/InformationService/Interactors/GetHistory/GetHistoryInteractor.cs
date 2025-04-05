﻿using CSharpFunctionalExtensions;
using InformationService.DataAccess;
using InformationService.Entities;
using InformationService.Utils;
using Microsoft.EntityFrameworkCore;

namespace InformationService.Interactors.GetHistory;

public class GetHistoryInteractor(ApplicationContext context) : IBaseInteractor<GetHistoryParams, IEnumerable<PostHistory>>
{
    public async Task<Result<IEnumerable<PostHistory>, ErrorsContainer>> ExecuteAsync(GetHistoryParams param)
    {
        var query = context.History
            .AsNoTracking()
            .Include(h => h.Post)
            .AsQueryable();

        if (param.Query != null)
        {
            query = query.Where(h => EF.Functions.ILike(h.Title, $"%{param.Query}%")
                || h.PostId.ToString().Contains(param.Query));
        }

        if (param.DateSortType == DateSortType.Descending)
        {
            query = query.OrderByDescending(ph => ph.UpdateTime);
        }
        else
        {
            query = query.OrderBy(ph => ph.UpdateTime);
        }

        var entities = await query.ToListAsync();
        if (entities.Any())
        {
            return Result.Success<IEnumerable<PostHistory>, ErrorsContainer>(entities);
        }

        var errors = new ErrorsContainer();
        return Result.Failure<IEnumerable<PostHistory>, ErrorsContainer>(errors);
    }
}
