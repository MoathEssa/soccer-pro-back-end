using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.AuthDTOs;

namespace SoccerPro.Application.Features.AuthFeature.Commands.RegisterUser;   
public class RegisterUserCommand(RegsterAccountRequestDTO registerUserDTO) : IRequest<ApiResponse<AuthenticationResponseDTO>>
{
    public RegsterAccountRequestDTO RegisterUserDTO { get; set; } = registerUserDTO;
}
