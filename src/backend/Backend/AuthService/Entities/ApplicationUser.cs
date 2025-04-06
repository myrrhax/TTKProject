using AuthService.Contracts;

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
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
    public Guid? AvatarId { get; set; } = null;

    public static implicit operator UserDto(ApplicationUser user)
    {
        string roleStr;
        
        switch(user.Role)
        {
            case Role.Admin:
                roleStr = "Админ";
                break;
            default:
                roleStr = "Пользователь";
                break;
        }

        return new UserDto
        {
            UserId = user.UserId,
            Login = user.Login,
            FullName = $"{user.Surname} {user.Name} {user.SecondName}",
            CreationDate = user.CreationDate,
            IsDeleted = user.IsDeleted,
            Role = roleStr,
        };
    }
}
