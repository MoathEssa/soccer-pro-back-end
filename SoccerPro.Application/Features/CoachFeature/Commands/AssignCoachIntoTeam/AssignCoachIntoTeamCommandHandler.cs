using AutoMapper;
using MediatR;
using SoccerPro.Application.Common.ApiResponse;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.Services.IServises;
using SoccerPro.Domain.Entities;

namespace SoccerPro.Application.Features.CoachFeature.Commands.AssignCoachIntoTeam;

public class AssignCoachIntoTeamCommandHandler : IRequestHandler<AssignCoachIntoTeamCommand, ApiResponse<bool>>
{
    private readonly ICoachServices _coachServices;
    private readonly IMapper _mapper;

    public AssignCoachIntoTeamCommandHandler(ICoachServices coachServices, IMapper mapper)
    {
        _coachServices = coachServices;
        _mapper = mapper;
    }

    public async Task<ApiResponse<bool>> Handle(AssignCoachIntoTeamCommand request, CancellationToken cancellationToken)
    {

        // Map and execute
        var coachTeam = _mapper.Map<CoachTeam>(request.AssignCoachIntoTeamDTO);
        var result = await _coachServices.AssignCoachToTeamAsync(coachTeam, request.AssignCoachIntoTeamDTO.TournamentId);

        return ApiResponseHandler.Build(
            data: result.Value,
            statusCode: result.StatusCode,
            succeeded: result.IsSuccess,
            message: result.IsSuccess ? "Coach assigned to team successfully" : result.Error.Message,
            errors: result.IsSuccess ? null : [result.Error.Message]
        );
    }
}