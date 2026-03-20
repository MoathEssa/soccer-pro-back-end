using FluentValidation;

namespace SoccerPro.Application.Features.TeamsFeature.Commands.AddTeam;

public class AddTeamDTOValidator : AbstractValidator<AddTeamCommand>
{
    public AddTeamDTOValidator()
    {
        RuleFor(x => x.AddTeamDTO.Name)
            .NotEmpty().WithMessage("Team name is required")
            .MaximumLength(100).WithMessage("Team name must not exceed 100 characters");

        RuleFor(x => x.AddTeamDTO.Address)
            .NotEmpty().WithMessage("Address is required")
            .MaximumLength(200).WithMessage("Address must not exceed 200 characters");

        RuleFor(x => x.AddTeamDTO.Website)
            .NotEmpty().WithMessage("Website is required")
            .MaximumLength(100).WithMessage("Website must not exceed 100 characters")
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("Website must be a valid URL");

        RuleFor(x => x.AddTeamDTO.NumberOfPlayers)
            .GreaterThan(0).WithMessage("Number of players must be greater than 0")
            .LessThanOrEqualTo(30).WithMessage("Number of players must not exceed 30");

        RuleFor(x => x.AddTeamDTO.ManagerId)
            .GreaterThan(0).WithMessage("Manager ID must be greater than 0");
    }
}