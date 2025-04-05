namespace InformationService.Interactors.UpdatePost;

public class UpdatePostParams
{
    public Guid PostId { get; }
    public Guid EditorId { get; }
    public string? NewTitle { get; }
    public string? NewContent { get; }
    public Guid? NewImageId { get; }

    public UpdatePostParams(Guid postId, Guid editorId, string? newTitle, string? newContent, Guid? newImageId)
    {
        PostId = postId;
        EditorId = editorId;
        NewTitle = newTitle;
        NewContent = newContent;
        NewImageId = newImageId;
    }
}

