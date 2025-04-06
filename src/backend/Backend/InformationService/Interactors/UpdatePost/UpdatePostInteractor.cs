using CSharpFunctionalExtensions;
using InformationService.DataAccess;
using InformationService.Entities;
using InformationService.Utils;
using Microsoft.EntityFrameworkCore;
using System;

namespace InformationService.Interactors.UpdatePost;

public class UpdatePostInteractor : IBaseInteractor<UpdatePostParams, Guid>
{
    private readonly ApplicationContext _context;

    public UpdatePostInteractor(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid, ErrorsContainer>> ExecuteAsync(UpdatePostParams param)
    {
        var validator = new UpdatePostParamsValidator();
        var validationResult = validator.Validate(param);

        if (!validationResult.IsValid)
        {
            var validationErrors = new ErrorsContainer(validationResult.Errors);
            return Result.Failure<Guid, ErrorsContainer>(validationErrors);
        }

        var errors = new ErrorsContainer();
        var entity = await _context.Posts
            .Include(p => p.History.OrderBy(ph => ph.UpdateTime))
            .FirstOrDefaultAsync(p => p.PostId == param.PostId);

        if (entity is null
            || entity.History.LastOrDefault()?.EditType == EditType.Deleted)
        {
            errors.AddError("PostId", "Статья не найдена");
            return Result.Failure<Guid, ErrorsContainer>(errors);
        }

        var newHistory = new PostHistory
        {
            PostId = entity.PostId,
            Title = entity.Title,
            RedactorId = param.EditorId,
            EditType = EditType.Edited,
            UpdateTime = DateTime.UtcNow,
        };
        
        entity.Title = param.NewTitle ?? entity.Title;
        entity.Content = param.NewContent ?? entity.Content;
        if (param.NewImageId.HasValue)
        {
            entity.ImageId = param.NewImageId.Value;
        }
        entity.History.Add(newHistory);

        try
        {
            await _context.SaveChangesAsync();

            return Result.Success<Guid, ErrorsContainer>(entity.PostId);
        }
        catch (Exception ex)
        {
            errors.AddError("History", "Не удалось добавить изменение");
            return Result.Failure<Guid, ErrorsContainer>(errors);
        }
    }
}
