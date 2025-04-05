using AuthService.DataAccess;
using AuthService.Utils;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Interactors.ChangeRole;

public class ChangeRoleInteractor(ApplicationContext context) : IBaseInteractor<ChangeRoleParams, bool>
{
    public async Task<Result<bool, ErrorsContainer>> ExecuteAsync(ChangeRoleParams param)
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
            user.Role = param.Role;

            await context.RefreshTokens
                .Where(t => t.UserId == param.UserId)
                .ExecuteDeleteAsync();

            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Result.Success<bool, ErrorsContainer>(true);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            errors.AddError("UserId", "Не удалось изменить роль");
            return Result.Failure<bool, ErrorsContainer>(errors);
        }
    }
}
