namespace SoccerPro.Application.Features.RequestsFeature.Commands.RequestJoinTeamForFirstTime;

using FluentValidation;
using SoccerPro.Domain.Entities.Enums;
using SoccerPro.Domain.Enums;

public class RequestJoinTeamForFirstTimeValidator : AbstractValidator<RequestJoinTeamForFirstTimeCommand>
{
    public RequestJoinTeamForFirstTimeValidator()
    {
        RuleFor(x => x.RequestJoinTeamDTO.UserId)
            .GreaterThan(0).WithMessage("User ID must be provided.");

        RuleFor(x => x.RequestJoinTeamDTO.TeamId)
            .GreaterThan(0).WithMessage("Team ID must be provided.");

        RuleFor(x => x.RequestJoinTeamDTO.PlayerPosition)
            .Must(value => Enum.IsDefined(typeof(PlayerPosition), value))
            .WithMessage("Invalid player position.");

        RuleFor(x => x.RequestJoinTeamDTO.PlayerRole)
            .Must(value => Enum.IsDefined(typeof(PlayerRole), value))
            .WithMessage("Invalid player role.");

        RuleFor(x => x.RequestJoinTeamDTO.PlayerType)
            .Must(value => Enum.IsDefined(typeof(PlayerType), value))
            .WithMessage("Invalid player type.");

        RuleFor(x => x.RequestJoinTeamDTO.DepartmentId)
            .GreaterThan(0).WithMessage("Department ID must be provided.");

        RuleFor(x => x.RequestJoinTeamDTO.Notes)
            .MaximumLength(500)
            .WithMessage("Notes cannot exceed 500 characters.");
    }
}
