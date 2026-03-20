using MediatR;
using SoccerPro.Application.Common.ApiResponse;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.Services.IServises;
using SoccerPro.Domain.Entities.Views;

namespace SoccerPro.Application.Features.MatchFeature.Queries.GetAllScheduledMatches;
public class GetAllScheduledMatchesQueryHandler
    : IRequestHandler<GetAllScheduledMatchesQuery, ApiResponse<List<MatchView>>>
{
    private readonly IMatchServices _matchServices;

    public GetAllScheduledMatchesQueryHandler(IMatchServices matchServices)
    {
        _matchServices = matchServices;
    }

    public async Task<ApiResponse<List<MatchView>>> Handle(
        GetAllScheduledMatchesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _matchServices.GetAllScheduledMatchesAsync(
            request.TournamentId,
            request.TournamentPhase,
            request.TeamAName,
            request.TeamBName,
            request.FieldName,
            request.MatchDate,
            request.PageNumber,
            request.PageSize
        );

        return ApiResponseHandler.Build(
            data: result.Value.matches,
            statusCode: result.StatusCode,
            succeeded: result.IsSuccess,
            message: result.IsSuccess ? "Matches retrieved successfully" : result.Error?.Message,
            errors: result.IsSuccess ? null : [result.Error?.Message ?? "Unknown error"], meta: new
            {
                count = result.Value.totalCount
            }
        );
    }
}
