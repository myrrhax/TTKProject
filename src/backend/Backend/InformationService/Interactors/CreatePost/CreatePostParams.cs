namespace InformationService.Interactors.CreatePost;

public record CreatePostParams(string Title, Guid CreatorId, string? Content = null, Guid? ImageId = null);
