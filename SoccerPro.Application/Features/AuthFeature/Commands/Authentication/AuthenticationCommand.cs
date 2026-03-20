using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.AuthDTOs;
namespace SoccerPro.Application.Features.AuthFeature.Commands.Authentication;
public class AuthenticationCommand : IRequest<ApiResponse<AuthenticationResponseDTO>>
{
    public AuthenticationRequestDTO AuthenticationRequest { set; get; }
    public AuthenticationCommand(AuthenticationRequestDTO authenticationRequest)
    {
        AuthenticationRequest = authenticationRequest;
    }

}