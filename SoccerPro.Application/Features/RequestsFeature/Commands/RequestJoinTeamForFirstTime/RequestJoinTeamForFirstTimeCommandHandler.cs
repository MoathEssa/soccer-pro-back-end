using MediatR;
using SoccerPro.Application.Common.ApiResponse;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.Services.IServises;
using SoccerPro.Domain.Entities;
using SoccerPro.Domain.Entities.Enums;
using SoccerPro.Domain.Enums;

namespace SoccerPro.Application.Features.RequestsFeature.Commands.RequestJoinTeamForFirstTime;

public class RequestJoinTeamForFirstTimeCommandHandler : IRequestHandler<RequestJoinTeamForFirstTimeCommand, ApiResponse<bool>>
{
    private readonly IRequestServices _requestServices;

    public RequestJoinTeamForFirstTimeCommandHandler(IRequestServices requestServices)
    {
        _requestServices = requestServices;
    }

    public async Task<ApiResponse<bool>> Handle(RequestJoinTeamForFirstTimeCommand request, CancellationToken cancellationToken)
    {

        var result = await _requestServices.CreateJoinTeamRequestAsync(
             new JoinTeamForFirstTimeRequest()
             {
                 UserId = request.RequestJoinTeamDTO.UserId,
                 TeamId = request.RequestJoinTeamDTO.TeamId,
                 PlayerPosition = (PlayerPosition)request.RequestJoinTeamDTO.PlayerPosition,
                 PlayerRole = (PlayerRole)request.RequestJoinTeamDTO.PlayerRole,
                 PlayerType = (PlayerType)request.RequestJoinTeamDTO.PlayerType,
                 DepartmentId = request.RequestJoinTeamDTO.DepartmentId,
                 Notes = request.RequestJoinTeamDTO.Notes
             }

            );

        return ApiResponseHandler.Build(
            result.Value,
            result.StatusCode,
            result.IsSuccess,
            null,
            [result.Error.Message]
        );

    }
}