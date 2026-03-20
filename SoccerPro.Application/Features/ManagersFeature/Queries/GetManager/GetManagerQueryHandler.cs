using MediatR;
using SoccerPro.Application.Common.ApiResponse;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.ManagerDTOs;
using SoccerPro.Application.Services.IServises;

namespace SoccerPro.Application.Features.ManagersFeature.Queries.GetManager;

public class GetManagerQueryHandler : IRequestHandler<GetManagerQuery, ApiResponse<ManagerViewDTO>>
{
    private readonly IManagerServices _managerServices;

    public GetManagerQueryHandler(IManagerServices managerServices)
    {
        _managerServices = managerServices;
    }

    public async Task<ApiResponse<ManagerViewDTO>> Handle(GetManagerQuery request, CancellationToken cancellationToken)
    {
        var result = await _managerServices.GetManagerByIdAsync(request.ManagerId);
        return ApiResponseHandler.Build(result.Value, result.StatusCode, result.IsSuccess, null, [result.Error.Message]);
    }
}