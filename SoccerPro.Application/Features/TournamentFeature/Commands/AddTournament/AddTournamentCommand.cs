using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.TournamentDTOs;

namespace SoccerPro.Application.Features.TournamentFeature.Commands.AddTournament;

public record AddTournamentCommand(AddTournamentDTO AddTournamentDTO) : IRequest<ApiResponse<bool>>;