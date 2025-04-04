using AuthService.DataAccess;
using AuthService.Interactors;
using AuthService.Interactors.Login;
using AuthService.Interactors.RefreshToken;
using AuthService.Interactors.Register;
using AuthService.Utils;
using AuthService.Utils.JwtEncoder;
using AuthService.Utils.PasswordHasher;
using Carter;
using Microsoft.EntityFrameworkCore;
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
builder.Services.AddScoped<IBaseInteractor<RegisterParams, RegisterResponse>, RegisterInteractor>();
builder.Services.AddScoped<IBaseInteractor<LoginParams, LoginResponse>, LoginInteractor>();
builder.Services.AddScoped<IBaseInteractor<RefreshTokenParam, RefreshTokenResponse>, RefreshTokenInteractor>();
#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapCarter();
app.Run();
