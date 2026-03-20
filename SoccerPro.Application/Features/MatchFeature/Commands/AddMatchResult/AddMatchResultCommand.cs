using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.MatchDTOs;

namespace SoccerPro.Application.Features.MatchFeature.Commands.AddMatchResult;

public record AddMatchResultCommand(AddMatchRecordDTO Dto) : IRequest<ApiResponse<bool>>;