using AuthService.DataAccess;
using AuthService.Utils;
using AuthService.Utils.PasswordHasher;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Interactors.ChangePassword;

public class ChangePasswordInteractor(ApplicationContext context, IPasswordHasher hasher) : IBaseInteractor<ChangePasswordParams, bool>
{
    public async Task<Result<bool, ErrorsContainer>> ExecuteAsync(ChangePasswordParams param)
    {
        var user = await context.Users.FindAsync(param.UserId);
        var errors = new ErrorsContainer();

        if (user is null)
        {
            errors.AddError("UserId", "Пользователь не найден");
            return Result.Failure<bool, ErrorsContainer>(errors);
        }

        var validator = new PasswordValidator();
        var validationResult = validator.Validate(param.Password);

        if (!validationResult.IsValid)
        {
            errors.AddValidationErrors(validationResult.Errors);
            return Result.Failure<bool, ErrorsContainer>(errors);
        }

        var hash = hasher.HashPassword(param.Password);
        using var transaction = context.Database.BeginTransaction();
        try
        {
            user.PasswordHash = hash;
            await context.RefreshTokens
                .Where(u => u.UserId == user.UserId)
                .ExecuteDeleteAsync();
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
            
            return Result.Success<bool, ErrorsContainer>(true);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            errors.AddError("UserId", "Не удалость изменить пароль");
            return Result.Failure<bool, ErrorsContainer>(errors);
        }

    }
}
