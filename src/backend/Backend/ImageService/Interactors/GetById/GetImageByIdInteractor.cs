using ImageService.DataAccess;
using ImageService.Entities;
using Microsoft.AspNetCore.Http;

namespace ImageService.Interactors.GetById;

public class GetImageByIdInteractor(ApplicationContext context)
{
    public async Task<IResult> ExecuteAsync(Guid id)
    {
        var image = await context.ImageFiles.FindAsync(id);

        if (image is null || string.IsNullOrWhiteSpace(image.FilePath) || !File.Exists(image.FilePath))
        {
            return Results.NotFound("Картинка не найдена.");
        }

        var contentType = GetContentType(image.FilePath);
        var fileStream = new FileStream(image.FilePath, FileMode.Open, FileAccess.Read);
        return Results.File(fileStream, contentType);
    }

    private static string GetContentType(string path)
    {
        var ext = Path.GetExtension(path).ToLowerInvariant();
        return ext switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            _ => "application/octet-stream"
        };
    }
}
