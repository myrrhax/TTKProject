namespace AuthService.Interactors.ChangePassword;

public record ChangePasswordParams(Guid UserId, string Password);
