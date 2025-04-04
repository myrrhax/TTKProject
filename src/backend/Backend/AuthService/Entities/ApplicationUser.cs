namespace AuthService.Entities;

public class ApplicationUser
{
    public Guid UserId { get; set; }
    public string Login { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string? SecondName { get; set; }
    public Role Role { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public bool IsDeleted { get; set; } = false;
}
