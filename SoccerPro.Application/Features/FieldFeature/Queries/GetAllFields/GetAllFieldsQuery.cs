using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.SharedDTOs;

namespace SoccerPro.Application.Features.FieldFeature.Queries.GetAllFields;

public record GetAllFieldsQuery() : IRequest<ApiResponse<List<FieldDTO>>>;