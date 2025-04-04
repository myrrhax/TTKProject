using CSharpFunctionalExtensions;
using InformationService.DataAccess;
using InformationService.Entities;
using InformationService.Utils;

namespace InformationService.Interactors.DeletePost;

public class DeletePostInteractor(ApplicationContext context) : IBaseInteractor<DeletePostParams, Post>
{
    public async Task<Result<Post, ErrorsContainer>> ExecuteAsync(DeletePostParams param)
    {
        var entity = await context.FindAsync<Post>(param.PostId);

        if (entity is null)
        {
            var errors = new ErrorsContainer();
            errors.AddError("PostId", "Статья не найдена");

            return Result.Failure<Post, ErrorsContainer>(errors);
        }
        var history = new PostHistory
        {
            PostId = entity.PostId,
            Title = entity.Title,
            RedactorId = param.RedactorId,
            UpdateTime = DateTime.UtcNow,
            EditType = EditType.Deleted,
        };
        entity.History.Add(history);

        try
        {
            await context.SaveChangesAsync();
            return Result.Success<Post, ErrorsContainer>(entity);
        }
        catch (Exception)
        {
            var errors = new ErrorsContainer();
            errors.AddError("PostId", "Не удалось удалить пост");
            return Result.Failure<Post, ErrorsContainer>(errors);
        }
    }
}
