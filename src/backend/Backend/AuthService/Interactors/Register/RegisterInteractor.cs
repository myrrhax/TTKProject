using AuthService.DataAccess;
using AuthService.Entities;
using AuthService.Utils;
using AuthService.Utils.JwtEncoder;
using AuthService.Utils.PasswordHasher;
using CSharpFunctionalExtensions;
using System.Collections.Generic;

namespace AuthService.Interactors.Register;

public class RegisterInteractor : IBaseInteractor<RegisterParams, RegisterResult>
{
    private readonly IPasswordHasher _hasher; 
    private readonly ITokenGenerator _tokenGenerator;
    private readonly ApplicationContext _context;

    public RegisterInteractor(ApplicationContext context, IPasswordHasher hasher, ITokenGenerator tokenGenerator)
    {
        _hasher = hasher;
        _tokenGenerator = tokenGenerator;
        _context = context;
    }

    public async Task<Result<RegisterResult, ErrorsContainer>> ExecuteAsync(RegisterParams param)
    {
        var errors = new ErrorsContainer();
        if (!Guid.TryParse(param.AvatarId, out var avatarId))
        {
            errors.AddError("AvatarId", "Неверный формат идентификатора");
            return Result.Failure<RegisterResult, ErrorsContainer>(errors);
        } 

        var entity = new ApplicationUser
        {
            Login = param.Login,
            Name = param.Name,
            Surname = param.Surname,
            SecondName = param.SecondName,
            PasswordHash = _hasher.HashPassword(param.Password),
            AvatarId = avatarId,
        };
        try
        {
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();

            var token = _tokenGenerator.GenerateAccessToken(entity);
            var refreshToken = await _tokenGenerator.CreateRefreshToken(entity.UserId);
            var response = new RegisterResult(token, refreshToken);

            return Result.Success<RegisterResult, ErrorsContainer>(response);
        }
        catch (Exception ex)
        {
            
            errors.AddError("Login", "Пользователь с данным логином уже существует");
            return Result.Failure<RegisterResult, ErrorsContainer>(errors);
        }
    }
}
