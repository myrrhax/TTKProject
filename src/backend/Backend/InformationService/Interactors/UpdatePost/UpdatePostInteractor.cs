using CSharpFunctionalExtensions;
using InformationService.DataAccess;
using InformationService.Entities;
using InformationService.Utils;

namespace InformationService.Interactors.UpdatePost;

public class UpdatePostInteractor : IBaseInteractor<UpdatePostParams, Post>
{
    private readonly ApplicationContext _context;

    public UpdatePostInteractor(ApplicationContext context)
    {
        _context = context;
    }

    public Task<Result<Post, ErrorsContainer>> ExecuteAsync(UpdatePostParams param)
    {
        
    }
}
