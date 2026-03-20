using MediatR;
using SoccerPro.Application.Common.ApiResponse;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.Services.IServises;

namespace SoccerPro.Application.Features.TeamsFeature.Commands.DeleteTeam;

public class DeleteTeamCommandHandler : IRequestHandler<DeleteTeamCommand, ApiResponse<bool>>
{
    private readonly ITeamServices _teamServices;

    public DeleteTeamCommandHandler(ITeamServices teamServices)
    {
        _teamServices = teamServices;
    }

    public async Task<ApiResponse<bool>> Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
    {
        var result = await _teamServices.DeleteTeamAsync(request.TeamId);
        return ApiResponseHandler.Build(result.Value, result.StatusCode, result.IsSuccess, null, [result.Error.Message]);
    }
}