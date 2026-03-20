using FluentValidation;

namespace SoccerPro.Application.Features.AuthFeature.Commands.ResetPasswordCommand
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(x => x.ResetPasswordDTO.UserId)
                .NotEmpty().WithMessage("UserId is required.")
                .GreaterThan(0).WithMessage("UserId must be greater than zero.");

            RuleFor(x => x.ResetPasswordDTO.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

            RuleFor(x => x.ResetPasswordDTO.ConfirmPassword)
                .Equal(x => x.ResetPasswordDTO.NewPassword).WithMessage("Passwords do not match.");
        }
    }


}

