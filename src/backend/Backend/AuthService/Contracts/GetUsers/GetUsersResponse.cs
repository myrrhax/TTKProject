namespace AuthService.Contracts.GetUsers;

public record GetUsersResponse(IEnumerable<UserDto> Users, int MaxPages);
