using CSharpFunctionalExtensions;
using TasksService.Utils;

namespace TasksService.Interactors;

public interface IBaseInteractor<TParams, TResult>
{
    Task<Result<TResult, ErrorsContainer>> ExecuteAsync(TParams param);
}