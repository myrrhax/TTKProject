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
using System.Security.Claims;


namespace TasksService.Endpoints.Task;

public class TaskEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/tasks", async (CreateTaskRequest request, CreateTaskInteractor interactor, ClaimsPrincipal claims) =>
        {
            var param = new CreateTaskParam
            {
                Title = request.Title,
                Creator = GetUserId(claims),
                Description = request.Description,
                DueDate = request.DueDate,
                Priority = request.Priority,
            };
            var result = await interactor.ExecuteAsync(param);
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(result.Error.GetAll());
        }).RequireAuthorization().WithOpenApi();

        app.MapPut("/tasks", async (UpdateTaskRequest request, UpdateTaskInteractor interactor, ClaimsPrincipal claims) =>
        {
            var param = new UpdateTaskParams()
            {
                Id = request.Id,
                Title = request.Title,
                AssignedUserId = GetUserId(claims),
                DueDate = request.DueDate,
                Status = request.Status,
                Priority = request.Priority,
                Description = request.Description,
            };
            var result = await interactor.ExecuteAsync(param);
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(result.Error.GetAll());
        }).RequireAuthorization().WithOpenApi();

        app.MapDelete("/tasks/{id:guid}", async (Guid id, DeleteTaskInteractor interactor, ClaimsPrincipal claims) =>
        {
            var param = new DeleteTaskParams()
            {
                TaskId = id,
                RedactorId = GetUserId(claims)
            };
            var result = await interactor.ExecuteAsync(param);
            return result.IsSuccess
                ? Results.Ok()
                : Results.BadRequest(result.Error.GetAll());
        }).RequireAuthorization().WithOpenApi();

        app.MapPatch("/tasks/status", async (ChangeTaskStatusRequest request, ChangeTaskStatusInteractor interactor, ClaimsPrincipal claims) =>
        {
            var param = new ChangeTaskStatusParams()
            {
                TaskId = request.TaskId,
                NewStatus = request.NewStatus,
                RedactorId = GetUserId(claims)
            };
            var result = await interactor.ExecuteAsync(param);
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(result.Error.GetAll());
        }).RequireAuthorization().WithOpenApi();

        app.MapGet("/tasks/status/{status}", async (string status, GetTasksByStatusInteractor interactor) =>
        {
            var result = await interactor.ExecuteAsync(status);
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(result.Error.GetAll());
        }).RequireAuthorization().WithOpenApi();

        app.MapGet("/tasks/priority/{priority}", async (TaskPriority priority, ApplicationContext db) =>
        {
            var tasks = await db.Tasks
                .Where(t => t.Priority == priority)
                .ToListAsync();

            return Results.Ok(tasks);
        }).RequireAuthorization().WithOpenApi();
    }

    private Guid GetUserId(ClaimsPrincipal claims)
    {
        return Guid.Parse(claims.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
    }
}