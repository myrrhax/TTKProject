using AuthService.DataAccess;
using AuthService.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthService.Utils.JwtEncoder;

internal class TokenGenerator(ApplicationContext context, IOptions<JwtConfig> jwtConfig) : ITokenGenerator
{
    public string GenerateAccessToken(ApplicationUser user)
    {
        string roleStringify;

        switch (user.Role)
        {
            case Role.Admin:
                roleStringify = "admin";
                break;
            default:
                roleStringify = "user";
                break;
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Role, roleStringify),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Value.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtConfig.Value.Issuer,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwtConfig.Value.ExpirationTimeMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> CreateRefreshToken(Guid userId)
    {
        var entity = new RefreshToken
        {
            Token = GenerateRefreshToken(),
            ExpirationDate = DateTime.UtcNow.AddMinutes(jwtConfig.Value.RefreshTokenExpirationDays),
            UserId = userId
        };
        await context.RefreshTokens.AddAsync(entity);
        await context.SaveChangesAsync();

        return entity.Token;
    }

    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}
