using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
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
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Tasks Service",
        Version = "v1"
    });
});


builder.Services.AddAuthentication();

builder.Services.AddJwtAuthentication(builder.Configuration);
// EF Core
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Tasks"));
});

// Carter
builder.Services.AddCarter();

// �����������
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




    if (app.Environment.IsProduction())
    {
        var context = app.Services.GetRequiredService<ApplicationContext>();
        var migrations = await context.Database.GetPendingMigrationsAsync();
        if (migrations.Any())
        {
            await context.Database.MigrateAsync();
        }
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "TasksService API v1");
        });
    }

    app.UseCors("AllowAll");
    app.MapCarter();
    app.UseCors("AllowAll");
    app.Run();
