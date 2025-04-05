namespace TasksService.Utils;

public class ErrorsContainer
{
    private readonly Dictionary<string, List<string>> _errors = new();

    public void AddError(string field, string message)
    {
        if (!_errors.ContainsKey(field))
            _errors[field] = new List<string>();

        _errors[field].Add(message);
    }

    public bool HasErrors => _errors.Any();

    public Dictionary<string, List<string>> GetAll() => _errors;
}
