using FluentValidation;

namespace InformationService.Interactors.UpdatePost;

public class UpdatePostParamsValidator : AbstractValidator<UpdatePostParams>
{
    public UpdatePostParamsValidator()
    {
        RuleFor(p => p.NewTitle)
            .MinimumLength(5)
            .When(p => p.NewTitle != null)
            .WithMessage("Слишком маленький заголовок");

        RuleFor(p => p.NewContent)
            .MaximumLength(550)
            .When(p => p.NewContent != null)
            .WithMessage("Превышен размер статьи");

        RuleFor(p => p.NewImageId)
            .NotEmpty()
            .Must(p => Guid.TryParse(p, out _))
            .When(p => p.NewImageId != null)
            .WithMessage("Неправильный идентификатор картинки");
    }
}
