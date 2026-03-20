
using FluentValidation;
using SoccerPro.Domain.Entities.Enums;

namespace SoccerPro.Application.Features.RequestsFeature.Commands.ProcessJoinTeamRequest
{

    public class ProcessJoinTeamRequestValidator : AbstractValidator<ProcessJoinTeamRequestCommand>
    {
        public ProcessJoinTeamRequestValidator()
        {
            RuleFor(x => x.ProcessJoinTeamRequestDTO.RequestId)
                .GreaterThan(0).WithMessage("Request ID is required.");

            RuleFor(x => x.ProcessJoinTeamRequestDTO.ProcessorUserId)
                .GreaterThan(0).WithMessage("Processor User ID is required.");

            RuleFor(x => x.ProcessJoinTeamRequestDTO.RequestStatus)
                .Must(value => Enum.IsDefined(typeof(RequestStatus), value))
                .WithMessage("Invalid request status.");

            RuleFor(x => x.ProcessJoinTeamRequestDTO.PlayerStatus)
                .Must(value => Enum.IsDefined(typeof(PlayerStatus), value))
                .WithMessage("Invalid player status.");
        }
    }

}
