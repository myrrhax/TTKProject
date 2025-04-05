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
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
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

var app = builder.Build();

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

// Включаем авторизацию (сначала .UseAuthentication, потом .UseAuthorization!)
app.UseAuthentication();
app.UseAuthorization();

// Carter endpoints
app.MapCarter();

app.Run();
