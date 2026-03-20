using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.TeamDTOs;

namespace SoccerPro.Application.Features.CoachFeature.Commands.AssignCoachIntoTeam;

public record AssignCoachIntoTeamCommand(AssignCoachIntoTeamDTO Dto) : IRequest<ApiResponse<bool>>
{
    public AssignCoachIntoTeamDTO AssignCoachIntoTeamDTO { get; set; } = Dto;
}