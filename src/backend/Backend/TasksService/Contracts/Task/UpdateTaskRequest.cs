namespace TasksService.Contracts.Task;

public class UpdateTaskRequest
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime? DueDate { get; set; }

    public string Priority { get; set; } = null!; // Пример: "High", "Medium", "Low"

    public string Status { get; set; } = null!; // Пример: "Current", "Deferred", "Done"

}
