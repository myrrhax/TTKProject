﻿using AuthService.DataAccess;
using AuthService.Utils;
using AuthService.Utils.JwtEncoder;
using AuthService.Utils.PasswordHasher;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AuthService.Interactors.RefreshToken;

public class RefreshTokenInteractor : IBaseInteractor<RefreshTokenParam, RefreshTokenResponse>
{
    private readonly ApplicationContext _context;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IPasswordHasher _hasher;
    private readonly IOptions<JwtConfig> _jwtConfig;

    public RefreshTokenInteractor(ApplicationContext context,
        IPasswordHasher hasher,
        ITokenGenerator tokenGenerator,
        IOptions<JwtConfig> jwtConfig)
    {
        _context = context;
        _tokenGenerator = tokenGenerator;
        _hasher = hasher;
        _jwtConfig = jwtConfig;
    }

    public async Task<Result<RefreshTokenResponse, ErrorsContainer>> ExecuteAsync(RefreshTokenParam param)
    {
        var entityToken = await _context.RefreshTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Token == param.RefreshToken);

        var errors = new ErrorsContainer();
        if (entityToken is null || DateTime.UtcNow >= entityToken.ExpirationDate)
        {
            errors.AddError("Token", "Токен не найден");
            return Result.Failure<RefreshTokenResponse, ErrorsContainer>(errors);
        }
            

        entityToken.Token = _tokenGenerator.GenerateRefreshToken();
        entityToken.ExpirationDate = DateTime.UtcNow.AddDays(7);
        await _context.SaveChangesAsync();

        string jwtToken = _tokenGenerator.GenerateAccessToken(entityToken.User);

        return new RefreshTokenResponse(jwtToken, entityToken.Token);
    }
}
