using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.PlayerDTOs;

namespace SoccerPro.Application.Features.PlayerFeature.Commands.AssignPlayersIntoTeam;

public record AssignPlayerIntoTeamCommand(AssignPlayerIntoTeamDTO Dto) : IRequest<ApiResponse<bool>>
{
    public AssignPlayerIntoTeamDTO AssignPlayerIntoTeamDTO { get; set; } = Dto;
}