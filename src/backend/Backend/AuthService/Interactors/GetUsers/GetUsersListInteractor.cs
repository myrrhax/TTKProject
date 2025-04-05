using AuthService.DataAccess;
using AuthService.Entities;
using AuthService.Utils;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Interactors.GetUsers;

public class GetUsersListInteractor(ApplicationContext context) : IBaseInteractor<GetUsersParams, GetUsersResult>
{
    private const int PAGE_SIZE = 10;
    public async Task<Result<GetUsersResult, ErrorsContainer>> ExecuteAsync(GetUsersParams param)
    {
        var query = context.Users.AsQueryable();

        if (param.SortByRole is not null)
        {
            query = query.Where(u => u.Role == param.SortByRole);
        }

        query = ApplySorting(query, param);
        string q = query.ToQueryString();

        var totalUsers = await query.CountAsync();
        var maxPages = (int)Math.Ceiling(totalUsers / (double) PAGE_SIZE);

        var users = await query
            .Skip((param.Page - 1) * PAGE_SIZE)
            .Take(PAGE_SIZE)
            .ToListAsync();

        if (users.Any())
        {
            var result = new GetUsersResult(users, maxPages);
            return Result.Success<GetUsersResult, ErrorsContainer>(result);
        }

        var errors = new ErrorsContainer();
        return Result.Failure<GetUsersResult, ErrorsContainer>(errors);
    }

    private IQueryable<ApplicationUser> ApplySorting(IQueryable<ApplicationUser> query, GetUsersParams filter)
    {
        if (filter.SortByRole is not null)
        {
            query = query.Where(p => p.Role == filter.SortByRole);
        }

        IOrderedQueryable<ApplicationUser>? resultQuery = null;

        if (filter.SortByFullname != SortOptions.NONE)
        {
            resultQuery = filter.SortByFullname == SortOptions.ASCENDING
                ? query.OrderBy(u => u.Surname + u.Name + u.SecondName)
                : query.OrderByDescending(u => u.Surname + u.Name + u.SecondName);
        }

        if (filter.SortByRegistrationDate != SortOptions.NONE)
        {
            if (resultQuery != null)
            {
                resultQuery = filter.SortByRegistrationDate == SortOptions.ASCENDING
                    ? resultQuery.ThenBy(u => u.CreationDate)
                    : resultQuery.ThenByDescending(u => u.CreationDate);
            }
            else
            {
                resultQuery = filter.SortByRegistrationDate == SortOptions.ASCENDING
                    ? query.OrderBy(u => u.CreationDate)
                    : query.OrderByDescending(u => u.CreationDate);
            }
        }

        if (filter.SortByLogin != SortOptions.NONE)
        {
            if (resultQuery != null)
            {
                resultQuery = filter.SortByLogin == SortOptions.ASCENDING
                    ? resultQuery.ThenBy(u => u.Login)
                    : resultQuery.ThenByDescending(u => u.Login);
            }
            else
            {
                resultQuery = filter.SortByLogin == SortOptions.ASCENDING
                    ? query.OrderBy(u => u.Login)
                    : query.OrderByDescending(u => u.Login);
            }
        }

        return resultQuery ?? query;
    }

}
