using AuthService.Configuration;
using AuthService.DataAccess;
using AuthService.Interactors;
using AuthService.Interactors.ChangePassword;
using AuthService.Interactors.ChangeRole;
using AuthService.Interactors.DeleteUser;
using AuthService.Interactors.EditUser;
using AuthService.Interactors.GetUsers;
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

builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Users"));
});

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
builder.Services.AddCarter();

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Interactors
builder.Services.AddScoped<IBaseInteractor<RegisterParams, RegisterResult>, RegisterInteractor>();
builder.Services.AddScoped<IBaseInteractor<LoginParams, LoginResult>, LoginInteractor>();
builder.Services.AddScoped<IBaseInteractor<RefreshTokenParam, RefreshTokenResult>, RefreshTokenInteractor>();
builder.Services.AddScoped<IBaseInteractor<GetUsersParams, GetUsersResult>, GetUsersListInteractor>();
builder.Services.AddScoped<IBaseInteractor<DeleteUserParams, bool>, DeleteUserInteractor>();
builder.Services.AddScoped<IBaseInteractor<ChangePasswordParams, bool>, ChangePasswordInteractor>();
builder.Services.AddScoped<IBaseInteractor<ChangeRoleParams, bool>, ChangeRoleInteractor>();
builder.Services.AddScoped<IBaseInteractor<EditUserParams, bool>, EditUserInteractor>();
#endregion

builder.Services.AddCors(options => options.AddPolicy("AllowAll", policy =>
{
    policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
}));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(); // Не забудь эту строку!

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Conn: " + builder.Configuration.GetConnectionString("Users"));
if (app.Environment.IsProduction())
{
    var context = app.Services.GetRequiredService<ApplicationContext>();
    var migrations = await context.Database.GetPendingMigrationsAsync();
    if (migrations.Any())
    {
        await context.Database.MigrateAsync();
    }
}

app.UseCors("AllowAll");
app.UseAuthorization();

app.MapCarter();
app.Run();
