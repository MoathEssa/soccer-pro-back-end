using MediatR;
using SoccerPro.Application.Common.ApiResponse;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.Services.IServises;
using SoccerPro.Domain.Entities;

namespace SoccerPro.Application.Features.MatchFeature.Commands.ScheduleMatch;

public class ScheduleMatchCommandHandler : IRequestHandler<ScheduleMatchCommand, ApiResponse<bool>>
{
    private readonly IMatchServices _matchServices;

    public ScheduleMatchCommandHandler(IMatchServices matchServices)
    {
        _matchServices = matchServices;
    }

    public async Task<ApiResponse<bool>> Handle(ScheduleMatchCommand request, CancellationToken cancellationToken)
    {
        var match = new MatchSchedule
        {
            TournamentId = request.TournamentId,
            TournamentPhase = request.TournamentPhase,
            TournamentTeamIdA = request.TournamentTeamIdA,
            TournamentTeamIdB = request.TournamentTeamIdB,
            Date = request.Date,
            FieldId = request.FieldId
        };

        var result = await _matchServices.ScheduleMatchAsync(match);

        return ApiResponseHandler.Build(
            data: result.Value,
            statusCode: result.StatusCode,
            succeeded: result.IsSuccess,
            message: result.IsSuccess ? "Match scheduled successfully" : result.Error?.Message,
            errors: result.IsSuccess ? null : [result.Error?.Message ?? "Unknown error"]
        );
    }
}