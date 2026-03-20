using AutoMapper;
using MediatR;
using SoccerPro.Application.Common.ApiResponse;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.Services.IServises;
using SoccerPro.Domain.Entities;

namespace SoccerPro.Application.Features.PlayerFeature.Commands.AssignPlayersIntoTeam;

public class AssignPlayerIntoTeamCommandHandler : IRequestHandler<AssignPlayerIntoTeamCommand, ApiResponse<bool>>
{
    private readonly IPlayerServices _playerServices;
    private readonly IMapper _mapper;

    public AssignPlayerIntoTeamCommandHandler(IPlayerServices playerServices, IMapper mapper)
    {
        _playerServices = playerServices;
        _mapper = mapper;
    }

    public async Task<ApiResponse<bool>> Handle(AssignPlayerIntoTeamCommand request, CancellationToken cancellationToken)
    {
        PlayerTeam playerTeam = _mapper.Map<PlayerTeam>(request.AssignPlayerIntoTeamDTO);

        var result = await _playerServices.AssignPlayerToTeamAsync(playerTeam, request.AssignPlayerIntoTeamDTO.TournamentId);

        return ApiResponseHandler.Build(
            data: result.Value,
            statusCode: result.StatusCode,
            succeeded: result.IsSuccess,
            message: result.IsSuccess ? "Players assigned to team successfully" : result.Error.Message,
            errors: result.IsSuccess ? null : [result.Error.Message]
        );
    }
}