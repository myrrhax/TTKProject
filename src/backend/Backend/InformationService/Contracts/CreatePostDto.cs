namespace InformationService.Contracts;

public record CreatePostDto(string Title, string? Content = null, Guid? ImageId = null);