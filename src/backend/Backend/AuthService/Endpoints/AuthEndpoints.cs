using AuthService.Contracts.Login;
using AuthService.Contracts.Refresh;
using AuthService.Contracts.Register;
using AuthService.Interactors;
using AuthService.Interactors.Login;
using AuthService.Interactors.RefreshToken;
using AuthService.Interactors.Register;
using AuthService.Utils;
using Carter;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AuthService.Endpoints;

public class AuthEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/auth");

        group.MapPost("login", Login)
            .Produces<LoginResult>(StatusCodes.Status200OK)
            .Produces<ErrorsContainer>(StatusCodes.Status400BadRequest)
            .WithOpenApi();

        group.MapPost("register", Register)
            .Produces<RegisterResponseDto>(StatusCodes.Status200OK)
            .Produces<ErrorsContainer>(StatusCodes.Status400BadRequest)
            .WithOpenApi();

        group.MapPost("refresh", Refresh)
            .Produces<RefreshResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();

        group.MapPost("logout", Logout)
            .RequireAuthorization()
            .WithOpenApi();
    }

    private async Task<Results<Ok<RefreshResponseDto>, NotFound>> Refresh(HttpContext context,
        IBaseInteractor<RefreshTokenParam, RefreshTokenResult> interactor,
        IConfiguration configuration)
    {
        if (!context.Request.Cookies.ContainsKey(configuration["JwtConfig:CookieName"]!))
            return TypedResults.NotFound();

        string token = context.Request.Cookies[configuration["JwtConfig:CookieName"]!]!;
        var param = new RefreshTokenParam(token);

        var updatedToken = await interactor.ExecuteAsync(param);

        if (updatedToken.IsFailure)
            return TypedResults.NotFound();

        SetRefreshToken(context, updatedToken.Value.UpdatedRefreshToken, configuration);
        return TypedResults.Ok(new RefreshResponseDto(updatedToken.Value.AccessToken));
    }

    public async Task<Results<Ok<LoginResponseDto>, BadRequest<ErrorsContainer>>> Login(LoginRequestDto dto,
        HttpContext context,
        IBaseInteractor<LoginParams, LoginResult> interactor,
        IConfiguration configuration)
    {
        var param = new LoginParams(dto.Login, dto.Password);
        var result = await interactor.ExecuteAsync(param);

        if (result.IsSuccess)
        {
            SetRefreshToken(context, result.Value.RefreshToken, configuration);
            var resultDto = new LoginResponseDto(result.Value.Token);
            return TypedResults.Ok(resultDto);
        }
        return TypedResults.BadRequest(result.Error);
    }

    public IResult Logout(HttpContext context, IConfiguration configuration)
    {
        RemoveRefreshToken(context, configuration);

        return Results.Ok();
    }

    public async Task<Results<Ok<RegisterResponseDto>, BadRequest<ErrorsContainer>>> Register(RegisterRequestDto dto,
        HttpContext context,
        IBaseInteractor<RegisterParams, RegisterResult> interactor,
        IConfiguration configuration)
    {
        var param = new RegisterParams(dto.Login, dto.Password, dto.Name, dto.Surname, dto.SecondName);
        var result = await interactor.ExecuteAsync(param);

        if (result.IsSuccess)
        {
            SetRefreshToken(context, result.Value.RefreshToken, configuration);
            var resultDto = new RegisterResponseDto(result.Value.Token);
            return TypedResults.Ok(resultDto);
        }
        return TypedResults.BadRequest(result.Error);
    }

    private void SetRefreshToken(HttpContext context,
        string refreshToken,
        IConfiguration configuration)
    {
        context.Response.Cookies.Append(configuration["JwtConfig:CookieName"]!, refreshToken, new CookieOptions
        {
            HttpOnly = true,
        });
    }

    private void RemoveRefreshToken(HttpContext context, IConfiguration configuration)
    {
        if (context.Request.Cookies.ContainsKey(configuration["JwtConfig:CookieName"]!))
        {
            context.Response.Cookies.Delete(configuration["JwtConfig:CookieName"]!);
        }
    }
}
