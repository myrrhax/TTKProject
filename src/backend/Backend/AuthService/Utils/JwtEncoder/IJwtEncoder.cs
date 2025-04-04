using AuthService.Entities;

namespace AuthService.Utils.JwtEncoder;

public interface IJwtEncoder
{
    string GenerateAccessToken(ApplicationUser user);
    string GenerateRefreshToken();
}
