using FluentValidation;

namespace InformationService.Interactors.UpdatePost;

public class UpdatePostParamsValidator : AbstractValidator<UpdatePostParams>
{
    public UpdatePostParamsValidator()
    {
        // Guid PostId, Guid EditorId, string? NewTitle, string? NewContent, Guid? NewImageId

        RuleFor(p => p.NewTitle)
            .MinimumLength(5)
            .When(p => p.NewTitle != null)
            .WithMessage("Слишком маленький заголовок");

        RuleFor(p => p.NewContent)
            .MaximumLength(550)
            .When(p => p.NewContent != null)
            .WithMessage("Превышен размер статьи");
    }
}
