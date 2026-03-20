using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.AuthDTOs;

namespace SoccerPro.Application.Features.AuthFeature.Queries.GetAllRoles;

public record GetAllRolesQuery : IRequest<ApiResponse<List<RoleDTO>>>;