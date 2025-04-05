using Carter;
using Microsoft.AspNetCore.Routing;
using TasksService.Contracts.Task;
using TasksService.Interactors;
using TasksService.Interactors.Task.Create;
using TasksService.Interactors.Task.Update;
using TasksService.Interactors.Task.Delete;
using TasksService.Interactors.Task.GetByStatus;
using TasksService.Interactors.Task.ChangeStatus;
using TasksService.Utils;
using TasksService.Entities;
using TasksService.DataAccess;
using Microsoft.EntityFrameworkCore;


namespace TasksService.Endpoints.Task;

public class TaskEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/tasks", async (CreateTaskRequest request, CreateTaskInteractor interactor) =>
        {
            var result = await interactor.ExecuteAsync(request);
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(result.Error.GetAll());
        });

        app.MapPut("/tasks", async (UpdateTaskRequest request, UpdateTaskInteractor interactor) =>
        {
            var result = await interactor.ExecuteAsync(request);
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(result.Error.GetAll());
        });

        app.MapDelete("/tasks/{id:guid}", async (Guid id, DeleteTaskInteractor interactor) =>
        {
            var result = await interactor.ExecuteAsync(id);
            return result.IsSuccess
                ? Results.Ok()
                : Results.BadRequest(result.Error.GetAll());
        });

        app.MapPatch("/tasks/status", async (ChangeTaskStatusRequest request, ChangeTaskStatusInteractor interactor) =>
        {
            var result = await interactor.ExecuteAsync(request);
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(result.Error.GetAll());
        });

        app.MapGet("/tasks/status/{status}", async (string status, GetTasksByStatusInteractor interactor) =>
        {
            var result = await interactor.ExecuteAsync(status);
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(result.Error.GetAll());
        });

        app.MapGet("/tasks/priority/{priority}", async (TaskPriority priority, ApplicationContext db) =>
        {
            var tasks = await db.Tasks
                .Where(t => t.Priority == priority)
                .ToListAsync();

            return Results.Ok(tasks);
        });
    }
}