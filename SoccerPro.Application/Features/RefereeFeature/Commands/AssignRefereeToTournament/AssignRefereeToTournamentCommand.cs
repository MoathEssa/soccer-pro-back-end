using MediatR;
using SoccerPro.Application.Common.ResultPattern;

namespace SoccerPro.Application.Features.RefereeFeature.Commands.AssignRefereeToTournament;

public record AssignRefereeToTournamentCommand(int RefereeId, int TournamentId) : IRequest<ApiResponse<bool>>;
