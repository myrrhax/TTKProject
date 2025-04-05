using InformationService.Contracts;

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

    public static implicit operator PostDto(Post post)
    {
        var dtos = new List<HistoryDto>();
        foreach (var postHistory in post.History)
        {
            dtos.Add(postHistory);
        }
        return new PostDto(post.PostId, post.Title, post.Content, post.CreationTime, post.ImageId, post.CreatorId, dtos);
    }
}
