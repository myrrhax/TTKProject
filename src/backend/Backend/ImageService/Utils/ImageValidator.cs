using Microsoft.AspNetCore.Http;

namespace ImageService.Utils;

public class ImageValidator
{
    private readonly string[] _permittedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".bmp"];

    public bool IsImage(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return _permittedExtensions.Contains(extension);
    }
}
