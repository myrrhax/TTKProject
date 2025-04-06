using FluentValidation;

namespace InformationService.Interactors.CreatePost;

public class CreatePostParamsValidator : AbstractValidator<CreatePostParams>
{
    public CreatePostParamsValidator()
    {
        RuleFor(p => p.Title)
            .MinimumLength(5)
            .WithMessage("Слишком маленький заголовок")
            .NotEmpty()
            .WithMessage("Заголовок не может быть пустым");

        RuleFor(p => p.Content)
            .MaximumLength(550)
            .When(p => p.Content != null)
            .WithMessage("Превышен размер статьи");
    }
}
