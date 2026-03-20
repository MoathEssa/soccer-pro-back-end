using MediatR;
using SoccerPro.Application.Common.ResultPattern;

namespace SoccerPro.Application.Features.RefereeFeature.Commands.AssignRefereeToMatchInTournament;

public record AssignRefereeToMatchInTournamentCommand(int tournamentRefereeId, int MatchScheduleId) : IRequest<ApiResponse<bool>>;
