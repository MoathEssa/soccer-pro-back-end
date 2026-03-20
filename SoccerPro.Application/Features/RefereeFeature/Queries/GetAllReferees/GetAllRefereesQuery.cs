using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.RefereeDTOs;

namespace SoccerPro.Application.Features.RefereeFeature.Queries.GetAllReferees;

public record GetAllRefereesQuery : IRequest<ApiResponse<List<RefereeViewDTO>>>;
