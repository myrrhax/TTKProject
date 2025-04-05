using AuthService.Contracts;
using AuthService.Contracts.GetUsers;
using AuthService.Entities;
using AuthService.Interactors;
using AuthService.Interactors.GetUsers;
using AuthService.Utils;
using Carter;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AuthService.Endpoints;

public class UsersEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/users");
        group.MapGet("", GetUsers)
            .WithOpenApi();
    }

    public async Task<Results<Ok<GetUsersResponse>, NotFound>> GetUsers(IBaseInteractor<GetUsersParams, GetUsersResult> interactor,
        int page = 1,
        string? sortByLogin = null,
        string? sortByFullname = null,
        string? sortByRole = null,
        string? sortByRegistrationDate = null,
        string query = "")
    {
        var loginSort = GetSortOptions(sortByLogin);
        var nameSort = GetSortOptions(sortByFullname);
        var dateSort = GetSortOptions(sortByRegistrationDate);

        Role? sortRole;
        switch (sortByRole)
        {
            case "user":
                sortRole = Role.User;
                break;
            case "admin":
                sortRole = Role.Admin;
                break;
            default:
                sortRole = null;
                break;
        }

        var param = new GetUsersParams(page, loginSort, nameSort, sortRole, dateSort, query);
        var result = await interactor.ExecuteAsync(param);

        if (result.IsSuccess)
        {
            List<UserDto> dtos = new List<UserDto>();
            foreach (var user in result.Value.Users)
            {
                dtos.Add(user);
            }

            var response = new GetUsersResponse(dtos, result.Value.MaxPages);
            return TypedResults.Ok(response);
        }

        return TypedResults.NotFound();
    }

    private static SortOptions GetSortOptions(string? filterName)
    {
        SortOptions sortType;
        switch (filterName)
        {
            case "asc":
                sortType = SortOptions.ASCENDING;
                break;
            case "desc":
                sortType = SortOptions.DESCENDING;
                break;
            default:
                sortType = SortOptions.NONE;
                break;
        }

        return sortType;
    }
}
