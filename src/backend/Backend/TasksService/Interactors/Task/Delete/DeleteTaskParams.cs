namespace TasksService.Interactors.Task.Delete;

public class DeleteTaskParams
{
    public Guid TaskId { get; set; }
    public Guid RedactorId { get; set; }
}
