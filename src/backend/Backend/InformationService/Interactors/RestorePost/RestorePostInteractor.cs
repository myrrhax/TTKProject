using CSharpFunctionalExtensions;
using InformationService.DataAccess;
using InformationService.Utils;
using InformationService.Entities;
using Microsoft.EntityFrameworkCore;

namespace InformationService.Interactors.RestorePost;

public class RestorePostInteractor(ApplicationContext context) : IBaseInteractor<RestorePostParams, bool>
{
    public async Task<Result<bool, ErrorsContainer>> ExecuteAsync(RestorePostParams param)
    {
        var entity = await context.Posts
            .Include(p => p.History.OrderBy(ph => ph.UpdateTime))
            .FirstOrDefaultAsync(p => p.PostId == param.PostId);

        var errors = new ErrorsContainer();
        if (entity is null ||
            entity.History.LastOrDefault()?.EditType != EditType.Deleted)
        {
            errors.AddError("PostId", "Статья не найдена");
            return Result.Failure<bool, ErrorsContainer>(errors);
        }

        var history = new PostHistory
        {
            PostId = entity.PostId,
            Title = entity.Title,
            UpdateTime = DateTime.UtcNow,
            RedactorId = param.EditorId,
            EditType = EditType.Restored,
        };
        entity.History.Add(history);
        try
        {
            await context.SaveChangesAsync();
            return Result.Success<bool, ErrorsContainer>(true);
        }
        catch (Exception ex)
        {
            errors.AddError("PostId", "Не удалось восстановить статью");
            return Result.Failure<bool, ErrorsContainer>(errors);
        }
    }
}
