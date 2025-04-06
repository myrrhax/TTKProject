using AuthService.DataAccess;
using AuthService.Entities;
using AuthService.Utils;
using CSharpFunctionalExtensions;

namespace AuthService.Interactors.GetUserDetails;

public class GetUserDetailsInteractor(ApplicationContext context) : IBaseInteractor<Guid, ApplicationUser>
{
    public async Task<Result<ApplicationUser, ErrorsContainer>> ExecuteAsync(Guid param)
    {
        var user = await context.Users.FindAsync(param);
        var errors = new ErrorsContainer();

        if (user is null)
        {
            errors.AddError("UserId", "Пользователь не найден");
            return Result.Failure<ApplicationUser, ErrorsContainer>(errors);
        }
        return Result.Success<ApplicationUser, ErrorsContainer>(user);
    }
}
