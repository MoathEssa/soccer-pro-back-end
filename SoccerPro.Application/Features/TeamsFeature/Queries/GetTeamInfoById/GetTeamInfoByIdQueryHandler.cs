using MediatR;
using SoccerPro.Application.Common.ApiResponse;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.TeamDTOs;
using SoccerPro.Application.Services.IServises;

namespace SoccerPro.Application.Features.TeamsFeature.Queries.GetTeamInfoById
{
    public class GetTeamInfoByIdQueryHandler : IRequestHandler<GetTeamInfoByIdQuery, ApiResponse<TeamViewDTO>>
    {
        private readonly ITeamServices _teamServices;

        public GetTeamInfoByIdQueryHandler(ITeamServices teamServices)
        {
            _teamServices = teamServices;
        }


        async Task<ApiResponse<TeamViewDTO>> IRequestHandler<GetTeamInfoByIdQuery, ApiResponse<TeamViewDTO>>.Handle(GetTeamInfoByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _teamServices.GetTeamByIdAsync(request.TeamId);

            return ApiResponseHandler.Success(result.Value);
        }
    }
}
