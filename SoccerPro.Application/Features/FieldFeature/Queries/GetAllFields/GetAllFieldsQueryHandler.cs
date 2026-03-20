using MediatR;
using SoccerPro.Application.Common.ApiResponse;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.SharedDTOs;
using SoccerPro.Application.Services.IServises;

namespace SoccerPro.Application.Features.FieldFeature.Queries.GetAllFields;

public class GetAllFieldsQueryHandler : IRequestHandler<GetAllFieldsQuery, ApiResponse<List<FieldDTO>>>
{
    private readonly IFieldServices _fieldServices;

    public GetAllFieldsQueryHandler(IFieldServices fieldServices)
    {
        _fieldServices = fieldServices;
    }

    public async Task<ApiResponse<List<FieldDTO>>> Handle(GetAllFieldsQuery request, CancellationToken cancellationToken)
    {
        var result = await _fieldServices.GetAllFieldsAsync();


        return ApiResponseHandler.Build<List<FieldDTO>>(result.Value, result.StatusCode, result.IsSuccess);
    }
}