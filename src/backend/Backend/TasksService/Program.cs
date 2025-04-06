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

// ✅ Нужно даже если авторизация пока не используется (иначе Ocelot/FluentValidator могут выбрасывать исключение)
builder.Services.AddAuthentication();

// EF Core
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
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

var app = builder.Build();

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TasksService API v1");
    });
}

app.MapCarter();

app.Run();
