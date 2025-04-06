using CSharpFunctionalExtensions;
using InformationService.DataAccess;
using InformationService.Entities;
using InformationService.Utils;

namespace InformationService.Interactors.CreatePost;

public class CreatePostInteractor : IBaseInteractor<CreatePostParams, Post>
{
    private readonly ApplicationContext _context;

    public CreatePostInteractor(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<Result<Post, ErrorsContainer>> ExecuteAsync(CreatePostParams param)
    {
        var validator = new CreatePostParamsValidator();
        var validationResult = validator.Validate(param);

        if (!validationResult.IsValid)
        {
            var errors = new ErrorsContainer(validationResult.Errors);

            return Result.Failure<Post, ErrorsContainer>(errors);
        }
        var entity = new Post
        {
            PostId = Guid.NewGuid(),
            ImageId = param.ImageId,
            Title = param.Title,
            CreatorId = param.CreatorId,
            Content = param.Content ?? string.Empty,
            CreationTime = DateTime.UtcNow
        };
        var history = new PostHistory
        {
            PostId = entity.PostId,
            Title = entity.Title,
            RedactorId = entity.CreatorId,
            EditType = EditType.Created,
            UpdateTime = DateTime.UtcNow,
            Post = entity,
        };
        entity.History.Add(history);
        try
        {
            await _context.Posts.AddAsync(entity);
            await _context.SaveChangesAsync();

            return Result.Success<Post, ErrorsContainer>(entity);
        }
        catch (Exception ex)
        {
            var errors = new ErrorsContainer();
            errors.AddError("Title", $"Статья с названием {param.Title} уже существует");
            return Result.Failure<Post, ErrorsContainer>(errors);
        }
    }
}
