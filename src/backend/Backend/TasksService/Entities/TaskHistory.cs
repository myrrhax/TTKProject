namespace TasksService.Entities
{
    // Entities/TaskHistory.cs
    public class TaskHistory
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public Guid ChangedByUserId { get; set; } // из AuthService
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
        public TaskChangeType ChangeType { get; set; }
        public string Description { get; set; } = null!;
    }
}
