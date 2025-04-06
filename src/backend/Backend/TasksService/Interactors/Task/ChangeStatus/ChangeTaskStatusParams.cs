namespace TasksService.Interactors.Task.ChangeStatus;

public class ChangeTaskStatusParams
{
    public Guid TaskId { get; set; }
    public Guid RedactorId { get; set; }
    public string NewStatus { get; set; } = null!;
}
