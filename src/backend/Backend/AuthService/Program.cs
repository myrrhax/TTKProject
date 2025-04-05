using AuthService.Configuration;
using AuthService.DataAccess;
using AuthService.Interactors;
using AuthService.Interactors.GetUsers;
using AuthService.Interactors.Login;
using AuthService.Interactors.RefreshToken;
using AuthService.Interactors.Register;
using AuthService.Utils;
using AuthService.Utils.JwtEncoder;
using AuthService.Utils.PasswordHasher;
using Carter;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using WebAPI.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
builder.Services.AddCarter();

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddDocumentation();

#region Interactors
builder.Services.AddScoped<IBaseInteractor<RegisterParams, RegisterResult>, RegisterInteractor>();
builder.Services.AddScoped<IBaseInteractor<LoginParams, LoginResult>, LoginInteractor>();
builder.Services.AddScoped<IBaseInteractor<RefreshTokenParam, RefreshTokenResult>, RefreshTokenInteractor>();
builder.Services.AddScoped<IBaseInteractor<GetUsersParams, GetUsersResult>, GetUsersListInteractor>();
#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(opt =>
    {
        opt.RouteTemplate = "openapi/{documentName}.json";
    });

    app.MapScalarApiReference(opt =>
    {
        opt.Title = "WebChatAPI";
        opt.Theme = ScalarTheme.Mars;
        opt.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapCarter();
app.Run();
