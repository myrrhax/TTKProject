namespace InformationService.Utils;

public class ErrorsContainer
{
    private readonly Dictionary<string, List<string>> _errors;
    public IReadOnlyDictionary<string, List<string>> Errors { get => _errors; }

    public ErrorsContainer()
    {
        _errors = new Dictionary<string, List<string>>();
    }

    public ErrorsContainer(IEnumerable<FluentValidation.Results.ValidationFailure> failures)
        : this()
    {
        foreach (var failure in failures)
        {
            AddError(failure.PropertyName, failure.ErrorMessage);
        }
    }

    public void AddError(string errorField, string message)
    {
        if (!_errors.ContainsKey(errorField))
        {
            _errors[errorField] = new List<string>();
        }
        _errors[errorField].Add(message);
    }
}
