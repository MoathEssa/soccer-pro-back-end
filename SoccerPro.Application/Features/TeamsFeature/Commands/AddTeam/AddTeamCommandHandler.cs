using AutoMapper;
using MediatR;
using SoccerPro.Application.Common.ApiResponse;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.Services.IServises;
using SoccerPro.Domain.Entities;

namespace SoccerPro.Application.Features.TeamsFeature.Commands.AddTeam;

public class AddTeamCommandHandler : IRequestHandler<AddTeamCommand, ApiResponse<bool>>
{
    private readonly ITeamServices _teamServices;
    private readonly IMapper _mapper;

    public AddTeamCommandHandler(ITeamServices teamServices, IMapper mapper)
    {
        _teamServices = teamServices;
        _mapper = mapper;
    }

    public async Task<ApiResponse<bool>> Handle(AddTeamCommand request, CancellationToken cancellationToken)
    {
        var team = _mapper.Map<Team>(request.AddTeamDTO);
        var result = await _teamServices.AddTeamAsync(team);

        return ApiResponseHandler.Build(result.Value, result.StatusCode, result.IsSuccess, null, [result.Error.Message]);
    }
}