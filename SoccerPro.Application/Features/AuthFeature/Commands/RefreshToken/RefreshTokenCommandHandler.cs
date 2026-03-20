using MediatR;
using SoccerPro.Application.Common.ApiResponse;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.AuthDTOs;
using SoccerPro.Application.Services.IServises;

namespace SoccerPro.Application.Features.AuthFeature.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ApiResponse<AuthenticationResponseDTO>>
    {
        private readonly IAuthenticationServices _authenticationService;

        public RefreshTokenCommandHandler(IAuthenticationServices authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<ApiResponse<AuthenticationResponseDTO>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.RefreshTokenAsync(request.RefreshToken);

            return ApiResponseHandler.Build(result.Value, result.StatusCode, result.IsSuccess, null, [result.Error.Message]);
        }
    }
}
