using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.sharedDTOs;

namespace SoccerPro.Application.Features.sharedFeature.Queries.FetchCountries
{
    public class FetchCountriesQuery : IRequest<ApiResponse<List<CountryDTO>>>
    {

    }
}