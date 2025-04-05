using Microsoft.EntityFrameworkCore;
using TasksService.DataAccess;
using TasksService.Interactors.Task.Create;
using TasksService.Interactors.Task.Update;
using TasksService.Interactors.Task.Delete;
using TasksService.Interactors.Task.ChangeStatus;
using TasksService.Interactors.Task.GetByStatus;
using TasksService.Interactors.History.GetAll;
using TasksService.Utils;
using Carter;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // обязательно

// EF Core
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Tasks"));
});

// Carter
builder.Services.AddCarter();

// Интеракторы
builder.Services.AddScoped<CreateTaskInteractor>();
builder.Services.AddScoped<UpdateTaskInteractor>();
builder.Services.AddScoped<DeleteTaskInteractor>();
builder.Services.AddScoped<ChangeTaskStatusInteractor>();
builder.Services.AddScoped<GetTasksByStatusInteractor>();
builder.Services.AddScoped<GetTaskHistoryInteractor>();
builder.Services.AddScoped<TaskHistoryLogger>();

builder.Services.AddCors(options => options.AddPolicy("AllowAll", policy =>
{
    policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
}));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(); // Не забудь эту строку!

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
// Маршруты
app.MapCarter();

app.Run();
