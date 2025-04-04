using CSharpFunctionalExtensions;
using InformationService.DataAccess;
using InformationService.Entities;
using InformationService.Utils;
using Microsoft.EntityFrameworkCore;

namespace InformationService.Interactors.GetPosts;

public class GetPostsInteractor : IBaseInteractor<GetPostsParams, IEnumerable<Post>>
{
    public const int PAGE_SIZE = 25;
    private readonly ApplicationContext _context;

    public GetPostsInteractor(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<Post>, ErrorsContainer>> ExecuteAsync(GetPostsParams param)
    {
        var query = _context.Posts
            .AsNoTracking()
            .Include(p => p.History)
            .AsQueryable();
        if (param.Query != string.Empty)
        {
            query = query.Where(p => EF.Functions.ILike(p.Title, $"%{param.Query}%")
                || p.Content.Contains(param.Query) 
                || EF.Functions.ILike(p.PostId.ToString(), $"%{param.Query}%"));
        }

        if (param.DateSortType == DateSortType.Descending)
        {
            query = query.OrderByDescending(p => p.History.Last().UpdateTime);
        }
        else
        {
            query = query.OrderBy(p => p.History.Last().UpdateTime);
        }

        var posts = await query.Skip(PAGE_SIZE * (param.Page - 1))
            .Take(PAGE_SIZE)
            .ToListAsync();
        
        if (!posts.Any())
        {
            var errors = new ErrorsContainer();
            errors.AddError("Posts", "Посты не найдены");
            return Result.Failure<IEnumerable<Post>, ErrorsContainer>(errors);
        }

        return Result.Success<IEnumerable<Post>, ErrorsContainer>(posts);
    }
}
