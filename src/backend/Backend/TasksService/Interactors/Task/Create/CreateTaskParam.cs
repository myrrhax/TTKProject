namespace TasksService.Interactors.Task.Create;

public class CreateTaskParam
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime? DueDate { get; set; }
    public string Priority { get; set; } = "Medium"; // строкой: Low, Medium, High
    public Guid Creator { get; set; }
}
