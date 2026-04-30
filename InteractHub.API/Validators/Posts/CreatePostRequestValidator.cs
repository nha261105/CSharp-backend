using FluentValidation;
using InteractHub.Core.DTOs.Posts;

namespace InteractHub.API.Validators.Posts;

public class CreatePostRequestValidator : AbstractValidator<CreatePostRequestDto>
{
    private static readonly string[] AllowedPostTypes = { "Text", "Image", "Video", "Shared" };
    private static readonly string[] AllowedVisibilities = { "Public", "Friends", "Private" };

    public CreatePostRequestValidator()
    {
        RuleFor(x => x.PostType)
            .NotEmpty().WithMessage("PostType không được để trống.")
            .Must(v => AllowedPostTypes.Contains(v)).WithMessage($"PostType phải là một trong: {string.Join(", ", AllowedPostTypes)}.");

        RuleFor(x => x.Visibility)
            .NotEmpty().WithMessage("Visibility không được để trống.")
            .Must(v => AllowedVisibilities.Contains(v)).WithMessage($"Visibility phải là một trong: {string.Join(", ", AllowedVisibilities)}.");

        RuleFor(x => x.Content)
            .MaximumLength(50000).WithMessage("Nội dung tối đa 50000 ký tự.")
            .When(x => x.Content != null);

        RuleFor(x => x.LocationName)
            .MaximumLength(255).WithMessage("Tên địa điểm tối đa 255 ký tự.")
            .When(x => x.LocationName != null);

        RuleFor(x => x.MusicStartSec)
            .GreaterThanOrEqualTo(0).WithMessage("MusicStartSec không được âm.");
    }
}
