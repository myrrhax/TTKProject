using InformationService.Contracts;

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

    public static implicit operator HistoryDto(PostHistory postHistory)
    {
        string editTypeStr;
        switch (postHistory.EditType)
        {
            case EditType.Created:
                editTypeStr = "Создание";
                break;
            case EditType.Edited:
                editTypeStr = "Изменение";
                break;
            default:
                editTypeStr = "Удаление";
                break;
        }
        return new HistoryDto(postHistory.PostId,
            postHistory.Title,
            postHistory.RedactorId,
            editTypeStr,
            postHistory.UpdateTime);
    }
}

public enum EditType
{
    Created, Edited, Deleted
}
