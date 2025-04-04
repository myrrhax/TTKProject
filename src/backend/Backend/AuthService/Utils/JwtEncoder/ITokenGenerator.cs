using AuthService.Entities;

namespace AuthService.Utils.JwtEncoder;

public interface ITokenGenerator
{
    string GenerateAccessToken(ApplicationUser user);
    string GenerateRefreshToken();
    Task<string> CreateRefreshToken(Guid userId);
}
