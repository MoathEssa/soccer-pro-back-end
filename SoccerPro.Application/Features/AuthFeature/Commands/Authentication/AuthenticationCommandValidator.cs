using FluentValidation;
using SoccerPro.Application.Features.AuthFeature.Commands.Authentication;

public class AuthenticationCommandValidator : AbstractValidator<AuthenticationCommand>
{
    public AuthenticationCommandValidator()
    {
        RuleFor(x => x.AuthenticationRequest.Username)
         .NotEmpty()
             .WithMessage("Invalid Username or password.")
         .EmailAddress()
             .WithMessage("Invalid Username or password.");

        RuleFor(x => x.AuthenticationRequest.Password)
            .NotEmpty()
                .WithMessage("Invalid Username or password.");
    }
}