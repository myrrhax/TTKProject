using Carter;
using ImageService.DataAccess;
using ImageService.Interactors.GetById;
using ImageService.Interactors.Upload;
using ImageService.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Swagger (для удобства тестов)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF Core + PostgreSQL
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Images"));
});

// Carter
builder.Services.AddCarter();

// Подключаем заглушку авторизации
builder.Services.AddAuthentication("Dummy")
    .AddScheme<AuthenticationSchemeOptions, DummyAuthHandler>("Dummy", null);
builder.Services.AddAuthorization();

// Интеракторы и утилиты
builder.Services.AddScoped<UploadImageInteractor>();
builder.Services.AddScoped<GetImageByIdInteractor>();
builder.Services.AddScoped<ImageValidator>();

builder.Services.AddCors(options => options.AddPolicy("AllowAll", policy =>
{
    policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
}));

var app = builder.Build();

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsProduction())
{
    var context = app.Services.GetRequiredService<ApplicationContext>();
    var migrations = await context.Database.GetPendingMigrationsAsync();
    if (migrations.Any())
    {
        await context.Database.MigrateAsync();
    }
}

app.UseRouting();

// Включаем авторизацию (сначала .UseAuthentication, потом .UseAuthorization!)
app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowAll");
// Carter endpoints
app.MapCarter();

app.Run();
