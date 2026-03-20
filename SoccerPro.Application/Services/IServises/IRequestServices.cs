using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Domain.Entities;
using SoccerPro.Domain.Entities.Enums;

namespace SoccerPro.Application.Services.IServises;

public interface IRequestServices
{
    public Task<Result<bool>> CreateJoinTeamRequestAsync(JoinTeamForFirstTimeRequest joinTeamForFirstTimeRequest);
    public Task<Result<bool>> ProcessRequestJoinTeamForFirstTimeAsync(int requestId, int processorUserId, RequestStatus requestStatus, PlayerStatus playerStatus);

}