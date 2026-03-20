using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.TeamDTOs;

namespace SoccerPro.Application.Features.TeamsFeature.Queries.GetTeamInfoById
{
    public class GetTeamInfoByIdQuery : IRequest<ApiResponse<TeamViewDTO>>
    {
        public GetTeamInfoByIdQuery(int teamId)
        {
            TeamId = teamId;
        }

        public int TeamId { set; get; }
    }
}
