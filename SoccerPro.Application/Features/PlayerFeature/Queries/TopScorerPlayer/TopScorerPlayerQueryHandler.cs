using AutoMapper;
using MediatR;
using SoccerPro.Application.Common.ApiResponse;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.PlayerDTOs;
using SoccerPro.Application.Services.IServises;

namespace SoccerPro.Application.Features.PlayerFeature.Queries.TopScorerPlayer
{
    public class TopScorerPlayerQueryHandler : IRequestHandler<TopScorerPlayerQuery, ApiResponse<List<TopScorerPlayerDTO>>>
    {
        private readonly IPlayerServices _playerServices;
        private readonly IMapper _mapper;

        public TopScorerPlayerQueryHandler(IPlayerServices playerServices, IMapper mapper)
        {
            _playerServices = playerServices;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<TopScorerPlayerDTO>>> Handle(TopScorerPlayerQuery request, CancellationToken cancellationToken)
        {
            var result = await _playerServices.GetTopScorersAsync(request.PageNumber, request.PageSize);
            var playerDTO = _mapper.Map<List<TopScorerPlayerDTO>>(result.Value);
            return ApiResponseHandler.Build(playerDTO, result.StatusCode, result.IsSuccess);
        }
    }
}
