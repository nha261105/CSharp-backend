using FluentValidation;
using InteractHub.Core.DTOs.Auth;

namespace InteractHub.API.Validators.Auth;

public class RegisterRequestValidator : AbstractValidator<RegisterRequestDto>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Tên đăng nhập không được để trống.")
            .Length(3, 50).WithMessage("Tên đăng nhập phải từ 3 đến 50 ký tự.")
            .Matches("^[a-zA-Z0-9._]+$").WithMessage("Tên đăng nhập chỉ được chứa chữ cái, số, dấu chấm và gạch dưới.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email không được để trống.")
            .EmailAddress().WithMessage("Email không hợp lệ.")
            .MaximumLength(254).WithMessage("Email tối đa 254 ký tự.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Mật khẩu không được để trống.")
            .MinimumLength(6).WithMessage("Mật khẩu tối thiểu 6 ký tự.")
            .MaximumLength(100).WithMessage("Mật khẩu tối đa 100 ký tự.");

        RuleFor(x => x.Fullname)
            .NotEmpty().WithMessage("Họ tên không được để trống.")
            .Length(2, 100).WithMessage("Họ tên phải từ 2 đến 100 ký tự.");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[0-9]{7,20}$").WithMessage("Số điện thoại không hợp lệ.")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));
    }
}
