using FluentValidation;
using InteractHub.Core.DTOs.Stories;

namespace InteractHub.API.Validators.Stories;

public class CreateStoryRequestValidator : AbstractValidator<CreateStoryRequestDto>
{
    private static readonly string[] AllowedMediaTypes = { "Image", "Video" };
    private static readonly string[] AllowedVisibilities = { "Friends", "Public" };

    public CreateStoryRequestValidator()
    {
        RuleFor(x => x.MediaUrl)
            .NotEmpty().WithMessage("MediaUrl không được để trống.");

        RuleFor(x => x.MediaType)
            .NotEmpty().WithMessage("MediaType không được để trống.")
            .Must(v => AllowedMediaTypes.Contains(v)).WithMessage($"MediaType phải là một trong: {string.Join(", ", AllowedMediaTypes)}.");

        RuleFor(x => x.Visibility)
            .Must(v => AllowedVisibilities.Contains(v)).WithMessage($"Visibility phải là một trong: {string.Join(", ", AllowedVisibilities)}.")
            .When(x => !string.IsNullOrEmpty(x.Visibility));

        RuleFor(x => x.DurationSec)
            .InclusiveBetween(1, 60).WithMessage("DurationSec phải từ 1 đến 60 giây.");

        RuleFor(x => x.Caption)
            .MaximumLength(500).WithMessage("Caption tối đa 500 ký tự.")
            .When(x => x.Caption != null);
    }
}
