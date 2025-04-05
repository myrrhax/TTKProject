namespace TasksService.Entities
{
    // Entities/Task.cs
    public class Task
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }

        public TaskPriority Priority { get; set; }
        public TaskStatus Status { get; set; }

        public Guid AssignedUserId { get; set; } // из AuthService
    }
}
