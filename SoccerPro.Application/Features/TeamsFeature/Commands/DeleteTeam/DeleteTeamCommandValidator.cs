using FluentValidation;

namespace SoccerPro.Application.Features.TeamsFeature.Commands.DeleteTeam
{

    public class DeleteTeamCommandValidator : AbstractValidator<DeleteTeamCommand>
    {
        public DeleteTeamCommandValidator()
        {
            RuleFor(x => x.TeamId)
                .NotEmpty()
                .WithMessage("TeamId ID is required")
                .GreaterThan(0)
                .WithMessage("TeamId ID must be gratter than zero.");
        }
    }

}
