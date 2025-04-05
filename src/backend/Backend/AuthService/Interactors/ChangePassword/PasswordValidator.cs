using FluentValidation;

namespace AuthService.Interactors.ChangePassword;

public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(u => u)
            .NotNull()
            .WithMessage("Пароль не может быть пустым")
            .MinimumLength(3)
            .WithMessage("Пароль слишком короткий")
            .MaximumLength(25)
            .WithMessage("Пароль слишком длинный")
            .Matches("^[A-Za-z0-9!@#$%^&*(),.?\":{}|<>]+$")
            .WithMessage("Пароль должен содержать латинские буквы, цифры, символы");
    }
}
