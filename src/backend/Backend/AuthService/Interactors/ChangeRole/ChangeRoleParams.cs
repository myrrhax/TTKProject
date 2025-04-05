using AuthService.Entities;

namespace AuthService.Interactors.ChangeRole;

public record ChangeRoleParams(Guid UserId, Role Role);
