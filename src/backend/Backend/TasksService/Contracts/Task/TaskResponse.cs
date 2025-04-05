namespace TasksService.Contracts.Task;

public class TaskResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public string Priority { get; set; } = null!;
    public string Status { get; set; } = null!;
    public Guid AssignedUserId { get; set; }
}
