using FluentValidation;

namespace SoccerPro.Application.Features.TournamentFeature.Commands.AssignTeamsInTournament;

public class AssignTeamInTournamentCommandValidator : AbstractValidator<AssignTeamInTournamentCommand>
{
    public AssignTeamInTournamentCommandValidator()
    {
        RuleFor(x => x.AssignTeamsInTournamentDTO.TournamentId)
            .NotEmpty()
            .WithMessage("Tournament ID is required");

        RuleFor(x => x.AssignTeamsInTournamentDTO.TeamId)
            .NotEmpty().WithMessage("At least one team must be assigned.")
            .GreaterThan(0)
            .WithMessage("TeamId must be gratter than zero");

    }
}