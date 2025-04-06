using AuthService.Entities;

namespace AuthService.Contracts.ChangeRole;

public record ChangeRoleRequest(Guid UserId, string Role = "user");
