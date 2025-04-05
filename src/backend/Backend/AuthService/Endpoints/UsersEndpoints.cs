using AuthService.Contracts;
using AuthService.Contracts.ChangePassword;
using AuthService.Contracts.ChangeRole;
using AuthService.Contracts.EditUser;
using AuthService.Contracts.GetUsers;
using AuthService.Entities;
using AuthService.Interactors;
using AuthService.Interactors.ChangePassword;
using AuthService.Interactors.ChangeRole;
using AuthService.Interactors.DeleteUser;
using AuthService.Interactors.EditUser;
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

        group.MapDelete("/{id:guid}", DeleteUser)
            .WithOpenApi();

        group.MapPatch("/change-password", ChangePassword)
            .WithOpenApi();

        group.MapPatch("/change-role", ChangeRole)
            .WithOpenApi();

        group.MapPut("", UpdateUser)
            .WithOpenApi();
    }

    public async Task<Results<Ok, BadRequest<ErrorsContainer>>> UpdateUser(IBaseInteractor<EditUserParams, bool> interactor, 
        EditUserRequest req)
    {
        var param = new EditUserParams(req.UserId, req.Login, req.Name, req.Surname, req.SecondName);

        var result = await interactor.ExecuteAsync(param);

        if (result.IsSuccess)
        {
            return TypedResults.Ok();
        }
        return TypedResults.BadRequest(result.Error);
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

    public async Task<Results<Ok, BadRequest<ErrorsContainer>>> ChangeRole(ChangeRoleRequest req,
        IBaseInteractor<ChangeRoleParams, bool> interactor)
    {

        Role role = req.Role == "admin" ? Role.Admin : Role.User;

        var param = new ChangeRoleParams(req.UserId, role);
        var result = await interactor.ExecuteAsync(param);

        if (result.IsSuccess)
        {
            return TypedResults.Ok();
        }
        return TypedResults.BadRequest(result.Error);
    }

    public async Task<Results<Ok, BadRequest<ErrorsContainer>>> ChangePassword(ChangePasswordRequest req,
        IBaseInteractor<ChangePasswordParams, bool> interactor)
    {
        var param = new ChangePasswordParams(req.UserId, req.Password);
        var result = await interactor.ExecuteAsync(param);

        if (result.IsSuccess)
        {
            return TypedResults.Ok();
        }
        return TypedResults.BadRequest(result.Error);
    }

    public async Task<Results<Ok, BadRequest<ErrorsContainer>>> DeleteUser(Guid id, 
        IBaseInteractor<DeleteUserParams, bool> interactor)
    {
        var param = new DeleteUserParams(id);
        var result = await interactor.ExecuteAsync(param);

        if (result.IsSuccess)
        {
            return TypedResults.Ok();
        }
        return TypedResults.BadRequest(result.Error);
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
