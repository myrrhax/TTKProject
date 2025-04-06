namespace AuthService.Contracts.EditUser;

public record EditUserRequest(Guid UserId, 
    string? Login, 
    string? Name,
    string? Surname,
    string? SecondName);
