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
builder.Services.AddSwaggerGen(); // �����������

// EF Core
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // �� ������ ��� ������!
}

// ��������
app.MapCarter();

app.Run();
