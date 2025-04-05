using CSharpFunctionalExtensions;
using InformationService.DataAccess;
using InformationService.Entities;
using InformationService.Utils;
using Microsoft.EntityFrameworkCore;

namespace InformationService.Interactors.GetPost;

public class GetPostInteractor(ApplicationContext context) : IBaseInteractor<Guid, Post>
{
    public async Task<Result<Post, ErrorsContainer>> ExecuteAsync(Guid param)
    {
        var entity = await context.Posts
            .AsNoTracking()
            .Include(p => p.History)
            .FirstOrDefaultAsync(p => p.PostId == param);

        if (entity is null)
        {
            var errors = new ErrorsContainer();
            errors.AddError("PostId", "Пост не найден");
            return Result.Failure<Post, ErrorsContainer>(errors);
        }

        return Result.Success<Post, ErrorsContainer>(entity);
    }
}
