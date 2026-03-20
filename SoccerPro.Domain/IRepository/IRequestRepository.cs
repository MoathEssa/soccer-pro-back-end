using SoccerPro.Domain.Entities;
using SoccerPro.Domain.Entities.Enums;

namespace SoccerPro.Domain.IRepository;

public interface IRequestRepository
{

    public Task<bool> ProcessRequestJoinTeamForFirstTimeAsync(int requestId, int processorUserId, RequestStatus requestStatus, PlayerStatus playerStatus);
    public Task<bool> CreateRequestAsync(JoinTeamForFirstTimeRequest joinTeamForFirstTimeRequest);
    public Task<bool> HasPendingTeamRequestAsync(int userId);

    ///----------
    Task<Request?> GetRequestByIdAsync(int requestId);
    Task<bool> IsPlayerInTeamAsync(int playerId, int teamId);
    Task<(List<Request> Requests, int TotalCount)> GetRequestsByPlayerAsync(
        int playerId,
        int pageNumber = 1,
        int pageSize = 10);
    Task<(List<Request> Requests, int TotalCount)> GetRequestsByTeamAsync(
        int teamId,
        int pageNumber = 1,
        int pageSize = 10);
}