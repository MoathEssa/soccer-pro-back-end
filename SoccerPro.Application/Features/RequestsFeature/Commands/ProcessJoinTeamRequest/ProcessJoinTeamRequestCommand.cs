using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.RequestDTOs;

namespace SoccerPro.Application.Features.RequestsFeature.Commands.ProcessJoinTeamRequest
{
    public class ProcessJoinTeamRequestCommand : IRequest<ApiResponse<bool>>
    {
        public ProcessJoinTeamRequestDTO ProcessJoinTeamRequestDTO { get; set; }

        public ProcessJoinTeamRequestCommand(ProcessJoinTeamRequestDTO processJoinTeamRequestDTO)
        {
            ProcessJoinTeamRequestDTO = processJoinTeamRequestDTO;
        }
    }
}
