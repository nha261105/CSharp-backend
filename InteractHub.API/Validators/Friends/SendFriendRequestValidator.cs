using FluentValidation;
using InteractHub.Core.DTOs.Friends;

namespace InteractHub.API.Validators.Friends;

public class SendFriendRequestValidator : AbstractValidator<SendFriendRequestDto>
{
    public SendFriendRequestValidator()
    {
        RuleFor(x => x.ReceiverId)
            .GreaterThan(0).WithMessage("ReceiverId không hợp lệ.");
    }
}
