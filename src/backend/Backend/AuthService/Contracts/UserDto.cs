namespace AuthService.Contracts;

public class UserDto
{
    public Guid UserId { get; set; }
    public string Login { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; }
    public bool IsDeleted { get; set; }
    public Guid? AvatarId { get; set; }
}
