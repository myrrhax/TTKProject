using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebAPI.Configuration;

public static class ConfigureAuth
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var cfg = configuration.GetSection("JwtConfig").Get<JwtConfig>();
        if (cfg is null) throw new ArgumentException("Unable to read jwt settings from appsettings.json");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateLifetime = true,
                   ValidateAudience = false,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = cfg.Issuer,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cfg.SecretKey)),
               };
           });
        services.AddAuthorization();

        return services;
    }
}
