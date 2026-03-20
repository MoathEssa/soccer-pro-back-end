using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.CoachDTOs;

namespace SoccerPro.Application.Features.CoachFeature.Commands.AddCoach;

public record AddCoachCommand(AddCoachDTO Dto) : IRequest<ApiResponse<bool>>;