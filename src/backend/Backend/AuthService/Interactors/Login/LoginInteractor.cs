using AuthService.DataAccess;
using AuthService.Utils;
using AuthService.Utils.JwtEncoder;
using AuthService.Utils.PasswordHasher;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AuthService.Interactors.Login;

public class LoginInteractor : IBaseInteractor<LoginParams, LoginResult>
{
    private readonly ApplicationContext _context;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IPasswordHasher _hasher;
    private readonly IOptions<JwtConfig> _jwtConfig;

    public LoginInteractor(ApplicationContext context,
        IPasswordHasher hasher,
        ITokenGenerator tokenGenerator,
        IOptions<JwtConfig> jwtConfig)
    {
        _context = context;
        _tokenGenerator = tokenGenerator;
        _hasher = hasher;
        _jwtConfig = jwtConfig;
    }

    public async Task<Result<LoginResult, ErrorsContainer>> ExecuteAsync(LoginParams param)
    {
        var validator = new LoginParamsValidation();

        var validationResult = validator.Validate(param);

        if (!validationResult.IsValid)
        {
            var validationErrors = new ErrorsContainer(validationResult.Errors);
            return Result.Failure<LoginResult, ErrorsContainer>(validationErrors);
        }

        var entity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Login == param.Login);

        var errors = new ErrorsContainer();
        if (entity is null)
        {
            errors.AddError("Login", $"Пользователь с логином: {param.Login} не найден");
            return Result.Failure<LoginResult, ErrorsContainer>(errors);
        }

        if (!_hasher.VerifyPassword(param.Password, entity.PasswordHash))
        {
            errors.AddError("Password", "Неправильный логин или пароль");
            return Result.Failure<LoginResult, ErrorsContainer>(errors);
        }

        var token = _tokenGenerator.GenerateAccessToken(entity);

        int workingSessionsCount = await _context.RefreshTokens
            .Where(t => DateTime.UtcNow < t.ExpirationDate)
            .CountAsync();

        if (workingSessionsCount >= _jwtConfig.Value.RefreshTokenMaxSessionsCount)
        {
            errors.AddError("Security", "На вашем аккаунте обнаружена подозрительная активность");

            await DropSessions(entity.UserId);
            return Result.Failure<LoginResult, ErrorsContainer>(errors);
        }

        var refreshToken = await _tokenGenerator.CreateRefreshToken(entity.UserId);
        var response = new LoginResult(token, refreshToken);

        return Result.Success<LoginResult, ErrorsContainer>(response);
    }

    private async Task DropSessions(Guid userId)
    {
        await _context.RefreshTokens
            .Where(u => u.UserId == userId)
            .ExecuteDeleteAsync();
    }
}
