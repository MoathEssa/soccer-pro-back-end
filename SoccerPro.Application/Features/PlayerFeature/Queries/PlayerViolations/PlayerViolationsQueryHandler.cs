using AutoMapper;
using MediatR;
using SoccerPro.Application.Common.ApiResponse;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.PlayerDTOs;
using SoccerPro.Application.Services.IServises;

namespace SoccerPro.Application.Features.PlayerFeature.Queries.PlayerViolations
{
    public class PlayerViolationsQueryHandler : IRequestHandler<PlayerViolationsQuery, ApiResponse<List<PlayerViolationDTO>>>
    {
        private readonly IPlayerServices _playerServices;
        private readonly IMapper _mapper;

        public PlayerViolationsQueryHandler(IPlayerServices playerServices, IMapper mapper)
        {
            _playerServices = playerServices;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<PlayerViolationDTO>>> Handle(PlayerViolationsQuery request, CancellationToken cancellationToken)
        {
            var result = await _playerServices.GetPlayerViolationsAsync(request.PageNumber, request.PageSize, request.CardType);

            var palyers = _mapper.Map<List<PlayerViolationDTO>>(result.Value);

            return ApiResponseHandler.Build(palyers, result.StatusCode, result.IsSuccess);
        }
    }
}
