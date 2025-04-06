namespace TasksService.Contracts.History;

public class TaskHistoryResponse
{
    public Guid Id { get; set; }
    public Guid TaskId { get; set; }
    public Guid ChangedByUserId { get; set; }
    public DateTime ChangedAt { get; set; }
    public string ChangeType { get; set; } = null!;
    public string Description { get; set; } = null!;
}
