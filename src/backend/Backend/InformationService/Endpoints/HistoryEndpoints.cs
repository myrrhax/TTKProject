using Carter;
using InformationService.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;

namespace InformationService.Endpoints;

public class HistoryEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/history");
        group.MapGet("", GetHistory);
    }

    public async Task<Results<Ok<IEnumerable<HistoryDto>>, NotFound>> GetHistory()
    {
        
    }
}
