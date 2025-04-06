namespace InformationService.Contracts;

public class PostDto
{
    public Guid PostId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreationTime { get; set; }
    public Guid? ImageId { get; set; }
    public Guid CreatorId { get; set; }
    public List<HistoryDto> History { get; set; } = new();

    public PostDto(
        Guid postId,
        string title,
        string content,
        DateTime creationTime,
        Guid? imageId,
        Guid creatorId,
        List<HistoryDto> history)
    {
        PostId = postId;
        Title = title;
        Content = content;
        CreationTime = creationTime;
        ImageId = imageId;
        CreatorId = creatorId;
        History = history;
    }

    // Пустой конструктор для сериализации (например, JSON)
    public PostDto() { }
}
