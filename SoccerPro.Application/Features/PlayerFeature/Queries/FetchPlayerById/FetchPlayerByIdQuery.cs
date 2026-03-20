using MediatR;
using SoccerPro.Application.DTOs.PlayerDTOs;
using SoccerPro.Application.Common.ResultPattern;

namespace SoccerPro.Application.Features.PlayerFeature.Queries.FetchPlayerById
{
    public record FetchPlayerByIdQuery(int PlayerId) : IRequest<ApiResponse<PlayerDTO>>;
}