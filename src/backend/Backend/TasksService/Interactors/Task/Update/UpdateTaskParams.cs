﻿namespace TasksService.Interactors.Task.Update;

public class UpdateTaskParams
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime? DueDate { get; set; }
    public string Priority { get; set; } = null!; // Пример: "High", "Medium", "Low"
    public string Status { get; set; } = null!; // Пример: "Current", "Deferred", "Done"
    public Guid AssignedUserId { get; set; }
}
