using FluentValidation;
using SoccerPro.Domain.Enums;

namespace SoccerPro.Application.Features.PlayerFeature.Commands.AssignPlayersIntoTeam;

public class AssignPlayerIntoTeamValidator : AbstractValidator<AssignPlayerIntoTeamCommand>
{
    public AssignPlayerIntoTeamValidator()
    {
        RuleFor(x => x.AssignPlayerIntoTeamDTO.TournamentId)
        .GreaterThan(0)
        .WithMessage("TeamId must be greater than 0");

        RuleFor(x => x.AssignPlayerIntoTeamDTO.TeamId)
            .GreaterThan(0)
            .WithMessage("TeamId must be greater than 0");

        RuleFor(x => x.AssignPlayerIntoTeamDTO.PlayerId)
          .GreaterThan(0)
          .WithMessage("PlayerId must be greater than 0");

        RuleFor(x => x.AssignPlayerIntoTeamDTO.Position)
            .Must(value => Enum.IsDefined(typeof(PlayerPosition), value))
            .WithMessage("Invalid player position");

        RuleFor(x => x.AssignPlayerIntoTeamDTO.Role)
            .Must(value => Enum.IsDefined(typeof(PlayerRole), value))
            .WithMessage("Invalid player role");

    }
}