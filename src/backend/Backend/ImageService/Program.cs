using Carter;
using ImageService.DataAccess;
using ImageService.Interactors.GetById;
using ImageService.Interactors.Upload;
using ImageService.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Swagger (для тестирования и работы через Ocelot)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Image Service",
        Version = "v1"
    });
});

// EF Core + PostgreSQL
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Carter (минималистичная маршрутизация)
builder.Services.AddCarter();

// Заглушка авторизации (Dummy)
builder.Services.AddAuthentication("Dummy")
    .AddScheme<AuthenticationSchemeOptions, DummyAuthHandler>("Dummy", null);
builder.Services.AddAuthorization();

// Интеракторы и утилиты
builder.Services.AddScoped<UploadImageInteractor>();
builder.Services.AddScoped<GetImageByIdInteractor>();
builder.Services.AddScoped<ImageValidator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Image Service v1");
    });
}

// Роутинг и авторизация
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Endpoints от Carter
app.MapCarter();

app.Run();
