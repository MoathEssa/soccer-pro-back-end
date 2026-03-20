using MediatR;
using SoccerPro.Application.Common.ApiResponse;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.AuthDTOs;
using SoccerPro.Application.Services.IServises;
namespace SoccerPro.Application.Features.AuthFeature.Commands.RegisterUser;


public class RegisterUserCommandHandler(IAuthenticationServices authenticationServices) : IRequestHandler<RegisterUserCommand, ApiResponse<AuthenticationResponseDTO>>
{
    private readonly IAuthenticationServices _authenticationServices = authenticationServices;

    async Task<ApiResponse<AuthenticationResponseDTO>> IRequestHandler<RegisterUserCommand, ApiResponse<AuthenticationResponseDTO>>.Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _authenticationServices.RegisterUserAsync(request.RegisterUserDTO);

        return ApiResponseHandler.Build(result.Value, result.StatusCode, result.IsSuccess, null, [result.Error.Message]);

    }
}