using MediatR;
using SoccerPro.Application.Common.ApiResponse;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.CoachDTOs;
using SoccerPro.Application.Services.IServises;

namespace SoccerPro.Application.Features.CoachFeature.Queries.GetAllCoaches;

public class GetAllCoachesQueryHandler
    : IRequestHandler<GetAllCoachesQuery, ApiResponse<List<CoachViewDTO>>>
{
    private readonly ICoachServices _coachServices;

    public GetAllCoachesQueryHandler(ICoachServices coachServices)
    {
        _coachServices = coachServices;
    }

    public async Task<ApiResponse<List<CoachViewDTO>>> Handle(
        GetAllCoachesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _coachServices.GetCoachesAsync(
            request.KFUPMId,
            request.FirstName,
            request.SecondName,
            request.ThirdName,
            request.LastName,
            request.DateOfBirth,
            request.NationalityId,
            request.TeamName,
            request.IsActive,
            request.PageNumber,
            request.PageSize
        );

        return ApiResponseHandler.Build(
            data: result.Value.coaches,
            statusCode: result.StatusCode,
            succeeded: result.IsSuccess,
            message: result.IsSuccess ? "Coaches retrieved successfully" : result.Error.Message,
            errors: result.IsSuccess ? null : [result.Error.Message],
            meta: new
            {
                count = result.Value.totalCount
            }

        );
    }
}