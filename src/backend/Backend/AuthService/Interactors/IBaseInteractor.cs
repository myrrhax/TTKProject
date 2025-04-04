using AuthService.Utils;
using CSharpFunctionalExtensions;

namespace AuthService.Interactors;

public interface IBaseInteractor<TParam, TResult>
{
    Task<Result<TResult, ErrorsContainer>> ExecuteAsync(TParam param);
}
