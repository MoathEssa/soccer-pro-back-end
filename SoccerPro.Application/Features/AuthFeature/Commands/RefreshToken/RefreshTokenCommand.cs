using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.AuthDTOs;

namespace SoccerPro.Application.Features.AuthFeature.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<ApiResponse<AuthenticationResponseDTO>>
    {
        public RefreshTokenCommand(string refreshToken)
        {
            RefreshToken = refreshToken;
        }

        public string RefreshToken { get; set; }
    }
}
