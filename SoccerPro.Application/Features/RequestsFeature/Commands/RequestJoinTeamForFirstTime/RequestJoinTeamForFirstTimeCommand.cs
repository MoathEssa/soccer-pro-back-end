using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.RequestDTOs;

namespace SoccerPro.Application.Features.RequestsFeature.Commands.RequestJoinTeamForFirstTime;

public record RequestJoinTeamForFirstTimeCommand(RequestJoinTeamDTO RequestJoinTeamDTO) : IRequest<ApiResponse<bool>>;