using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.TournamentDTOs;

namespace SoccerPro.Application.Features.TournamentFeature.Commands.UpdateTournament;

public record UpdateTournamentCommand(UpdateTournamentDTO UpdateTournamentDTO) : IRequest<ApiResponse<bool>>;