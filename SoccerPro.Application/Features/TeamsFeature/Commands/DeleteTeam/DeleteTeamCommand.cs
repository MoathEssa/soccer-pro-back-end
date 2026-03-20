using MediatR;
using SoccerPro.Application.Common.ResultPattern;

namespace SoccerPro.Application.Features.TeamsFeature.Commands.DeleteTeam;

public record DeleteTeamCommand(int TeamId) : IRequest<ApiResponse<bool>>;