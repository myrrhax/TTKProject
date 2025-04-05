using AuthService.DataAccess;
using AuthService.Utils;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Interactors.EditUser;

public class EditUserInteractor(ApplicationContext context) : IBaseInteractor<EditUserParams, bool>
{
    public async Task<Result<bool, ErrorsContainer>> ExecuteAsync(EditUserParams param)
    {
        var user = await context.Users.FindAsync(param.UserId);
        var errors = new ErrorsContainer();

        if (user is null)
        {
            errors.AddError("UserId", "Пользователь не найден");
            return Result.Failure<bool, ErrorsContainer>(errors);
        }

        using var transaction = context.Database.BeginTransaction();
        try
        {
            user.Name = param.Name ?? user.Name;
            user.Login = param.Login ?? user.Login;
            user.Surname = param.Surname ?? user.Surname;
            user.SecondName = param.SecondName ?? user.SecondName;

            var sessions = await context.RefreshTokens
                .Where(u => u.UserId == user.UserId)
                .ExecuteDeleteAsync();

            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Result.Success<bool, ErrorsContainer>(true);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            errors.AddError("UserId", "Не удалось обновить информацию о пользователе");
            return Result.Failure<bool, ErrorsContainer>(errors);
        }
    }
}
