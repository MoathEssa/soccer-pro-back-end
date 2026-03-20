using AutoMapper;
using MediatR;
using SoccerPro.Application.Common.ApiResponse;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.PlayerDTOs;
using SoccerPro.Application.Services.IServises;

namespace SoccerPro.Application.Features.PlayerFeature.Queries.FetchPlayerById
{
    public class FetchPlayerByIdQueryHandler : IRequestHandler<FetchPlayerByIdQuery, ApiResponse<PlayerDTO>>
    {
        private readonly IPlayerServices _playerServices;
        private readonly IMapper _mapper;

        public FetchPlayerByIdQueryHandler(IPlayerServices playerServices, IMapper mapper)
        {
            _playerServices = playerServices;
            _mapper = mapper;
        }

        public async Task<ApiResponse<PlayerDTO>> Handle(FetchPlayerByIdQuery request, CancellationToken cancellationToken)
        {
            var player = await _playerServices.GetPlayerByIdAsync(request.PlayerId);
            
            return ApiResponseHandler.Build(player.Value,player.StatusCode,player.IsSuccess);
        }
    }
}