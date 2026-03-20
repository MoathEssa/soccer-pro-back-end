using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.PlayerDTOs;

namespace SoccerPro.Application.Features.PlayerFeature.Queries.TopScorerPlayer
{
    public record TopScorerPlayerQuery(int PageNumber = 1, int PageSize = 10) : IRequest<ApiResponse<List<TopScorerPlayerDTO>>>;
}
