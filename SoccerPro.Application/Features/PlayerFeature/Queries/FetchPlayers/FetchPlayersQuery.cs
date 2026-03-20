using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.PlayerDTOs;

namespace SoccerPro.Application.Features.PlayerFeature.Queries.FetchPlayers;

public class FetchPlayersQuery : IRequest<ApiResponse<List<PlayerDTO>>> {

    public int? PlayerId { get; set; } = null;
    public  string? KfupmId { get; set; } = null;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;


public FetchPlayersQuery(int? playerId, string? kfupmId, int pageNumber, int pageSize) {
    PlayerId = playerId;
    KfupmId = kfupmId;
    PageNumber = pageNumber;
    PageSize = pageSize;   
}



}