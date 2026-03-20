using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Domain.Entities.Enums;

namespace SoccerPro.Application.Features.MatchFeature.Commands.ScheduleMatch;

public record ScheduleMatchCommand(
    int TournamentId,
    TournamentPhase TournamentPhase,
    int TournamentTeamIdA,
    int TournamentTeamIdB,
    DateTime Date,
    int FieldId
) : IRequest<ApiResponse<bool>>;