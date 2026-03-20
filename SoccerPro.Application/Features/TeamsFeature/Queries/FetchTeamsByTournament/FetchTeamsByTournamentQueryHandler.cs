using MediatR;
using SoccerPro.Application.Common.ApiResponse;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.TeamDTOs;
using SoccerPro.Application.Services.IServises;

namespace SoccerPro.Application.Features.TeamsFeature.Queries.FetchTeamsByTournament;

public class FetchTeamsByTournamentQueryHandler : IRequestHandler<FetchTeamsByTournamentQuery, ApiResponse<List<TeamTournamentViewDTO>>>
{
    private readonly ITeamServices _teamServices;

    public FetchTeamsByTournamentQueryHandler(ITeamServices teamServices)
    {
        _teamServices = teamServices;
    }

    public async Task<ApiResponse<List<TeamTournamentViewDTO>>> Handle(FetchTeamsByTournamentQuery request, CancellationToken cancellationToken)
    {
        var result = await _teamServices.GetTeamsByTournamentAsync(
            tournamentId: request.TournamentId,
            pageNumber: request.PageNumber,
            pageSize: request.PageSize
        );

        return ApiResponseHandler.Build(
            data: result.Value.teams,
            statusCode: result.StatusCode,
            succeeded: result.IsSuccess,
            message: result.IsSuccess ? "Teams fetched successfully" : result.Error?.Message,
            errors: [result.Error?.Message],
            meta: new
            {
                totalCount = result.Value.totalCount,
            }
        );
    }
}
