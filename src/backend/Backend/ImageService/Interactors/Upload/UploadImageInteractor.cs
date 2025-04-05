using ImageService.Contracts;
using ImageService.DataAccess;
using ImageService.Entities;
using ImageService.Utils;
using Microsoft.AspNetCore.Http;

namespace ImageService.Interactors.Upload;

public class UploadImageInteractor
{
    private readonly ApplicationContext _context;
    private readonly ImageValidator _validator;

    public UploadImageInteractor(ApplicationContext context, ImageValidator validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<ImageResponse?> ExecuteAsync(IFormFile file, string uploadPath)
    {
        if (!_validator.IsImage(file))
            return null;

        var imageId = Guid.NewGuid();
        var fileExtension = Path.GetExtension(file.FileName);
        var fileName = $"{imageId}{fileExtension}";
        var filePath = Path.Combine(uploadPath, fileName);

        Directory.CreateDirectory(uploadPath); // если нет папки — создадим

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var image = new ImageFile
        {
            Id = imageId,
            FilePath = Path.Combine("uploads", fileName) // относительный путь
        };

        _context.ImageFiles.Add(image); // <-- поправлено
        await _context.SaveChangesAsync();

        return new ImageResponse
        {
            Id = image.Id,
            FilePath = image.FilePath // <-- поправлено
        };
    }
}
