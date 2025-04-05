using FluentValidation;

namespace AuthService.Interactors.Login;

public class LoginParamsValidation : AbstractValidator<LoginParams>
{
    public LoginParamsValidation()
    {
        RuleFor(u => u.Login)
            .NotNull()
            .WithMessage("Логин не может быть пустым")
            .MinimumLength(3)
            .WithMessage("Логин слишком короткий")
            .MaximumLength(20)
            .WithMessage("Логин слишком длинный")
            .Matches("^[A-Za-z]+$")
            .WithMessage("Логин должен содержать только латинские буквы");

        RuleFor(u => u.Password)
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
