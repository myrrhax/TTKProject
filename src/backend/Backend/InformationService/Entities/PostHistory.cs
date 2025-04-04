namespace InformationService.Entities;

public class PostHistory
{
    public Guid UpdateId { get; set; }
    public Guid PostId { get; set; }
    public string Title { get; set; } = string.Empty;
    public Guid RedactorId { get; set; }
    public EditType EditType { get; set; }
    public DateTime UpdateTime { get; set; } = DateTime.UtcNow;

    public Post Post { get; set; } = null!;
}

public enum EditType
{
    Created, Edited, Deleted
}
