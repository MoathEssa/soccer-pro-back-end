
using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.ManagerDTOs;


namespace SoccerPro.Application.Features.ManagersFeature.Queries.SearchManagers
{


    public record SearchManagersQuery(
        string? KFUPMId,
        string? FirstName,
        string? SecondName,
        string? ThirdName,
        string? LastName,
        DateTime? DateOfBirth,
        int? NationalityId,
        string? TeamName,
        int PageNumber = 1,
        int PageSize = 10
    ) : IRequest<ApiResponse<List<ManagerSearchViewDTO>>>;

}
