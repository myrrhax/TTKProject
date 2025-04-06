using FluentValidation;

namespace AuthService.Interactors.Register;

public class RegisterParamsValidation: AbstractValidator<RegisterParams>
{
    //string Login, string Password, string Name, string Surname, string? SecondName
    public RegisterParamsValidation()
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

        RuleFor(u => u.Name)
            .NotNull()
            .WithMessage("Имя не может быть пустым")
            .MinimumLength(3)
            .WithMessage("Имя слишком короткое")
            .MaximumLength(25)
            .Matches("^[А-Яа-яЁё]+$")
            .WithMessage("Имя должно состоять только русских букв");

        RuleFor(u => u.Surname)
            .NotNull()
            .WithMessage("Фамилия не может быть пустым")
            .MinimumLength(3)
            .WithMessage("Фамилия слишком короткая")
            .MaximumLength(25)
            .Matches("^[А-Яа-яЁё]+$")
            .WithMessage("Имя должно состоять только русских букв");

        RuleFor(u => u.SecondName)
            .Matches("^[А-Яа-яЁё]*$")
            .WithMessage("Отчество должно состоять только русских букв");
    }
}
