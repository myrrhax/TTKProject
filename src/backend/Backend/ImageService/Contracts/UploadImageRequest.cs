using Microsoft.AspNetCore.Http;

namespace ImageService.Contracts;

public class UploadImageRequest
{
    public IFormFile File { get; set; } = null!;
}
