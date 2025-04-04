namespace InformationService.Interactors.UpdatePost;

public record UpdatePostParams(Guid PostId, Guid EditorId, string? NewTitle, string? NewContent, Guid? NewImageId);
