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
            .Must(g => g == null || g != Guid.Empty)
            .WithMessage("Неправильный идентификатор картинки");
    }
}
