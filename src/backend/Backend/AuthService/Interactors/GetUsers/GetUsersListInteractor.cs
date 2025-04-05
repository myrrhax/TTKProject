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
        if (filter.SortByLogin == SortOptions.ASCENDING)
            query = query.OrderBy(u => u.Login);
        else
            query = query.OrderByDescending(u => u.Login);

        if (filter.SortByFullname == SortOptions.ASCENDING)
            query = ((IOrderedQueryable<ApplicationUser>)query).ThenBy(u => u.Surname + u.Name + u.SecondName);
        else
            query = ((IOrderedQueryable<ApplicationUser>)query).ThenByDescending(u => u.Surname + u.Name + u.SecondName);

        if (filter.SortByRegistrationDate == SortOptions.ASCENDING)
            query = ((IOrderedQueryable<ApplicationUser>)query).ThenBy(u => u.CreationDate);
        else
            query = ((IOrderedQueryable<ApplicationUser>)query).ThenByDescending(u => u.CreationDate);

        return query;
    }

}
