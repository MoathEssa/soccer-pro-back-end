using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.SharedDTOs;

namespace SoccerPro.Application.Features.sharedFeature.Queries.FetchDepartments;

public class FetchDepartmentsQuery : IRequest<ApiResponse<List<DepartmentDTO>>> { }