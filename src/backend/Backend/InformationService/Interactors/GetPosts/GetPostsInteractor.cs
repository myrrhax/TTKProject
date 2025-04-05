using CSharpFunctionalExtensions;
using InformationService.DataAccess;
using InformationService.Entities;
using InformationService.Utils;
using Microsoft.EntityFrameworkCore;

namespace InformationService.Interactors.GetPosts;

public class GetPostsInteractor : IBaseInteractor<GetPostsParams, GetPostsResult>
{
    public const int PAGE_SIZE = 10;
    private readonly ApplicationContext _context;

    public GetPostsInteractor(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<Result<GetPostsResult, ErrorsContainer>> ExecuteAsync(GetPostsParams param)
    {
        var query = _context.Posts
            .AsNoTracking()
            .Include(p => p.History.OrderByDescending(ph => ph.UpdateTime))
            .Where(p => p.History
                .OrderByDescending(ph => ph.UpdateTime)
                .First().EditType != EditType.Deleted)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(param.Query))
        {
            query = query.Where(p => EF.Functions.ILike(p.Title, $"%{param.Query}%")
                || p.Content.Contains(param.Query)
                || EF.Functions.ILike(p.PostId.ToString(), $"%{param.Query}%"));
        }

        if (param.DateSortType == DateSortType.Descending)
        {
            query = query.OrderByDescending(p => p.History
                .OrderByDescending(ph => ph.UpdateTime)
                .Last()
                .UpdateTime);
        }
        else
        {
            query = query.OrderBy(p => p.History.Last().UpdateTime);
        }

        var totalPostsCount = await query.CountAsync();
        var maxPages = (int)Math.Ceiling(totalPostsCount / (double)PAGE_SIZE);

        var posts = await query.Skip(PAGE_SIZE * (param.Page - 1))
            .Take(PAGE_SIZE)
            .ToListAsync();

        if (!posts.Any())
        {
            var errors = new ErrorsContainer();
            errors.AddError("Posts", "Посты не найдены");
            return Result.Failure<GetPostsResult, ErrorsContainer>(errors);
        }

        return Result.Success<GetPostsResult, ErrorsContainer>(new GetPostsResult
        {
            Posts = posts,
            MaxPages = maxPages
        });
    }
}

public class GetPostsResult
{
    public IEnumerable<Post> Posts { get; set; } = new List<Post>();
    public int MaxPages { get; set; }
}
