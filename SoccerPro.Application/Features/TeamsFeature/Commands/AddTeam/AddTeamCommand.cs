using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.TeamDTOs;

namespace SoccerPro.Application.Features.TeamsFeature.Commands.AddTeam;

public record AddTeamCommand(AddTeamDTO dto) : IRequest<ApiResponse<bool>>
{
    public AddTeamDTO AddTeamDTO { get; set; } = dto;
}