using MediatR;
using SoccerPro.Application.Common.ApiResponse;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.Services.IServises;
using SoccerPro.Domain.Entities.Enums;

namespace SoccerPro.Application.Features.RequestsFeature.Commands.ProcessJoinTeamRequest
{
    public class ProcessJoinTeamRequestCommandHandler : IRequestHandler<ProcessJoinTeamRequestCommand, ApiResponse<bool>>
    {
        private readonly IRequestServices _requestServices;

        public ProcessJoinTeamRequestCommandHandler(IRequestServices requestServices)
        {
            _requestServices = requestServices;
        }

        public async Task<ApiResponse<bool>> Handle(ProcessJoinTeamRequestCommand request, CancellationToken cancellationToken)
        {

            var result = await _requestServices.ProcessRequestJoinTeamForFirstTimeAsync(
                requestId: request.ProcessJoinTeamRequestDTO.RequestId,
                processorUserId: request.ProcessJoinTeamRequestDTO.ProcessorUserId,
                requestStatus: (RequestStatus)request.ProcessJoinTeamRequestDTO.RequestStatus,
                playerStatus: (PlayerStatus)request.ProcessJoinTeamRequestDTO.PlayerStatus
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
}
