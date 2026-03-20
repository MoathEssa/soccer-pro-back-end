using AutoMapper;
using MediatR;
using SoccerPro.Application.Common.ApiResponse;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.Services.IServises;
using SoccerPro.Domain.Entities;

namespace SoccerPro.Application.Features.ManagersFeature.Commands.AddManager;

public class AddManagerCommandHandler : IRequestHandler<AddManagerCommand, ApiResponse<bool>>
{
    private readonly IManagerServices _managerServices;
    private readonly IMapper _mapper;

    public AddManagerCommandHandler(IManagerServices managerServices, IMapper mapper)
    {
        _managerServices = managerServices;
        _mapper = mapper;
    }

    public async Task<ApiResponse<bool>> Handle(AddManagerCommand request, CancellationToken cancellationToken)
    {
        var manager = _mapper.Map<Manager>(request.AddManagerDTO);
        var result = await _managerServices.AddManagerAsync(manager, request.AddManagerDTO.UserName, request.AddManagerDTO.IntialPassword);
        return ApiResponseHandler.Build(result.Value, result.StatusCode, result.IsSuccess, null, [result.Error.Message]);
    }
}