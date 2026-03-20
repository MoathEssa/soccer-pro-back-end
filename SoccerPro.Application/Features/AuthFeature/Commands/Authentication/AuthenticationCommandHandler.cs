using MediatR;
using SoccerPro.Application.Common.ApiResponse;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.AuthDTOs;
using SoccerPro.Application.Features.AuthFeature.Commands.Authentication;
using SoccerPro.Application.Services.IServises;

public class AuthenticationCommandHandler : IRequestHandler<AuthenticationCommand, ApiResponse<AuthenticationResponseDTO>>
{

    private readonly IAuthenticationServices _authenticationServices;

    public AuthenticationCommandHandler(IAuthenticationServices authenticationServices)
    {
        _authenticationServices = authenticationServices;
    }

    public async Task<ApiResponse<AuthenticationResponseDTO>> Handle(AuthenticationCommand request, CancellationToken cancellationToken)
    {

        var result = await _authenticationServices.AuthenticateUser(request.AuthenticationRequest.Username, request.AuthenticationRequest.Password);

        if (!result.IsSuccess)
            return ApiResponseHandler.Unauthorized<AuthenticationResponseDTO>(result.Error.Message);

        return ApiResponseHandler.Success(result.Value!);

    }
}