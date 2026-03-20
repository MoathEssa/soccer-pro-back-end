using AutoMapper;
using MediatR;
using SoccerPro.Application.Common.ApiResponse;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.sharedDTOs;
using SoccerPro.Application.Services.IServises;

namespace SoccerPro.Application.Features.sharedFeature.Queries.FetchCountries
{
    public class FetchCountriesQueryHandler : IRequestHandler<FetchCountriesQuery, ApiResponse<List<CountryDTO>>>
    {
        private readonly ISharedServices _sharedServices;
        public FetchCountriesQueryHandler(ISharedServices sharedServices)
        {
            _sharedServices = sharedServices;
        }

        public async Task<ApiResponse<List<CountryDTO>>> Handle(FetchCountriesQuery request, CancellationToken cancellationToken)
        {
            var countries = await  _sharedServices.GetAllCountriesAsync();

            return ApiResponseHandler.Success(countries.Value??[]);
        }
    }
}