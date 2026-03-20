using MediatR;
using SoccerPro.Application.Common.ApiResponse;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.Services.IServises;

namespace SoccerPro.Application.Features.TournamentFeature.Commands.AssignTeamsInTournament;

public class AssignTeamInTournamentCommandHandler : IRequestHandler<AssignTeamInTournamentCommand, ApiResponse<bool>>
{
    private readonly ITournamentServices _tournamentServices;

    public AssignTeamInTournamentCommandHandler(ITournamentServices tournamentServices)
    {
        _tournamentServices = tournamentServices;
    }

    public async Task<ApiResponse<bool>> Handle(AssignTeamInTournamentCommand request, CancellationToken cancellationToken)
    {

        var result = await _tournamentServices.AssignTeamToTournamentAsync(
          request.AssignTeamsInTournamentDTO.TournamentId,
          request.AssignTeamsInTournamentDTO.TeamId);

        return ApiResponseHandler.Build(
            data: result.Value,
            statusCode: result.StatusCode,
            succeeded: result.IsSuccess,
            message: result.IsSuccess ? "Team assigned to tournament successfully" : result.Error.Message,
            errors: result.IsSuccess ? null : [result.Error.Message]
        );
    }
}