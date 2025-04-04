namespace InformationService.Entities;

public class Post
{
    public Guid PostId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreationTime { get; set; } = DateTime.UtcNow;
    public Guid? ImageId { get; set; }
    public Guid CreatorId { get; set; }

    public List<PostHistory> History { get; set; } = [];
}
