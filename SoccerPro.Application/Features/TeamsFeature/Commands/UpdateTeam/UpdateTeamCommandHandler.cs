using AutoMapper;
using MediatR;
using SoccerPro.Application.Common.ApiResponse;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.Services.IServises;
using SoccerPro.Domain.Entities;

namespace SoccerPro.Application.Features.TeamsFeature.Commands.UpdateTeam;

public class UpdateTeamCommandHandler : IRequestHandler<UpdateTeamCommand, ApiResponse<bool>>
{
    private readonly ITeamServices _teamServices;
    private readonly IMapper _mapper;

    public UpdateTeamCommandHandler(ITeamServices teamServices, IMapper mapper)
    {
        _teamServices = teamServices;
        _mapper = mapper;
    }

    public async Task<ApiResponse<bool>> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
    {
        var team = _mapper.Map<Team>(request.UpdateTeamDTO);
        var result = await _teamServices.UpdateTeamAsync(team);

        return ApiResponseHandler.Build(result.Value, result.StatusCode, result.IsSuccess, null, [result.Error.Message]);
    }
}