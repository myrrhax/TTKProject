namespace AuthService.Contracts.ChangePassword;

public record ChangePasswordRequest(Guid UserId, string Password);