using MediatR;
using SoccerPro.Application.Common.ApiResponse;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.RefereeDTOs;
using SoccerPro.Application.Services.IServises;

namespace SoccerPro.Application.Features.RefereeFeature.Queries.GetAllReferees;

public class GetAllRefereesQueryHandler : IRequestHandler<GetAllRefereesQuery, ApiResponse<List<RefereeViewDTO>>>
{
    private readonly IRefereeServices _refereeServices;

    public GetAllRefereesQueryHandler(IRefereeServices refereeServices)
    {
        _refereeServices = refereeServices;
    }

    public async Task<ApiResponse<List<RefereeViewDTO>>> Handle(GetAllRefereesQuery request, CancellationToken cancellationToken)
    {
        var result = await _refereeServices.GetAllRefereesAsync();
        return ApiResponseHandler.Build(result.Value, result.StatusCode, result.IsSuccess);
    }
}
