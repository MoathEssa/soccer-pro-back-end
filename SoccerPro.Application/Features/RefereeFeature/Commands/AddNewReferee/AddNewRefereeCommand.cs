using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.RefereeDTOs;

namespace SoccerPro.Application.Features.RefereeFeature.Commands.AddNewReferee;

public record AddNewRefereeCommand(AddRefereeDTO RefereeDTO) : IRequest<ApiResponse<bool>>;
