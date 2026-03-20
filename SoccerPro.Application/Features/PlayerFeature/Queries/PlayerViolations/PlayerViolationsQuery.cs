using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.PlayerDTOs;

namespace SoccerPro.Application.Features.PlayerFeature.Queries.PlayerViolations
{
    public record PlayerViolationsQuery(int PageNumber = 1, int PageSize = 10, int? CardType = null) : IRequest<ApiResponse<List<PlayerViolationDTO>>>;
}
