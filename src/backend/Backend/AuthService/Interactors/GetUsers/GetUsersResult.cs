using AuthService.Entities;

namespace AuthService.Interactors.GetUsers;

public record GetUsersResult(IEnumerable<ApplicationUser> Users, int MaxPages);
