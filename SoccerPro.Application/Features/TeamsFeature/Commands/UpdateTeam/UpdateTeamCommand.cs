using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.TeamDTOs;

namespace SoccerPro.Application.Features.TeamsFeature.Commands.UpdateTeam;

public class UpdateTeamCommand : IRequest<ApiResponse<bool>>
{
    public UpdateTeamCommand(UpdateTeamDTO updateTeamDTO)
    {
        UpdateTeamDTO = updateTeamDTO;
    }

    public UpdateTeamDTO UpdateTeamDTO { get; set; }
}