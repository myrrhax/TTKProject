namespace AuthService.Interactors.EditUser;

public record EditUserParams(Guid UserId, 
    string? Login, 
    string? Name, 
    string? Surname, 
    string? SecondName);
