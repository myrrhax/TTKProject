using Carter;
using InformationService.Contracts;
using InformationService.Entities;
using InformationService.Interactors;
using InformationService.Interactors.GetHistory;
using InformationService.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace InformationService.Endpoints;

public class HistoryEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/history");
        group.MapGet("", GetHistory);
    }

    public async Task<Results<Ok<List<HistoryDto>>, NotFound>> GetHistory(IBaseInteractor<GetHistoryParams, IEnumerable<PostHistory>> interactor,
        [FromQuery] int page = 1,
        [FromQuery] string query = "",
        [FromQuery] string orderBy = "desc")
    {
        var sortType = orderBy == "asc" ? DateSortType.Ascending : DateSortType.Descending;

        var param = new GetHistoryParams(page, query, sortType);
        var result = await interactor.ExecuteAsync(param);

        if (result.IsSuccess)
        {
            var dtos = new List<HistoryDto>();
            foreach (var history in result.Value)
            {
                dtos.Add(history);
            }

            return TypedResults.Ok(dtos);
        }

        return TypedResults.NotFound();
    }
}
