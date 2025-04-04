namespace AuthService.Interactors.Register;

public record RegisterParams(string Login, string Password, string Name, string Surname, string? SecondName);