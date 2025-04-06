using Carter;
using Microsoft.AspNetCore.Routing;
using TasksService.Interactors.History.GetAll;
using TasksService.Utils;

namespace TasksService.Endpoints.History;

public class HistoryEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/tasks/history", async (Guid? taskId, GetTaskHistoryInteractor interactor) =>
        {
            var result = await interactor.ExecuteAsync(taskId);
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(result.Error.GetAll());
        }).RequireAuthorization();
    }
}