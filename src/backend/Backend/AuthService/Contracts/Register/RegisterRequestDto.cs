namespace AuthService.Contracts.Register;

public record RegisterRequestDto(string Login, string Password, string Name, string Surname, string? SecondName, string? UserId, string? AvatarId);