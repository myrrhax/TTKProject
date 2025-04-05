using AuthService.DataAccess;
using AuthService.Utils;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Interactors.DeleteUser;

public class DeleteUserInteractor(ApplicationContext context) : IBaseInteractor<DeleteUserParams, bool>
{
    public async Task<Result<bool, ErrorsContainer>> ExecuteAsync(DeleteUserParams param)
    {
        var user = await context.Users
            .FindAsync(param.Id);
        var errors = new ErrorsContainer();

        if (user is null || user.IsDeleted)
        {
            errors.AddError("UserId", "Пользователь не найден");
            return Result.Failure<bool, ErrorsContainer>(errors);
        }

        using var transaction = context.Database.BeginTransaction();
        try
        {
            user.IsDeleted = true;
            await context.RefreshTokens
                .Where(r => r.UserId == param.Id)
                .ExecuteDeleteAsync();

            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Result.Success<bool, ErrorsContainer>(true);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
        }
        errors.AddError("UserId", "Не удалось удалить пользователя");
        return Result.Failure<bool, ErrorsContainer>(errors);
    }
}
