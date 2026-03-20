
using FluentValidation;
using SoccerPro.Application.Features.AuthFeature.Commands.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {

        RuleFor(x => x.RegisterUserDTO.KFUPMId)
            .NotEmpty().WithMessage("KFUPM ID is required.")
            .Matches(@"^\d{9}$").WithMessage("KFUPM ID must be exactly 9 digits.");

        RuleFor(x => x.RegisterUserDTO.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50);

        RuleFor(x => x.RegisterUserDTO.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50);

        RuleFor(x => x.RegisterUserDTO.UserName)
      .NotEmpty().WithMessage("Username is required.")
      .EmailAddress().WithMessage("Username must be a valid email address.");

        RuleFor(x => x.RegisterUserDTO.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");



    }
}



