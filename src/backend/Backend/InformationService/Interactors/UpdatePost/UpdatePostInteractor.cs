using CSharpFunctionalExtensions;
using InformationService.DataAccess;
using InformationService.Entities;
using InformationService.Utils;
using System;

namespace InformationService.Interactors.UpdatePost;

public class UpdatePostInteractor : IBaseInteractor<UpdatePostParams, Post>
{
    private readonly ApplicationContext _context;

    public UpdatePostInteractor(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<Result<Post, ErrorsContainer>> ExecuteAsync(UpdatePostParams param)
    {
        var validator = new UpdatePostParamsValidator();
        var validationResult = validator.Validate(param);

        if (!validationResult.IsValid)
        {
            var validationErrors = new ErrorsContainer(validationResult.Errors);
            return Result.Failure<Post, ErrorsContainer>(validationErrors);
        }

        var errors = new ErrorsContainer();
        var entity = await _context.Posts.FindAsync(param.PostId);

        if (entity is null)
        {
            errors.AddError("PostId", "Пост не найден");
            return Result.Failure<Post, ErrorsContainer>(errors);
        }
        var newHistory = new PostHistory
        {
            PostId = entity.PostId,
            Title = entity.Title,
            RedactorId = param.EditorId,
            EditType = EditType.Edited,
            UpdateTime = DateTime.Now,
        };
        
        entity.Title = param.NewTitle ?? entity.Title;
        entity.Content = param.NewContent ?? entity.Content;
        entity.ImageId = param.NewImageId ?? entity.ImageId;
        entity.History.Add(newHistory);

        try
        {
            await _context.SaveChangesAsync();

            return Result.Success<Post, ErrorsContainer>(entity);
        }
        catch (Exception ex)
        {
            errors.AddError("History", "Не удалось добавить изменение");
            return Result.Failure<Post, ErrorsContainer>(errors);
        }
    }
}
