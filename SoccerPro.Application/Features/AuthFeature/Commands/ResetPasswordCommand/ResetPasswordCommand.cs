using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.AuthDTOs;

namespace SoccerPro.Application.Features.AuthFeature.Commands.ResetPasswordCommand
{
    public class ResetPasswordCommand : IRequest<ApiResponse<bool>>
    {
        public ResetPasswordCommand(ResetPasswordDTO resetPasswordDTO)
        {
            ResetPasswordDTO = resetPasswordDTO;
        }

        public ResetPasswordDTO ResetPasswordDTO { get; set; }
    }
}
