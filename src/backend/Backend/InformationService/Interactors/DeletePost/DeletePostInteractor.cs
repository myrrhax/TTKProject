using CSharpFunctionalExtensions;
using InformationService.DataAccess;
using InformationService.Entities;
using InformationService.Utils;
using Microsoft.EntityFrameworkCore;

namespace InformationService.Interactors.DeletePost;

public class DeletePostInteractor(ApplicationContext context) : IBaseInteractor<DeletePostParams, Guid>
{
    public async Task<Result<Guid, ErrorsContainer>> ExecuteAsync(DeletePostParams param)
    {
        var entity = await context
            .Posts
            .Include(p => p.History.OrderBy(ph => ph.UpdateTime))
            .FirstOrDefaultAsync(p => p.PostId == param.PostId);

        var errors = new ErrorsContainer();
        if (entity is null ||
            entity.History.LastOrDefault()?.EditType == EditType.Deleted)
        {
            errors.AddError("PostId", "Статья не найдена");

            return Result.Failure<Guid, ErrorsContainer>(errors);
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
            return Result.Success<Guid, ErrorsContainer>(entity.PostId);
        }
        catch (Exception)
        {
            errors.AddError("PostId", "Не удалось удалить пост");
            return Result.Failure<Guid, ErrorsContainer>(errors);
        }
    }
}
