using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.ManagerDTOs;

namespace SoccerPro.Application.Features.ManagersFeature.Queries.GetManager;

public record GetManagerQuery(int ManagerId) : IRequest<ApiResponse<ManagerViewDTO>>;