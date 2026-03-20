namespace SoccerPro.Application.Features.PlayerFeature.Commands.AddPlayer;

using AutoMapper;
using MediatR;
using SoccerPro.Application.Common.ApiResponse;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.Services.IServises;
using SoccerPro.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

public class AddPlayerCommandHandler : IRequestHandler<AddPlayerCommand, ApiResponse<bool>>
{
    private readonly IPlayerServices _playerServices;
    private readonly IMapper _mapper;
    public AddPlayerCommandHandler(IMapper mapper, IPlayerServices playerServices)
    {
        _mapper = mapper;
        _playerServices = playerServices;
    }

    public async Task<ApiResponse<bool>> Handle(AddPlayerCommand request, CancellationToken cancellationToken)
    {
        // Map the command to the Player entity using AutoMapper
        Player player = _mapper.Map<Player>(request.AddPlayerDTO);

        // Call the service to add the player
        var result = await _playerServices.AddPlayerAsync(player, request.AddPlayerDTO.UserName, request.AddPlayerDTO.IntialPassword);

        return ApiResponseHandler.Build(
            data: result.Value,
            statusCode: result.StatusCode,
            succeeded: result.IsSuccess,
            message: result.IsSuccess ? "Player created successfully" : result.Error.Message,
            errors: result.IsSuccess ? null : [result.Error.Message]
        );

    }

}