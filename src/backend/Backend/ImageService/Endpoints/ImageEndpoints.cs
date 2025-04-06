using Carter;
using ImageService.Contracts;
using ImageService.Interactors.GetById;
using ImageService.Interactors.Upload;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.OpenApi;


namespace ImageService.Endpoints;

public class ImageEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/images", async (IFormFile file, UploadImageInteractor interactor) =>
        {
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

            var result = await interactor.ExecuteAsync(file, uploadPath);

            return result is not null
                ? Results.Ok(result)
                : Results.BadRequest("Невалидный файл");
        })
        .Accepts<IFormFile>("multipart/form-data")
        .Produces<ImageResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization()
        .WithOpenApi(); 

        app.MapGet("/images/{id:guid}", async (Guid id, GetImageByIdInteractor interactor) =>
        {
            return await interactor.ExecuteAsync(id);
        })
        .Produces<FileStreamResult>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi(); 
    }
}
