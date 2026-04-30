using FluentValidation;
using InteractHub.Core.DTOs.Posts;

namespace InteractHub.API.Validators.Posts;

public class CreateCommentRequestValidator : AbstractValidator<CreateCommentRequestDto>
{
    public CreateCommentRequestValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Nội dung bình luận không được để trống.")
            .MaximumLength(5000).WithMessage("Nội dung bình luận tối đa 5000 ký tự.");
    }
}
