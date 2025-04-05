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
        string sortByLogin = "asc",
        string sortByFullname = "asc",
        string? sortByRole = null,
        string sortByRegistrationDate = "asc")
    {
        SortOptions loginSort = sortByLogin == "asc" ? SortOptions.ASCENDING : SortOptions.DESCENDING;
        SortOptions nameSort = sortByFullname == "asc" ? SortOptions.ASCENDING : SortOptions.DESCENDING;
        SortOptions registrationSort = sortByRegistrationDate == "asc" ? SortOptions.ASCENDING : SortOptions.DESCENDING;

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

        var param = new GetUsersParams(page, loginSort, nameSort, sortRole, registrationSort);
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
}
