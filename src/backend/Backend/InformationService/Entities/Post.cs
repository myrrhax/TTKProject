namespace InformationService.Entities;

public class Post
{
    public Guid PostId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreationTime { get; set; } = DateTime.UtcNow;
    public DateTime LastUpdateTime { get; set; } = DateTime.UtcNow;
    public Guid LastRedactorId { get; set; }
    public Guid ImageId { get; set; }
    public EventType EventType { get; set; } = EventType.Created;
}

public enum EventType
{
    Created, Updated, Deleted
}
