using Namespace.SoccerPro.Domain.IRepository;
using SoccerPro.Application.Common.Errors;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.Services.IServises;
using SoccerPro.Domain.Entities;
using SoccerPro.Domain.Entities.Enums;
using SoccerPro.Domain.IRepository;
using System.Net;

namespace SoccerPro.Application.Services;

public class RequestServices : IRequestServices
{
    private readonly IRequestRepository _requestRepository;
    private readonly IPlayerRepository _playerRepository;

    public RequestServices(IRequestRepository requestRepository, IPlayerRepository playerRepository)
    {
        _requestRepository = requestRepository;
        _playerRepository = playerRepository;
    }

    public async Task<Result<bool>> CreateJoinTeamRequestAsync(JoinTeamForFirstTimeRequest joinTeamForFirstTimeRequest)
    {

        // Check if the player is already in the team
        bool isInTeam = await _playerRepository.IsUserAlreadyPlayerAsync(joinTeamForFirstTimeRequest.UserId);

        if (isInTeam)
        {
            return Result<bool>.Failure(
                Error.ValidationError("This user is already assigned to a team. This action is only allowed for players requesting to join a team for the first time."),
                HttpStatusCode.BadRequest);
        }

        // Check for pending requests
        bool hasPendingRequest = await _requestRepository.HasPendingTeamRequestAsync(joinTeamForFirstTimeRequest.UserId);
        if (hasPendingRequest)
        {
            return Result<bool>.Failure(
                Error.ValidationError("Player already has a pending request for this team"),
                HttpStatusCode.BadRequest);
        }

        // Create the request
        bool result = await _requestRepository.CreateRequestAsync(joinTeamForFirstTimeRequest);
        return Result<bool>.Success(result);

    }


    public async Task<Result<bool>> ProcessRequestJoinTeamForFirstTimeAsync(int requestId, int processorUserId, RequestStatus requestStatus, PlayerStatus playerStatus)
    {
        var request = await _requestRepository.GetRequestByIdAsync(requestId);

        if (request == null)
        {
            return Result<bool>.Failure(
                Error.ValidationError("Request not found"),
                HttpStatusCode.NotFound);
        }

        if (request.Status != RequestStatus.Pending)
        {
            return Result<bool>.Failure(
                Error.ValidationError("Request is not pending"),
                HttpStatusCode.BadRequest);
        }


        bool result = await _requestRepository.ProcessRequestJoinTeamForFirstTimeAsync(requestId, processorUserId, requestStatus, playerStatus);
        return Result<bool>.Success(result);

    }


    //---------------------------


}