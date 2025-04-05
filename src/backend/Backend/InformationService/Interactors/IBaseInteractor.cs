using CSharpFunctionalExtensions;
using InformationService.Utils;

namespace InformationService.Interactors;

public interface IBaseInteractor<TParam, TResult>
{
    Task<Result<TResult, ErrorsContainer>> ExecuteAsync(TParam param);
}