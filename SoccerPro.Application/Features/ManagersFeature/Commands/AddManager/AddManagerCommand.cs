using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.ManagerDTOs;

namespace SoccerPro.Application.Features.ManagersFeature.Commands.AddManager;

public record AddManagerCommand(AddManagerDTO dto) : IRequest<ApiResponse<bool>>
{
    public AddManagerDTO AddManagerDTO { get; set; } = dto;
}