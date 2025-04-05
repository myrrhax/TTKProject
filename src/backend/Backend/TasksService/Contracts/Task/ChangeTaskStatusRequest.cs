namespace TasksService.Contracts.Task;

public class ChangeTaskStatusRequest
{
    public Guid TaskId { get; set; }
    public string NewStatus { get; set; } = null!; // "Deferred", "Done", "Current"
}
