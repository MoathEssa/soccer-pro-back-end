using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.PlayerDTOs;

namespace SoccerPro.Application.Features.PlayerFeature.Commands.AddPlayer;

public class AddPlayerCommand(AddPlayerDTO addPlayerDTO) : IRequest<ApiResponse<bool>>
{
    public AddPlayerDTO AddPlayerDTO { get; set; } = addPlayerDTO;
}