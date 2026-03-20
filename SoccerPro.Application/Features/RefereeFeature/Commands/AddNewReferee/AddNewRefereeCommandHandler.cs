using AutoMapper;
using MediatR;
using SoccerPro.Application.Common.ApiResponse;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.Services.IServises;
using SoccerPro.Domain.Entities;

namespace SoccerPro.Application.Features.RefereeFeature.Commands.AddNewReferee;

public class AddNewRefereeCommandHandler : IRequestHandler<AddNewRefereeCommand, ApiResponse<bool>>
{
    private readonly IRefereeServices _refereeServices;
    private readonly IMapper _mapper;

    public AddNewRefereeCommandHandler(IRefereeServices refereeServices, IMapper mapper)
    {
        _refereeServices = refereeServices;
        _mapper = mapper;
    }

    public async Task<ApiResponse<bool>> Handle(AddNewRefereeCommand request, CancellationToken cancellationToken)
    {
        var referee = _mapper.Map<Referee>(request.RefereeDTO);
        var result = await _refereeServices.AddRefereeAsync(referee, request.RefereeDTO.Username, request.RefereeDTO.IntialPassword);
        return ApiResponseHandler.Build(result.Value, result.StatusCode, result.IsSuccess);
    }
}
