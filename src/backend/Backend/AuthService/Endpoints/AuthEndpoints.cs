using AuthService.Utils;
using Carter;
using Microsoft.Win32;

namespace AuthService.Endpoints;

public class AuthEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/auth");

        var group = app.MapGroup("api/auth");

        group.MapPost("login", Login)
            .Produces<LoginResultDto>(StatusCodes.Status200OK)
            .Produces<ErrorsContainer>(StatusCodes.Status400BadRequest)
            .WithOpenApi();

        group.MapPost("register", Register)
            .Produces<RegisterResponseDto>(StatusCodes.Status200OK)
            .Produces<ErrorsContainer>(StatusCodes.Status400BadRequest)
            .WithOpenApi();

        group.MapPost("refresh", Refresh)
            .Produces<RefreshResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();

        group.MapPost("logout", Logout)
            .WithOpenApi();
    }
}
