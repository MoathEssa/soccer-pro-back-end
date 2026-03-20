using MediatR;
using SoccerPro.Application.Common.ApiResponse;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.Services.IServises;

namespace SoccerPro.Application.Features.AuthFeature.Commands.ResetPasswordCommand
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ApiResponse<bool>>
    {
        private readonly IAuthenticationServices _authenticationServices;

        public ResetPasswordCommandHandler(IAuthenticationServices authenticationServices)
        {
            _authenticationServices = authenticationServices;
        }

        async Task<ApiResponse<bool>> IRequestHandler<ResetPasswordCommand, ApiResponse<bool>>.Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await _authenticationServices.ResetPasswordDirectlyAsync(request.ResetPasswordDTO.UserId, request.ResetPasswordDTO.NewPassword);

            return ApiResponseHandler.Build(
                data: result.Value,
                statusCode: result.StatusCode,
                succeeded: result.IsSuccess,
                message: result.IsSuccess ? "Password reset successfully" : result.Error.Message,
                errors: [result.Error.Message]
            );
        }
    }

}
