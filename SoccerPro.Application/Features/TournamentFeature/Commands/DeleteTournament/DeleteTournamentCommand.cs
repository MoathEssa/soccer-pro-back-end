using MediatR;
using SoccerPro.Application.Common.ResultPattern;

namespace SoccerPro.Application.Features.TournamentFeature.Commands.DeleteTournament;

public record DeleteTournamentCommand(int TournamentId) : IRequest<ApiResponse<bool>>;