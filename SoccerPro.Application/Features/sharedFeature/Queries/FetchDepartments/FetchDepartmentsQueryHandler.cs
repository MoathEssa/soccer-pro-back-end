using MediatR;
using SoccerPro.Application.Common.ApiResponse;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.Services.IServises;
using SoccerPro.Application.DTOs.SharedDTOs;

namespace SoccerPro.Application.Features.sharedFeature.Queries.FetchDepartments;

public class FetchDepartmentsQueryHandler : IRequestHandler<FetchDepartmentsQuery, ApiResponse<List<DepartmentDTO>>>
{
    private readonly ISharedServices _sharedServices;

    public FetchDepartmentsQueryHandler(ISharedServices sharedServices)
    {
        _sharedServices = sharedServices;
    }

    public async Task<ApiResponse<List<DepartmentDTO>>> Handle(FetchDepartmentsQuery request, CancellationToken cancellationToken)
    {
        var departments = await _sharedServices.GetAllDepartmentsAsync();
        return ApiResponseHandler.Success(departments.Value ?? []);
    }
}