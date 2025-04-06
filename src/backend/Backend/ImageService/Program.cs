using Carter;
using ImageService.DataAccess;
using ImageService.Interactors.GetById;
using ImageService.Interactors.Upload;
using ImageService.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Swagger (��� ������������ � ������ ����� Ocelot)
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
    options.UseNpgsql(builder.Configuration.GetConnectionString("Images"));
});

// Carter (��������������� �������������)
builder.Services.AddCarter();

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

// ����������� � �������
builder.Services.AddScoped<UploadImageInteractor>();
builder.Services.AddScoped<GetImageByIdInteractor>();
builder.Services.AddScoped<ImageValidator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options => options.AddPolicy("AllowAll", policy =>
{
    policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
}));

builder.Services.AddAuthentication();

var app = builder.Build();

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Image Service v1");
    });
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

// ������� � �����������
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowAll");
// Carter endpoints
app.MapCarter();

app.Run();
