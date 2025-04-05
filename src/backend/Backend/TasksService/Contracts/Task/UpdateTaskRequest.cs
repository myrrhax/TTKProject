namespace TasksService.Contracts.Task;

public class UpdateTaskRequest
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime? DueDate { get; set; }
    public string Priority { get; set; } = "Medium";
    public Guid AssignedUserId { get; set; }
}
